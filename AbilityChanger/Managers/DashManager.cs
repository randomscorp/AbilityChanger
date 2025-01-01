namespace AbilityChanger
{
    public class DashManager : AbilityManager
    {
  
        public override string abilityName { get; protected set; } = Abilities.DASH;
        public override bool hasDefaultAbility()  => PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasDash) || PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.canDash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_DASH";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_DASH";
        public DashManager() : base (){

        }
        public override GameObject getIconGo() =>  InvGo.Find("Dash Cloak");

      
    }
}