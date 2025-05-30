using commonStates = AbilityChanger.Base.CommonStates.NailArts;

namespace AbilityChanger.Base
{
    public abstract class CycloneSlash: Ability
    {
        public override string abilityType => Abilities.CYCLONESLASH;

        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.NAILARTS];
        /// <summary>
        /// Replaces the GameObject spawned when cyclone starts
        /// </summary>
        /// <param name="prefab"> the GamewObject to be spawned</param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at the Knight's current position </param>
        public void ReplaceSpawn(GameObject prefab, Vector3? position=null)
        {
            OnSelect += () =>
            {
                myFsm.Intercept( new TransitionInterceptor()
                {
                    fromState = states.Flash,
                    toStateDefault = states.CycloneStart,
                    toStateCustom = commonStates.RegainControl,
                    eventName = "FINISHED",
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position: position),
                                            Quaternion.identity).SetActive(true)
                });
            };
        }

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
                    fromState = states.HasCyclone,
                    toStateDefault = states.Flash,
                    eventName ="FINISHED",
                    toStateCustom = shouldContinue ? states.Flash : commonStates.RegainControl,
                    shouldIntercept =()=> true,
                    onIntercept = (a,b) => triggerAction()
                });
            };
        }

        /// <summary>
        /// Register an action to be called every fixed frame during cyclone's slashing animation and extended animation
        /// </summary>
        /// <param name="action"></param>
        public void DuringSlash(Action action)
        {
            OnSelect += () =>
            {
                myFsm.InsertAction(states.CycloneSpin, new CustomFsmActionFixedUpdate(action), 1);
                myFsm.InsertAction(states.CycloneExtend, new CustomFsmActionFixedUpdate(action), 1);
            };
        }
        
        // TODO: This can prob be replaced by abstract proprierty in a interface (?) which would skip having to copy the docstring in every file
        /// <summary>
        /// The FSM states Ability Changer considers belong to this Ability and expects to be modified without repercutions. 
        /// Shared states between abilities can be accessed withing the Base.CommonStates namespace, changes in those states can affect other abilities and should be done with care  
        /// </summary>
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
