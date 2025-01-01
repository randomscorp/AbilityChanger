using commonStates = AbilityChanger.Base.CommonStates.SpellControl;

namespace AbilityChanger.Base
{
    public abstract class Fireball: Ability
    {
        public override string abilityType => Abilities.FIREBALL;
        public PlayMakerFSM myFsm => AbilityChanger.FsmMap[AbilitiesFSMs.SPELLCONTROL];

        public void RegisterSpawn(GameObject prefab, Vector3? position = null)
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

                myFsm.GetValidState("Fireball 2").GetAction(3).Enabled = false;
                myFsm.InsertCustomAction("Fireball 2", () =>
                {
                    GameObject.Instantiate(prefab,
                                            (Vector3)(position is null ? HeroController.instance.transform.position : position),
                                            Quaternion.identity).SetActive(true);
                }, 3);
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
                    fromState = states.HasFireball,
                    toStateDefault = states.Wallside,
                    eventName ="CAST",
                    toStateCustom = shouldContinue ? states.Wallside : commonStates.SpellEnd,
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => trigger()
                });
            };
        }
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
