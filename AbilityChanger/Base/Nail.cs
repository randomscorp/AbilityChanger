namespace AbilityChanger.Base
{
    public abstract class Nail: Ability
    {
        public override string abilityType => Abilities.NAIL;

        #region Attack
        private Func<bool> onTriggerAction;
        /// <summary>
        /// Register a function to be called when the nail button is pressed
        /// </summary>
        /// <param name="func"> The function to be called. It must return <c> true</c> if it wants the default behaviour to continue, <c>false</c> if it doesn't</param>
        public void RegisterAttack(Func<bool> func)
        {
            onTriggerAction = func;
            OnSelect += () => { On.HeroController.Attack += NailTrigger; };
            OnUnselect += () => { On.HeroController.Attack -= NailTrigger; };
        }
        private void NailTrigger(On.HeroController.orig_Attack orig, HeroController self, GlobalEnums.AttackDirection attackDir)
        {
            if (onTriggerAction()) orig(self,attackDir);
        }
        #endregion

        #region CancelAttack
        private Func<bool> onCancelAttack;
        /// <summary>
        /// Register a function to be called when the nail swing animation ends
        /// </summary>
        /// <param name="func"> The function to be called. It must return <c> true</c> if it wants the default behaviour to continue, <c>false</c> if it doesn't</param>        
        public void RegisterCancelAttack(Func<bool> func) 
        { 
            onCancelAttack = func;
            OnSelect += () => { On.HeroController.CancelAttack += OnCancelAttack; };
            OnUnselect += () => { On.HeroController.CancelAttack -= OnCancelAttack; };
        }

        private void OnCancelAttack(On.HeroController.orig_CancelAttack orig, HeroController self)
        {
            if (onCancelAttack()) orig(self);
        }
        #endregion
    }
}
