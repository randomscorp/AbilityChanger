using commonStates = AbilityChanger.Base.CommonStates.SpellControl;

namespace AbilityChanger.Base
{
    public abstract class Focus: Ability
    {
        public override string abilityType => Abilities.FOCUS;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.SPELLCONTROL];


        private Action trigger;
        /// <summary>
        /// Register an anction to called when the ability would start
        /// </summary>
        /// <param name="triggerFunc"> the action to call</param>
        /// <param name="shouldContinue"> if the default behaviour should continue </param>
        public void RegisterTrigger(Action triggerFunc, bool shouldContinue)
        {
            trigger = triggerFunc;
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.CanFocus,
                    toStateDefault = states.StartSlugAnim,
                    eventName = "FINISHED",
                    toStateCustom = shouldContinue ? states.StartSlugAnim : states.Cancel,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => trigger()
                });
            };
        }
    }
    public static class states
    {
        public static string HeldDown { get; } = "Held Down";
        public static string CanFocus { get; } = "Can Focus?";
        public static string StartSlugAnim { get; } = "Start Slug Anim";
        public static string FocusStart { get; } = "Focus Start";
        public static string SetFocusSpeed { get; } = "Set Focus Speed";
        public static string DeepFocusSpeed { get; } = "Deep Focus Speed";
        public static string Slug { get; } = "Slug?";
        public static string Focus { get; } = "Focus";
        public static string ResetCamZoom { get; } = "Reset Cam Zoom";
        public static string FSMCancel { get; } = "FSM Cancel";
        public static string ResetDamageMode { get; } = "Reset Damage Mode";
        public static string CancelAll { get; } = "Cancel All";
        public static string BackIn { get; } = "Back In?";
        public static string Cancel { get; } = "Cancel";
        public static string RegainControl { get; } = "Regain Control";
        public static string HoldDown { get; } = "Hold Down";
        public static string FocusStartD { get; } = "Focus Start D";
        public static string FocusD { get; } = "Focus D";
        public static string FocusCancel { get; } = "Focus Cancel";
        public static string FocusCancelD { get; } = "Focus Cancel D";
        public static string KeepFocus { get; } = "Keep Focus";
        public static string GraceCheck { get; } = "Grace Check";
        public static string FocusGetFinish { get; } = "Focus Get Finish";
        public static string FirstGraceCheck { get; } = "First Grace Check";
        public static string SporeCloud { get; } = "Spore Cloud";
        public static string SetHPAmount { get; } = "Set HP Amount";
        public static string DungCloud { get; } = "Dung Cloud";
        public static string SetFull { get; } = "Set Full";
        public static string FullHP { get; } = "Full HP?";
        public static string SlugSpeed { get; } = "Slug Speed";
        public static string AnimCheck { get; } = "Anim Check";
        public static string Normal { get; } = "Normal";
        public static string Blocker { get; } = "Blocker";
        public static string Shroom { get; } = "Shroom";
        public static string Combo { get; } = "Combo";
        public static string Speedup { get; } = "speedup?";
        public static string StartMPDrain { get; } = "Start MP Drain";
        public static string FocusCancel2 { get; } = "Focus Cancel 2";
        public static string FocusGetFinish2 { get; } = "Focus Get Finish 2";
        public static string SetFull2 { get; } = "Set Full 2";
        public static string GraceCheck2 { get; } = "Grace Check 2";
        public static string FirstGraceCheck2 { get; } = "First Grace Check 2";
        public static string FullHP2 { get; } = "Full HP? 2";
        public static string FocusS { get; } = "Focus S";
        public static string SetHPAmount2 { get; } = "Set HP Amount 2";
        public static string FocusHeal2 { get; } = "Focus Heal 2";
        public static string TurnAnimL { get; } = "Turn Anim L?";
        public static string TurnAnimR { get; } = "Turn Anim R?";
        public static string SporeCloud2 { get; } = "Spore Cloud 2";
        public static string DungCloud2 { get; } = "Dung Cloud 2";
        public static string FocusLeft { get; } = "Focus Left";
        public static string FocusRight { get; } = "Focus Right";

    }
}
