namespace AbilityChanger
{
    public class DashSlashManager : AbilityManagerFSM
    {

        public override string abilityName { get; protected set; } = Abilities.DASHSLASH;
        public override bool hasDefaultAbility() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasUpwardSlash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_UPPER";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_UPPER";

        public override List<string> relatedManagers => new() { Abilities.GREATSLASH,Abilities.CYCLONESLASH};

        public override string fsmName => AbilitiesFSMs.NAILARTS;

        public DashSlashManager() : base() { }
        public override GameObject getIconGo() =>  InvGo.Find("Art Uppercut");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Uppercut",
                    eventName = "UI CONFIRM",
                    onIntercept = () =>
                    {
                        currentAbility = nextAbility();
                        updateInventory();
                    }
                });
            }
        }


    }
}