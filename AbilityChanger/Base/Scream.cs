using commonStates = AbilityChanger.Base.CommonStates.SpellControl;
namespace AbilityChanger.Base
{
    public abstract class Scream : Ability
    {
        public override string abilityType => Abilities.SCREAM;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.SPELLCONTROL];

        public void RegisterSpawn(GameObject prefab, Vector3? position = null)
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = "Scream Burst 1",
                    toStateDefault = "End Roar",
                    toStateCustom = "Spell End",
                    eventName = "FINISHED",
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position + new Vector3(0, -1.42f, -0.0035f) : position),
                                            Quaternion.identity).SetActive(true)
                });
            };

            myFsm.Intercept(new TransitionInterceptor()
            {
                fromState = "Scream Burst 2",
                toStateDefault = "End Roar 2",
                toStateCustom = "Spell End",
                eventName = "FINISHED",
                shouldIntercept = () => true,
                onIntercept = (a, b) => GameObject.Instantiate(prefab,
                                        (Vector3)(position is null ? HeroController.instance.transform.position + new Vector3(0, -1.42f, -0.0035f) : position),
                                        Quaternion.identity).SetActive(true)
            });
        }
        private Action trigger;
        /// <summary>
        /// Register an anction to called when the ability would start
        /// </summary>
        /// <param name="triggerFunc"> the action to call</param>
        /// <param name="shouldContinue"> if the default behaviour should continue </param>
        public void RegisterTrigger(Action triggerFunc, bool shouldContinue)
        {
            OnSelect += () =>
            {

                trigger = triggerFunc;
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.HasScream,
                    toStateDefault = states.ScreamGet,
                    eventName = "CAST",
                    toStateCustom = shouldContinue ? states.ScreamGet : commonStates.SpellEnd,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => trigger(),
                });
            };
        }
        public static class states
        {
            public static string HasScream { get; } = "Has Scream?";
            public static string ScreamGet { get; } = "Scream Get?";
            public static string SGAntic { get; } = "SG Antic";
            public static string ScreamBurst3 { get; } = "Scream Burst 3";
            public static string SendEvent { get; } = "Send Event";
            public static string LevelCheck3 { get; } = "Level Check 3";
            public static string ScreamAntic1 { get; } = "Scream Antic1";
            public static string ScreamBurst1 { get; } = "Scream Burst 1";
            public static string EndRoar { get; } = "End Roar";
            public static string ScreamEnd { get; } = "Scream End";
            public static string ScreamAntic2 { get; } = "Screm Antic2";
            public static string ScreamBurst2 { get; } = "Scream Burst 2";
            public static string EndRoar2 { get; } = "End Roar 2";
            public static string ScreamEnd2 { get; } = "Scream End 2";

        }
    }
}

