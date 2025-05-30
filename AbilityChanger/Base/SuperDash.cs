namespace AbilityChanger.Base
{
    public abstract class SuperDash: Ability 
    {
        public override string abilityType => Abilities.SUPERDASH;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.SUPERDASH];

        /// <summary>
        /// Register an action to be called when the ability would start
        /// </summary>
        /// <param name="triggerAction"> the action to call</param>
        /// <param name="shouldContinue"> if the default behaviour should continue </param>
        public void OnTrigger(Action triggerAction, bool shouldContinue)
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.RelinquishControl,
                    toStateDefault = states.OnGround,
                    eventName = "FINISHED",
                    toStateCustom = shouldContinue ? states.OnGround : states.RegainControl,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => triggerAction(),
                });
            };
        }
        /// <summary>
        /// Register an action to be called every fixed frame during cdash's charging animation
        /// </summary>
        /// <param name="chargingAction"></param>
        public void DuringCharging(Action chargingAction)
        {
            OnSelect += () =>
            {
                myFsm.InsertAction(states.WallCharge, new CustomFsmActionFixedUpdate(chargingAction), 1);
                myFsm.InsertAction(states.GroundCharge, new CustomFsmActionFixedUpdate(chargingAction), 2);
            };
        }

        /// <summary>
        /// Register an action to be called every fixed frame during cdash's dash animation
        /// </summary>
        /// <param name="dashingAction"></param>
        public void DuringSuperDash(Action dashingAction)
        {
            OnSelect += () =>
            {
                myFsm.AddAction(states.Dashing, new CustomFsmActionFixedUpdate(dashingAction));
                myFsm.AddAction(states.Cancelable, new CustomFsmActionFixedUpdate(dashingAction));
            };
        }

        /// <summary>
        /// The FSM states Ability Changer considers belong to this Ability and expects to be modified without repercutions. 
        /// Shared states between abilities can be accessed withing the Base.CommonStates namespace, changes in those states can affect other abilities and should be done with care  
        /// </summary>
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
