namespace AbilityChanger.Base
{
    public abstract class SuperDash: Ability 
    {
        public override string abilityType => Abilities.SUPERDASH;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.SUPERDASH];

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
                    fromState = states.RelinquishControl,
                    toStateDefault = states.OnGround,
                    eventName = "FINISHED",
                    toStateCustom = shouldContinue ? states.OnGround : states.RegainControl,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => trigger(),
                });
            };
        }
        public static class states
        {
            public static string Init { get; } = "Init";
            public static string Cancel { get; } = "Cancel";
            public static string Inactive { get; } = "Inactive";
            public static string CanSuperdash { get; } = "Can Superdash?";
            public static string RelinquishControl { get; } = "Relinquish Control";
            public static string OnGround { get; } = "On Ground?";
            public static string WallCharge { get; } = "Wall Charge";
            public static string ChargeCancelGround { get; } = "Charge Cancel Ground";
            public static string RegainControl { get; } = "Regain Control";
            public static string  GroundCharge { get; } = "Ground Charge";
            public static string  GroundCharged { get; } = "Ground Charged";
            public static string WallCharged { get; } = "Wall Charged";
            public static string ChargeCancelWall { get; } = "Charge Cancel Wall";
            public static string Direction { get; } = "Direction";
            public static string DirectionWall { get; } = "Direction Wall";
            public static string GRight { get; } = "G Right";
            public static string GLeft { get; } = "G Left";
            public static string Left { get; } = "Left";
            public static string Right { get; } = "Right";
            public static string DashStart { get; } = "Dash Start";
            public static string Dashing { get; } = "Dashing";
            public static string AirCancel { get; } = "Air Cancel";
            public static string HitWall { get; } = "Hit Wall";
            public static string Cancelable { get; } = "Cancelable";
            public static string EnterSuperDash { get; } = "Enter Super Dash";
            public static string EnterL { get; } = "Enter L";
            public static string EnterR { get; } = "Enter R";
            public static string EnterVelocity { get; } = "Enter Velocity";
        }
}
}
