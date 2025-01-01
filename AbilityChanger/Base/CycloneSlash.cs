using commonStates = AbilityChanger.Base.CommonStates.NailArts;

namespace AbilityChanger.Base
{
    public abstract class CycloneSlash: Ability
    {
        public override string abilityType => Abilities.CYCLONESLASH;

        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.NAILARTS];

        public void RegisterSpawn(GameObject prefab, Vector3? position=null)
        {
            OnSelect += () =>
            {
                myFsm.Intercept( new TransitionInterceptor()
                {
                    fromState = "Flash",
                    toStateDefault = "Cyclone Start",
                    toStateCustom = "Regain Control",
                    eventName = "FINISHED",
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position: position),
                                            Quaternion.identity).SetActive(true)
                });
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
                    fromState = states.HasCyclone,
                    toStateDefault = states.Flash,
                    eventName ="FINISHED",
                    toStateCustom = shouldContinue ? states.Flash : commonStates.RegainControl,
                    shouldIntercept =()=> true,
                    onIntercept = (a,b) => trigger()
                });
            };
        }

        public static class states
        {
            public static string HasCyclone { get; } = "Has Cyclone?";
            public static string Flash { get; } = "Flash";
            public static string CycloneStart { get; } = "Cyclone Start";
            public static string HoverStart { get; } = "Hover Start";
            public static string ActivateSlash { get; } = "Activate Slash";
            public static string PlayAudio { get; } = "Play Audio";
            public static string CycloneSpin { get; } = "Cyclone Spin";
            public static string CycloneExtend { get; } = "Cyclone Extend";
            public static string CycloneEnd { get; } = "Cyclone End";
            public static string CycSendMsg { get; } = "Cyc Send Msg";
        }
    }
}
