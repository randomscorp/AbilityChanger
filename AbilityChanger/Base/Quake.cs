using commonStates = AbilityChanger.Base.CommonStates.SpellControl;

namespace AbilityChanger.Base
{
    public abstract class Quake: Ability
    {
        public override string abilityType => Abilities.QUAKE;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.SPELLCONTROL];

        /// <summary>
        /// Register an action to be called when the ability would start
        /// </summary>
        /// <param name="triggerAction"> the action to call</param>
        /// <param name="shouldContinue"> if the default behaviour should continue after </param>
        public void OnTrigger(Action triggerAction, bool shouldContinue)
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.HasQuake,
                    toStateDefault = states.OnGround,
                    eventName ="CAST",
                    toStateCustom = shouldContinue ? states.OnGround : commonStates.SpellEnd,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => triggerAction()
                });
            };
        }
        /// <summary>
        /// Register an action to be called when the player lands after a Desolate Dive
        /// </summary>
        /// <param name="landAction">The action to be called </param>
        /// <param name="shouldContinue"> If the default behavior should continue after </param>
        public void OnLandQuake1(Action landAction, bool shouldContinue)
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.Quake1Down,
                    toStateDefault=states.Quake1Land,
                    eventName="HERO LANDED",
                    toStateCustom = shouldContinue ? states.Quake1Land : states.QuakeFinish,
                    shouldIntercept = () => true,
                    onIntercept = (a,b) => landAction()
                });
            };
        }

        /// <summary>
        /// Register an action to be called when the player lands after a Descending Dark
        /// </summary>
        /// <param name="landAction">The action to be called </param>
        /// <param name="shouldContinue"> If the default behavior should continue after </param>
        public void OnLandQuake2(Action landAction, bool shouldContinue)
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.Quake2Down,
                    toStateDefault = states.Q2Land,
                    eventName = "HERO LANDED",
                    toStateCustom = shouldContinue ? states.Q2Land : states.QuakeFinish,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => landAction()
                });
            };
        }

        /// <summary>
        /// The FSM states Ability Changer considers belong to this Ability and expects to be modified without repercutions. 
        /// Shared states between abilities can be accessed withing the Base.CommonStates namespace, changes in those states can affect other abilities and should be done with care  
        /// </summary>
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
