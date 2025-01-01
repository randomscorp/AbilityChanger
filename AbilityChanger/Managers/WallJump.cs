namespace AbilityChanger
{
    public class WallJump : AbilityManager {
        public override string abilityName { get; protected set; } = Abilities.WALLJUMP;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasWalljump);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_WALLJUMP";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_WALLJUMP";
        public WallJump() : base (){
        }

        public override GameObject getIconGo() =>  InvGo.Find("Mantis Claw");

    }
}