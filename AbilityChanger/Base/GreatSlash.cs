using commonStates = AbilityChanger.Base.CommonStates.NailArts;

namespace AbilityChanger.Base
{
    public abstract class GreatSlash: Ability
    {
        public override string abilityType => Abilities.GREATSLASH;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.NAILARTS];
        public void RegisterSpawn(GameObject prefab, Vector3? position = null)
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
                    fromState = states.HasGSlash,
                    toStateDefault = states.Flash2,
                    eventName ="FINISHED",
                    toStateCustom = shouldContinue ? states.Flash2 : commonStates.RegainControl,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => trigger()
                });
            };
        }
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
