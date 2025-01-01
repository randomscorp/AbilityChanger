namespace AbilityChanger
{
    public class FocusManager : AbilityManagerFSM

    {

        public override string abilityName { get; protected set; } = Abilities.FOCUS;
        public override bool hasDefaultAbility()  => true;
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_SPELL_FOCUS";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_SPELL_FOCUS";

        public override List<string> relatedManagers => new() { Abilities.SCREAM,Abilities.FIREBALL,Abilities.QUAKE};

        public override string fsmName => AbilitiesFSMs.SPELLCONTROL;

        public FocusManager() : base() { }
        public override GameObject getIconGo() =>  InvGo.Find("Spell Focus");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);   
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Focus",
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
