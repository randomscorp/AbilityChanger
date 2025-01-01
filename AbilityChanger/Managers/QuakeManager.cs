namespace AbilityChanger
{
    public class QuakeManager : AbilityManagerFSM
    {
       
        public override string abilityName { get; protected set; } = Abilities.QUAKE;
        public override bool hasDefaultAbility()  => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.quakeLevel)) > 0;
        public override string inventoryTitleKey
        {
            get
            {
                return $"INV_NAME_SPELL_QUAKE{PlayerData.instance.GetIntInternal(nameof(PlayerData.quakeLevel))}";
            }
            protected set { }
        }
        public override string inventoryDescKey
        {
            get
            {
                return $"INV_DESC_SPELL_QUAKE{PlayerData.instance.GetIntInternal(nameof(PlayerData.quakeLevel))}";
            }
            protected set { }
        }

        public override List<string> relatedManagers => new() {Abilities.FIREBALL,Abilities.SCREAM,Abilities.FOCUS };

        public override string fsmName => AbilitiesFSMs.SPELLCONTROL;

        public QuakeManager() : base() {}
        public override GameObject getIconGo() =>  InvGo.Find("Spell Quake");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Quake",
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
