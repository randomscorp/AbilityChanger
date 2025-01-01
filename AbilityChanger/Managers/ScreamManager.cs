using System.Data;

namespace AbilityChanger
{
    public class ScreamManager : AbilityManagerFSM 
    {
        public override string abilityName { get; protected set; } = Abilities.SCREAM;
        public override bool hasDefaultAbility()  => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.screamLevel)) > 0;
        public override string inventoryTitleKey
        {
            get
            {
                return $"INV_NAME_SPELL_SCREAM{PlayerData.instance.GetIntInternal(nameof(PlayerData.screamLevel))}";
            }
            protected set { }
        }
        public override string inventoryDescKey
        {
            get
            {
                return $"INV_DESC_SPELL_SCREAM{PlayerData.instance.GetIntInternal(nameof(PlayerData.screamLevel))}";
            }
            protected set { }
        }

        public override List<string> relatedManagers => new() {Abilities.QUAKE, Abilities.FOCUS, Abilities.FIREBALL };

        public override string fsmName => AbilitiesFSMs.SPELLCONTROL;

        public ScreamManager() : base() { }
        public override GameObject getIconGo() =>  InvGo.Find("Spell Scream");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Scream",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentAbility = nextAbility();
                        updateInventory();
                    }
                });
            }
        }
    }
}
