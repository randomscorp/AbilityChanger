using commonStates = AbilityChanger.Base.CommonStates.SpellControl;

namespace AbilityChanger.Base
{
    public abstract class Fireball: Ability
    {
        public override string abilityType => Abilities.FIREBALL;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.SPELLCONTROL];
        /// <summary>
        /// Replaces the GameObject spawned by Vengefull Spirit
        /// </summary>
        /// <param name="prefab"> The GameObject to spawn </param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at the Knight's current position </param>
        public void ReplaceSpawnFireball1(GameObject prefab, Vector3? position = null)
        {
            OnSelect += () =>
            {
                myFsm.GetValidState("Fireball 1").GetAction(3).Enabled = false;
                myFsm.InsertCustomAction("Fireball 1", () =>
                {
                    GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position : position),
                                            Quaternion.identity).SetActive(true);
                }, 3);
            };
        }

        /// <summary>
        /// Replaces the GameObject spawned by Shade Soul
        /// </summary>
        /// <param name="prefab"> The GameObject to spawn </param>
        /// <param name="position"> The position it should spawn. If not provided, spaws at the Knight's current position </param>
        public void ReplaceSpawnFireball2(GameObject prefab, Vector3? position = null)
        {
            OnSelect += () =>
            {
                myFsm.GetValidState("Fireball 2").GetAction(3).Enabled = false;
                myFsm.InsertCustomAction("Fireball 2", () =>
                {
                    GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position : position),
                                            Quaternion.identity).SetActive(true);
                }, 3);
            };
        }


        /// <summary>
        /// Register an anction to be called when the ability button is pressed
        /// </summary>
        /// <param name="triggerAction"> the action to call</param>
        /// <param name="shouldContinue"> if the default behaviour should continue after</param>
        public void OnTrigger(Action triggerAction, bool shouldContinue)
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.HasFireball,
                    toStateDefault = states.Wallside,
                    eventName ="CAST",
                    toStateCustom = shouldContinue ? states.Wallside : commonStates.SpellEnd,
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
            public static string HasFireball { get; } = "Has Fireball?";
            public static string Wallside { get; } = "Wallside?";
            public static string FireballAntic { get; } = "Fireball Antic";
            public static string LevelCheck { get; } = "Level Check";
            public static string Fireball1 { get; } = "Fireball 1";
            public static string Fireball2 { get; } = "Fireball 2";
            public static string FireballRecoil { get; } = "Fireball Recoil";
        }

    }
}
