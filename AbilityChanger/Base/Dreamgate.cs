using commonStates = AbilityChanger.Base.CommonStates.DreamNail;

namespace AbilityChanger.Base
{
    public abstract class Dreamgate: Ability
    {

        public override string abilityType => Abilities.DREAMGATE;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.DREAMNAIL];

        #region Replace Spawn Hook
        /// <summary>
        /// Replaces the Dream Gate Game Object to be spawned
        /// </summary>
        /// <param name="prefab"> The game object to spawn </param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at Dream Gate's default position </param>
        public void ReplaceSpawn(GameObject prefab, Vector3? position=null)
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = "Set",
                    toStateDefault = "Spawn Gate",
                    toStateCustom = prefab == null ? "Set Fail" : "Set Recover",
                    eventName = "FINISHED",
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position + new Vector3(0,-1.42f, -0.0035f) : position),
                                            Quaternion.identity).SetActive(true)

                });
                myFsm.GetValidState("Set").GetAction(4).Enabled = false;
            };
        }
        #endregion

        /// <summary>
        /// The FSM states Ability Changer considers belong to this Ability and expects to be modified without repercutions. 
        /// Shared states between abilities can be accessed withing the Base.CommonStates namespace, changes in those states can affect other abilities and should be done with care  
        /// </summary>
        public static class states
        {
            public static string SetChargeStart { get; } = "Set Charge Start";
            public static string SetCharge { get; } = "Set Charge";
            public static string SetAntic { get; } = "Set Antic";
            public static string CanSet { get; } = "Can Set?";
            public static string SetFail { get; } = "Set Fail";
            public static string Set { get; } = "Set";
            public static string SpawnGate { get; } = "Spawn Gate";
            public static string SetRecover { get; } = "Set Recover";
            public static string WarpChargeStart { get; } = "Warp Charge Start";
            public static string WarpCharge { get; } = "Warp Charge";
            public static string CanWarp { get; } = "Can Warp?";
            public static string WarpCancel { get; } = "Warp Cancel";
            public static string WarpFail { get; } = "Warp Fail";
            public static string ShowEssence { get; } = "Show Essence";
            public static string CheckScene { get; } = "Check Scene";
            public static string WarpEffect { get; } = "Warp Effect";
            public static string Flower { get; } = "Flower?";
            public static string LeaveType { get; } = "Warp End";
            public static string Boss { get; } = "Boss?";
            public static string NewScene { get; } = "New Scene";
            public static string LeaveDream { get; } = "Leave Dream";
            public static string SetGGWaterways { get; } = "Set GG Waterways";
            public static string AllowDreamGate { get; } = "Allow Dream Gate";
        }
}
}
