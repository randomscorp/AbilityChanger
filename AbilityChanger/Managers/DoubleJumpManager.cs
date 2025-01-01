namespace AbilityChanger
{
    public class DoubleJumpManager : AbilityManager {
       
        public override string abilityName { get; protected set; } = Abilities.DOUBLEJUMP;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDoubleJump);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DOUBLEJUMP";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DOUBLEJUMP";
        public DoubleJumpManager() : base (){
        }
        public override GameObject getIconGo() =>  InvGo.Find("Double Jump");
    }
}