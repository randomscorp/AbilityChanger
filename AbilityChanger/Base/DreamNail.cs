using commonStates = AbilityChanger.Base.CommonStates.DreamNail;


namespace AbilityChanger.Base
{
    public abstract class DreamNail: Ability
    {
        public override string abilityType => Abilities.DREAMNAIL;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.DREAMNAIL];

        /// <summary>
        /// Replaces the Dream Nail Game Object to be spawned
        /// </summary>
        /// <param name="prefab"> The game object to spawn </param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at Dream Gate's default position </param>
        public void ReplaceSpawn(GameObject prefab, Vector3? position = null)
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

        private Func<GameObject, bool> onImpact;
        /// <summary>
        /// Register a function to be called when an enemy receives Dream Nail's impact
        /// </summary>
        /// <param name="func"> The function to be called. It must receive a GameObject as parameter.  It must return <c>true</c> if it wants the default behaviour to continue or <c>false</c> if it doesn't</param>
        public void OnInpact(Func<GameObject,bool> func)
        {
            onImpact = func;
            OnSelect += () => { On.EnemyDreamnailReaction.RecieveDreamImpact += EnemyDreamnailReaction_RecieveDreamImpact; };
            OnUnselect += () => { On.EnemyDreamnailReaction.RecieveDreamImpact -= EnemyDreamnailReaction_RecieveDreamImpact; };
        }

        private void EnemyDreamnailReaction_RecieveDreamImpact(On.EnemyDreamnailReaction.orig_RecieveDreamImpact orig, EnemyDreamnailReaction self)
        {
            if (onImpact(self.gameObject)) orig(self) ;
        }

        /// <summary>
        /// The FSM states Ability Changer considers belong to this Ability and expects to be modified without repercutions. 
        /// Shared states between abilities can be accessed withing the Base.CommonStates namespace, changes in those states can affect other abilities and should be done with care  
        /// </summary>
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
