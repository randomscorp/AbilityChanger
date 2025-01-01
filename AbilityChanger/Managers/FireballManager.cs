using Satchel.Futils;

namespace AbilityChanger
{
    public class FireballManager : AbilityManagerFSM
    {

        public override string abilityName { get; protected set; } = Abilities.FIREBALL;
        public override bool hasDefaultAbility() => (PlayerDataPatcher.GetIntInternal(PlayerDataPatcher.fireballLevel)) > 0;
        public override string inventoryTitleKey {
            get => $"INV_NAME_SPELL_FIREBALL{PlayerData.instance.GetIntInternal(nameof(PlayerData.fireballLevel))}";

            protected set { }
        }
        public override string inventoryDescKey
        {
            get => $"INV_DESC_SPELL_FIREBALL{PlayerData.instance.GetIntInternal(nameof(PlayerData.fireballLevel))}";
            protected set { }
        }

        public override List<string> relatedManagers => new() {Abilities.FOCUS,Abilities.SCREAM,Abilities.QUAKE };

        public override string fsmName => AbilitiesFSMs.SPELLCONTROL;

        public FireballManager() : base() { }
        public override GameObject getIconGo() =>  InvGo.Find("Spell Fireball");

        public override void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            if (self.gameObject.name == "Inv" && self.FsmName == "UI Inventory")
            {
                self.Intercept(new EventInterceptor()
                {
                    fromState = "Fireball",
                    eventName = "UI CONFIRM",
                    onIntercept = () => {
                        currentAbility = nextAbility();
                        updateInventory();
                    }
                });
            }
            orig(self);
        }

        
    }
}
