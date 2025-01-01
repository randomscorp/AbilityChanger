namespace AbilityChanger
{
    public class DreamNailManager : AbilityManagerFSM
    {
        public override string abilityName { get; protected set; } = Abilities.DREAMNAIL;
        public override bool hasDefaultAbility() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDreamNail);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DREAMNAIL";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DREAMNAIL";

        public override List<string> relatedManagers => new() { Abilities.DREAMGATE};

        public override string fsmName => AbilitiesFSMs.DREAMNAIL;

        public override GameObject getIconGo() =>  InvGo.Find("Dream Nail");
        public DreamNailManager() : base()
        {
        }
        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
        
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Dream Nail",
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