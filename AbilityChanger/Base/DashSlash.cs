using commonStates = AbilityChanger.Base.CommonStates.NailArts;
namespace AbilityChanger.Base
{
    public abstract class DashSlash: Ability
    {
        public override string abilityType => Abilities.DASHSLASH;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.NAILARTS];

        /// <summary>
        /// Replaces the GameObject spawned by Dash Slash
        /// </summary>
        /// <param name="prefab"> The GameObject to spawn </param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at the Knight's current position </param>
        public void ReplaceSpawn(GameObject prefab, Vector3? position = null)
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
                    fromState = states.HasDash,
                    toStateDefault = states.DashSlashReady,
                    eventName ="FINISHED",
                    toStateCustom = shouldContinue ? states.DSlashStart : commonStates.RegainControl,
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
