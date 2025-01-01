
using Satchel.Futils.Serialiser;
using System.Reflection;
using System.Runtime.CompilerServices;
using AbilityChanger.Managers;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using System.CodeDom;
using UnityEngine;

namespace AbilityChanger
{
    public class AbilityChanger : Mod, ILocalSettings<SaveSettings>
    {
        public override string GetVersion() => "1.0";
        public static AbilityChanger instance;

        /// <summary>
        /// Dictionary that holds every ability manager 
        /// </summary>
        internal static Dictionary<string, AbilityManager> ManagersMap = new(){
            { Abilities.DREAMGATE,new DreamgateManager()},
            { Abilities.CYCLONESLASH,new CycloneSlashManager()},
            { Abilities.GREATSLASH,new GreatSlashManager()},
            { Abilities.DASHSLASH,new DashSlashManager()},
            { Abilities.DASH,new DashManager()},
            { Abilities.DOUBLEJUMP,new DoubleJumpManager()},
            { Abilities.WALLJUMP,new WallJump()},
            { Abilities.NAIL,new Nail()},
            { Abilities.FIREBALL,new FireballManager()},
            { Abilities.QUAKE,new QuakeManager()},
            { Abilities.SCREAM,new ScreamManager()},
            {Abilities.FOCUS,new FocusManager()},
            {Abilities.SUPERDASH, new SuperDashManager() },
            {Abilities.DREAMNAIL, new DreamNailManager() },

        };

        internal static Dictionary<string, PlayMakerFSM> FsmMap;
        internal static Dictionary<string, PlayMakerFSM> VannilaFsmMap;
        internal static Dictionary<string, Fsm> BackupFsmMap;


        /// <summary>
        /// Register a new ability for the given ability type
        /// </summary>
        /// <param name="ability">An Ability</param>
        public static void RegisterAbility(Ability ability)
        {
            if (ManagersMap[ability.abilityType].acquiredAbilities().Contains(ability)) return;
            ManagersMap[ability.abilityType].addAbility(ability);
            AbilityChanger.instance.LogDebug($"Ability [{ability.name}] registered");
        }
        /// <summary>
        /// Deregister an ability from ability changer
        /// </summary>
        /// <param name="ability">An Ability</param>
        public static void DeregisterAbility(Ability ability)
        {
            ManagersMap[ability.abilityType].removeAbility(ability.name);
            AbilityChanger.instance.LogDebug($"Ability [{ability.name}] dergistered");

        }
        public override void Initialize()
        {
            instance ??= this;
            ModHooks.GetPlayerBoolHook += PlayerDataPatcher.OnGetPlayerBoolHook;
            On.PlayMakerFSM.OnEnable += Equipment.OnFsmEnable;
            On.HeroController.Start += ACStart;
        }

        private void ACStart(On.HeroController.orig_Start orig, HeroController self)
        {

            //Copy the FSMs
            VannilaFsmMap = new();
            FsmMap = new();
            BackupFsmMap = new();
            GameObject hero = HeroController.instance.gameObject;

            foreach (string name in AbilitiesFSMs.names)
            {
                PlayMakerFSM vannilaFSM = hero.LocateMyFSM(name);

                // We need 3 versions of the same FSM, the first is kept as the vannila to hold FSM changes made by other mods 
                // The second is the actual modified FSM and the third is the backup where we reset from every change
                VannilaFsmMap.Add(name, vannilaFSM);
                FsmMap.Add(name, hero.CloneFsm(vannilaFSM));
                BackupFsmMap.Add(name, new Fsm(vannilaFSM.Fsm));
            }

            // Iniates abilities and equipes the saved ones
            foreach (KeyValuePair<string, AbilityManager> manager in ManagersMap)
            {
                if (settings.equipedAbilities.ContainsKey(manager.Key) && manager.Value.currentAbility.name != settings.equipedAbilities[manager.Key])
                {
                    var ability = manager.Value.getAbility(settings.equipedAbilities[manager.Key]);
                    if (ability.aquiredAbility) manager.Value.setAbility(ability);
                }
            }

            orig(self);
        }
        public static SaveSettings settings { get; set; } = new();

        public void OnLoadLocal(SaveSettings s)
        {
            AC.settings = s;
            // Load save data
            if (ManagersMap != null && ManagersMap.Count > 0)
            {
                foreach (KeyValuePair<string, AbilityManager> manager in ManagersMap)
                {
                    var abilities = manager.Value.registeredAbilities();
                    if (abilities != null)
                    {
                        foreach (var ability in abilities)
                        {
                            var saved = s.savedAbilities.Find((a) => (ability.name == a.abilityName) && (a.managerName == manager.Key));
                            if (saved != null)
                            {
                                ability.setAquireAbility(saved.aquiredAbility);

                            }
                            else ability.setAquireAbility(false);
                        }
                    }
                }

            }
        }

        public SaveSettings OnSaveLocal()
        {
            SaveSettings s = new SaveSettings();

            foreach (KeyValuePair<string, AbilityManager> manager in ManagersMap)
            {
                foreach (Ability ability in manager.Value.registeredAbilities())
                {
                    if (ability.isCustom)
                    {
                        SavedAbility abilityToSave = new SavedAbility()
                        {
                            abilityName = ability.name,
                            managerName = manager.Key,
                            aquiredAbility = ability.aquiredAbility,
                        };
                        s.savedAbilities.Add(abilityToSave);
                    }
                }
                if (manager.Value.currentAbility.isCustom) s.equipedAbilities.Add(manager.Key, manager.Value.currentAbility.name);
            }
            return s;
        }
    }

    public static class ComponentExtensions
    {
        /// <summary>
        /// Clones a PlayMakerFSM and adds it to the GO
        /// </summary>
        /// <param name="go"> The GameObject which will hold the cloned FSM</param>
        /// <param name="toAdd"> The PlayMakerFSM to clone</param>
        /// <returns>Returns a reference to the cloned PMFSM</returns>
        public static PlayMakerFSM CloneFsm(this GameObject go, PlayMakerFSM toAdd)// where T : Component
        {
            // Creates new FSM to hold the clone
            PlayMakerFSM newFsm = go.AddComponent<PlayMakerFSM>();
            
            // Keeps the Vannila FSM as the only one enabled;
            // PMFSM does werid things when you have the two intsnaces of the same FSM enabled
            newFsm.enabled = false;

            // Use reflection to clone the Fsm field (it is what actually holds the states and transitions)
            FieldInfo fsmInfo = typeof(PlayMakerFSM).GetField("fsm", BindingFlags.NonPublic | BindingFlags.Instance);
            fsmInfo.SetValue(newFsm, new Fsm(toAdd.Fsm));

            // Use reflection to call the private 'Awake' method needed to set up the transitions between states
            MethodInfo awakeInfo = typeof(PlayMakerFSM).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
            awakeInfo.Invoke(newFsm, new object[] { });
            
            newFsm.FsmName += " AC";
            return newFsm;  
        }
    }

}
