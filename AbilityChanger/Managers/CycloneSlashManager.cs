namespace AbilityChanger
{
    public class CycloneSlashManager : AbilityManagerFSM {

        public override string abilityName { get; protected set; } = Abilities.CYCLONESLASH;
        public override bool hasDefaultAbility() => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasCyclone);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_ART_CYCLONE";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_ART_CYCLONE";

        public override string fsmName => AbilitiesFSMs.NAILARTS;

        public override List<string> relatedManagers => new() { Abilities.DASHSLASH, Abilities.GREATSLASH};

        public CycloneSlashManager() : base() { }
        public override GameObject getIconGo() =>  InvGo.Find("Art Cyclone");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor(){
                    fromState = "Cyclone",
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


