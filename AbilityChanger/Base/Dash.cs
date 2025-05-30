using Microsoft.SqlServer.Server;

namespace AbilityChanger.Base
{
    public abstract class Dash: Ability
    {
        public override string abilityType => Abilities.DASH;

        #region Trigger
        private Func<bool> triggerAction;
        /// <summary>
        /// Register an action to be called when the dash button is pressed.
        /// </summary>
        /// <param name="action"> The action to be called. It must return <c>true</c> if it wants the default behaviour to continue, <c>false</c> if it doesn't</param>
        public void OnTrigger(Func<bool> action)
        {
            triggerAction = action;
            OnSelect += () => { On.HeroController.HeroDash += DashAbilityTrigger; };
            OnUnselect += () => { On.HeroController.HeroDash -= DashAbilityTrigger; };
        }
        private void DashAbilityTrigger(On.HeroController.orig_HeroDash orig, HeroController self) { if (triggerAction()) orig(self); } 
        #endregion

        #region onDash
        private Func<bool> dashingAction;
        /// <summary>
        /// Register an action to be called every frame while dashing
        /// </summary>
        /// <param name="action"> The action to be called. It must return <c>true</c> if it wants the default behaviour to continue, <c>false</c> if it doesn't</param>
        public void OnDashing(Func<bool> action)
        {
            dashingAction = action;
            OnSelect += () => { On.HeroController.Dash += OnDash; };
            OnUnselect += () => { On.HeroController.Dash -= OnDash; };
        }
        private void OnDash(On.HeroController.orig_Dash orig, HeroController self) {if (dashingAction()) { orig(self); }; }
        #endregion

    }
}
