namespace AbilityChanger.Base
{
    public abstract class Dash: Ability
    {
        public override string abilityType => Abilities.DASH;

        #region Trigger
        private Func<bool> onHeroDash;
        /// <summary>
        /// Register a function to be called when the dash button is pressed.
        /// </summary>
        /// <param name="func"> The function to be called. It must return <c>true</c> if it wants the default behaviour to continue, <c>false</c> if it doesn't</param>
        public void RegisterOnHeroDash(Func<bool> func)
        {
            onHeroDash = func;
            OnSelect += () => { On.HeroController.HeroDash += DashAbilityTrigger; };
            OnUnselect += () => { On.HeroController.HeroDash -= DashAbilityTrigger; };
        }
        private void DashAbilityTrigger(On.HeroController.orig_HeroDash orig, HeroController self) { if (onHeroDash()) orig(self); } 
        #endregion

        #region onDash
        private Func<bool> onDash;
        /// <summary>
        /// Register a function to be called every frame while dashing
        /// </summary>
        /// <param name="func"> The function to be called. It must return <c>true</c> if it wants the default behaviour to continue, <c>false</c> if it doesn't</param>
        public void RegisterOnDash(Func<bool> func)
        {
            onDash= func;
            OnSelect += () => { On.HeroController.Dash += OnDash; };
            OnUnselect += () => { On.HeroController.Dash -= OnDash; };
        }

        private void OnDash(On.HeroController.orig_Dash orig, HeroController self) {if (onDash()) { orig(self); }; }
        #endregion

        #region Finished Dashing
        private Func<bool> onFinishedDashing;
        /// <summary>
        /// Register a function to be called when the dash animation finish
        /// </summary>
        /// <param name="func"> The function to be called. It must return <c>true</c> if it wants the default behaviour to continue, <c>false</c> if it doesn't</param>
        public void RegisterFinishedDashing(Func<bool> func)
        {
            onFinishedDashing = func;
            OnSelect += () => { On.HeroController.FinishedDashing += FinishedDashing; };
            OnUnselect += () => { On.HeroController.FinishedDashing -= FinishedDashing; };
        }
        private void FinishedDashing(On.HeroController.orig_FinishedDashing orig, HeroController self) { if (onFinishedDashing()) orig(self); }
        #endregion

    }
}
