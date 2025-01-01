using commonStates = AbilityChanger.Base.CommonStates.NailArts;
namespace AbilityChanger.Base
{
    public abstract class DashSlash: Ability
    {
        public override string abilityType => Abilities.DASHSLASH;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.NAILARTS];

        public void RegisterSpawn(GameObject prefab, Vector3? position = null)
        {
            OnSelect += () =>
            {
                myFsm.GetValidState("Dash Slash").GetAction(0).Enabled= false;
                myFsm.InsertCustomAction("Dash Slash", () =>
                {
                    GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position : position),
                                            Quaternion.identity).SetActive(true);
                }, 0);
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
                    fromState = states.HasDash,
                    toStateDefault = states.DashSlashReady,
                    eventName ="FINISHED",
                    toStateCustom = shouldContinue ? states.DSlashStart : commonStates.RegainControl,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => trigger()
                });
            };
        }

        public static class states
        {
            public static string HasDash { get; } = "Has Dash?";
            public static string DashSlashReady { get; } = "Dash Slash Ready";
            public static string DSlashStart { get; } = "DSlash Start";
            public static string Facing2 { get; } = "Facing? 2";
            public static string Right2 { get; } = "Right 2";
            public static string Left2 { get; } = "Left 2";
            public static string DashSlash { get; } = "Dash Slash";
            public static string DSlashMoveEnd { get; } = "DSlash Move End";
            public static string DSlashEnd { get; } = "D Slash End";

        }

    }
}
