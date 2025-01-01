namespace AbilityChanger.Managers
{
    public class DreamgateManager : AbilityManagerFSM
    {
        public override string abilityName { get; protected set; } = Abilities.DREAMGATE;
        public override bool hasDefaultAbility() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDreamGate);
        public override List<string> relatedManagers => new() { Abilities.DREAMNAIL };
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DREAMGATE";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DREAMGATE";
        public override string fsmName => AbilitiesFSMs.DREAMNAIL;
        public DreamgateManager() : base (){}
        public override GameObject getIconGo() => InvGo.Find("Dream Gate");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Dream Gate",
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