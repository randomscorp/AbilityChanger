using commonStates = AbilityChanger.Base.CommonStates.NailArts;

namespace AbilityChanger.Base
{
    public abstract class GreatSlash: Ability
    {
        public override string abilityType => Abilities.GREATSLASH;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.NAILARTS];

        /// <summary>
        /// Replaces the GameObject spawned by Great Slash
        /// </summary>
        /// <param name="prefab"> The GameObject to spawn </param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at the Knight's current position </param>
        public void ReplaceSpawn(GameObject prefab, Vector3? position = null)
        {
            OnSelect += () =>
            {
                myFsm.GetValidState("G Slash").GetAction(2).Enabled = false;
                myFsm.InsertCustomAction("G Slash", () =>
                {
                    GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position : position),
                                            Quaternion.identity).SetActive(true);
                }, 2);
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
                    fromState = states.HasGSlash,
                    toStateDefault = states.Flash2,
                    eventName ="FINISHED",
                    toStateCustom = shouldContinue ? states.Flash2 : commonStates.RegainControl,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => triggerAction()
                });
            };
        }

        /// <summary>
        /// The FSM states Ability Changer considers belong to this Ability and expects to be modified without repercutions. 
        /// Shared states between abilities can be accessed withing the Base.CommonStates namespace, changes in those states can affect other abilities and should be done with care  
        /// </summary>
        public static class states
        {
            public static string HasGSlash { get; } = "Has G Slash?";
            public static string Flash2 { get; } = "Flash 2";
            public static string Facing { get; } = "Facing?";
            public static string Left { get; } = "Left";
            public static string Right { get; } = "Right";
            public static string GSlash { get; } = "G Slash";
            public static string StopMOve { get; } = "Stop Move";
            public static string GSlashEnd { get; } = "G Slash End";

        }
    }
}
