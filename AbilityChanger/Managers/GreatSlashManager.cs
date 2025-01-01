namespace AbilityChanger
{
    public class GreatSlashManager : AbilityManagerFSM{
       
        public override string abilityName { get; protected set; } = Abilities.GREATSLASH;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDashSlash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_DASH";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_DASH";

        public override List<string> relatedManagers => new() {Abilities.CYCLONESLASH, Abilities.DASHSLASH };

        public override string fsmName => AbilitiesFSMs.NAILARTS;

        public GreatSlashManager() : base (){}
        public override GameObject getIconGo() =>  InvGo.Find("Art Dash");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Dash Slash",
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