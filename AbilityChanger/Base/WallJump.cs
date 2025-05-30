namespace AbilityChanger.Base
{
    public abstract class WallJump: Ability
    {
        public override string abilityType => Abilities.WALLJUMP;

        #region Trigger
        private Func<bool> onTriggerAction;
        /// <summary>
        /// Register a function to be called when the walljump animation starts
        /// </summary>
        /// <param name="func"> The function to be called. It must return <c> true</c> if it wants the default behaviour to continue, <c>false</c> if it doesn't</param>
        public void OnTrigger(Func<bool> func)
        {
            onTriggerAction = func;
            OnSelect += () => { On.HeroController.DoWallJump += WallJumpAbilityTrigger; };
            OnUnselect += () => { On.HeroController.DoWallJump -= WallJumpAbilityTrigger; };
        }
        public void WallJumpAbilityTrigger(On.HeroController.orig_DoWallJump orig, HeroController self)
        {
           if (onTriggerAction()) orig(self);
        }
        #endregion
    }
}
