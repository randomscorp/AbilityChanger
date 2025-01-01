namespace AbilityChanger
{
    public class Nail: AbilityManager
    {
        public override string abilityName { get; protected set; } = Abilities.NAIL;
        public override bool hasDefaultAbility()  => true;
        public override string inventoryTitleKey
        {
            get
            {
                return $"INV_NAME_NAIL{PlayerData.instance.GetIntInternal(nameof(PlayerData.nailSmithUpgrades)) + 1}";
            }
            protected set { }
        }
        public override string inventoryDescKey
        {
            get
            {
                return $"INV_DESC_NAIL{PlayerData.instance.GetIntInternal(nameof(PlayerData.nailSmithUpgrades)) + 1}";
            }
            protected set { }
        }

        public Nail() : base()
        {
        }
        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Nail",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentAbility = nextAbility();
                        updateInventory();
                    }
                });
            }

        }
        public override GameObject getIconGo() =>  InvGo.Find("Nail");
    }
}
