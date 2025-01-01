using System.Linq.Expressions;
using System.Reflection;
using static Satchel.FsmUtil;

namespace AbilityChanger
{
    public abstract class AbilityManagerFSM: AbilityManager {

        public abstract string fsmName { get; }

        internal override Ability setAbility(Ability ability, List<AbilityManager> prevManagers = null)
        {
            // For FSM abilities we need to enable and reset the right FSM when switching        
            AbilityChanger.FsmMap[fsmName].enabled = false;
            AbilityChanger.VannilaFsmMap[fsmName].enabled = false;

            // Enables the vannila FSM when all current abilities are 
            if (!ability.isCustom && relatedManagers.All((name) => !AbilityChanger.ManagersMap[name].isCustom() ))
            {
                AbilityChanger.VannilaFsmMap[fsmName].enabled = true;
            }
            // Resets the AC's FSM and enables it
            else
            {
                // TODO: there's probably a non-reflection way of doing this 
                var fsmInfo = typeof(PlayMakerFSM).GetField("fsm", BindingFlags.NonPublic | BindingFlags.Instance);
                fsmInfo.SetValue(AbilityChanger.FsmMap[fsmName], new Fsm(AbilityChanger.BackupFsmMap[fsmName]));

                var awakeInfo = typeof(PlayMakerFSM).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
                awakeInfo.Invoke(AbilityChanger.FsmMap[fsmName], new object[] { });

                AbilityChanger.FsmMap[fsmName].FsmName += " AC";
                AbilityChanger.FsmMap[fsmName].enabled = true;

            }
            return base.setAbility(ability,prevManagers);
        }
    }
}