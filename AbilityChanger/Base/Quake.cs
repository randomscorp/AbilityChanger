using commonStates = AbilityChanger.Base.CommonStates.SpellControl;

namespace AbilityChanger.Base
{
    public abstract class Quake: Ability
    {
        public override string abilityType => Abilities.QUAKE;
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
                    fromState = states.HasQuake,
                    toStateDefault = states.OnGround,
                    eventName ="CAST",
                    toStateCustom = shouldContinue ? states.OnGround : commonStates.SpellEnd,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => trigger()
                });
            };
        }
        public static class states
        {
            public static string HasQuake { get; } = "Has Quake?";
            public static string OnGround { get; } = "On Ground?";
            public static string QOnGround { get; } = "Q On Ground";
            public static string QOffGround { get; } = "Q Off Ground";
            public static string QuakeAntic { get; } = "Quake Antic";
            public static string LevelCheck2 { get; } = "Level Check 2";
            public static string Q1Effect { get; } = "Q1 Effect";
            public static string Quake1Down { get; } = "Quake1 Down";
            public static string Quake1Land { get; } = "Quake1 Land";
            public static string QuakeFinish { get; } = "Quake Finish";
            public static string Quake2Effect { get; } = "Q2 Effect";
            public static string Quake2Down { get; } = "Quake2 Down";
            public static string Q2Land { get; } = "Q2 Land";
            public static string Q2Pillar { get; } = "Q2 Pillar";

        }
    }   
}
