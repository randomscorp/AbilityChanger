using commonStates = AbilityChanger.Base.CommonStates.DreamNail;


namespace AbilityChanger.Base
{
    public abstract class DreamNail: Ability
    {
        public override string abilityType => Abilities.DREAMNAIL;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.DREAMNAIL];
        public void RegisterSpawn(GameObject prefab, Vector3? position = null)
        {
            OnSelect += () =>
            {
                myFsm.GetValidState("Slash").GetAction(2).Enabled = false;
                myFsm.InsertCustomAction("Slash", () =>
                {
                    GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position : position),
                                            Quaternion.identity).SetActive(true);
                }, 2);
            };
        }
        public static class states
        {
            public static string SlashAntic { get; } = "Slash Antic";
            public static string Slash { get; } = "Slash";
            public static string Cancelable { get; } = "Cancelable";
            public static string CancelableDash { get; } = "Cancelable Dash";
            public static string SetDashCancel { get; } = "Set Dash Cancel";
            public static string SetAttackCancel { get; } = "Set Attack Cancel";
            public static string SetJumpCancel { get; } = "Set Jump Cancel";
            public static string End { get; } = "End";
        }
    }
}
