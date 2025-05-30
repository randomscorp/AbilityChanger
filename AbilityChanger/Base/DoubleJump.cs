using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityChanger.Base
{
    public abstract class DoubleJump: Ability
    {
        public override string abilityType => Abilities.DOUBLEJUMP;

        #region Trigger
        private Func<bool> onDoDoubleJump;
        /// <summary>
        /// Register a function to be called when the wings input is pressed.
        /// </summary>
        /// <param name="func"> The function to be called. It must return <c>true</c> if it wants the default behaviour to continue or <c>false</c> if it doesn't</param>
        public void OnTrigger(Func<bool> func)
        {
            onDoDoubleJump = func;
            OnSelect += () => { On.HeroController.DoDoubleJump += HeroController_DoDoubleJump; };
            OnUnselect += () => { On.HeroController.DoDoubleJump -= HeroController_DoDoubleJump; };
        }
        private void HeroController_DoDoubleJump(On.HeroController.orig_DoDoubleJump orig, HeroController self)
        {
            if (onDoDoubleJump()) orig(self); 
        }
        #endregion

        #region DoubleJump
        private Func<bool> onDoubleJump;
        /// <summary>
        /// Register a function to be called every fixed frame while the wings animation plays.
        /// </summary>
        /// <param name="func"> The function to be called. It must return <c>true</c> if it wants the default behaviour to continue or <c>false</c> if it doesn't</param>
        public void DuringDoubleJump(Func<bool> func)
        {
            onDoubleJump = func;
            OnSelect += () => { On.HeroController.DoubleJump += OnDoubleJump; ; };
            OnUnselect += () => { On.HeroController.DoubleJump -= OnDoubleJump; ; };
        }
        private void OnDoubleJump(On.HeroController.orig_DoubleJump orig, HeroController self) { if (onDoubleJump()) orig(self); }
        #endregion
    }
}
