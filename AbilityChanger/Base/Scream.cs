using commonStates = AbilityChanger.Base.CommonStates.SpellControl;
namespace AbilityChanger.Base
{
    public abstract class Scream : Ability
    {
        public override string abilityType => Abilities.SCREAM;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.SPELLCONTROL];

        /// <summary>
        /// Replaces the GameObject spawned by Howlling Wraiths
        /// </summary>
        /// <param name="prefab"> The GameObject to spawn </param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at the Knight's current position </param>
        public void ReplaceSpawnScream1(GameObject prefab, Vector3? position = null)
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

        }

        /// <summary>
        /// Replaces the GameObject spawned by Abyss Shriek
        /// </summary>
        /// <param name="prefab"> The GameObject to spawn </param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at the Knight's current position </param>
        public void ReplaceSpawnScream2(GameObject prefab, Vector3? position = null)
        {
            OnSelect += () =>
            {

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
            };
        }

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
                    fromState = states.HasScream,
                    toStateDefault = states.ScreamGet,
                    eventName = "CAST",
                    toStateCustom = shouldContinue ? states.ScreamGet : commonStates.SpellEnd,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => triggerAction(),
                });
            };
        }

        /// <summary>
        /// The FSM states Ability Changer considers belong to this Ability and expects to be modified without repercutions. 
        /// Shared states between abilities can be accessed withing the Base.CommonStates namespace, changes in those states can affect other abilities and should be done with care  
        /// </summary>
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

