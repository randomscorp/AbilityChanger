# AbilityChanger

A library mod that creates an environment to modify the knight's abilities. It manages inventory, saves and switching between abilities and also has functions to simplify part of the process.
This mod overrides the PlayerData related to the abilities using [PlayerDataPatcher](https://github.com/PrashantMohta/AbilityChanger/blob/main/AbilityChanger/Ability/PlayerDataPatcher.cs).

Ability Changer offer's [base classes](https://github.com/randomscorp/AbilityChanger/tree/main/AbilityChanger/Base) to build an ability from.
To create an ability you should inherit one of those classes and register an instance of it with the [RegisterAbility](https://github.com/randomscorp/AbilityChanger/blob/main/AbilityChanger/AbilityChanger.cs#L50)
function.

A created ability, once aquired, will be available to be equipped by going into to the ability it is replacing in the inventory and pressing the confirm buttom (same button
used for interacating with the pause buttton). If the ability is the only one available for that inventory slot, it'll be autoenabled. AC also manages local saves and
will save your aquired abilities and autoequip the previously equiped ones after load.

Ability Changer is built with [Satchel](https://github.com/PrashantMohta/Satchel)
# How it works
AC works by calling actions registered in the ´OnSelect´ and ´OnUnselect´ delegates every time a switch occurs. This has different meanings for FSM abilities and Non-fsm abilities
so we'll go through them separetly:

## FSM abilities

For FSM abilities, AC maitains a copy of the vannila FSM that will replace the original. When ´OnSelect´ is called, AC disables the vannila FSM and enables the custom one
(unless all equiped abilities that uses that FSM are not custom then the vannila one will be exposed) meaning that AC's environment is completly isolated from other mods. 
You can register a delegate to ´OnSelect´ and modify the FSM as you normally would for a mod, the modfied FSM can be accessed by the name ´{vannilaFSMName} AC´ or directly from
the ´myFSM´ porprierty on the base class.

Because some abilities will share the FSM, when ´OnSelect´ is called AC resets the whole FSM and not only calls the actions registered by the switched ability, but all the 
other ones registered by all abilities that use the FSM. That means there is no need to undo your changes on ´OnUnselect´ and actions registered there should be reserved to
undo permanent effects in the world, like despawning GameObjects that interact with your ability. It also means that you have access to the whole FSM, to guide you on what states
AC considers your's to modify you can check the ´states´ proprierty within the base class. Every state in ´states´ is considered the domain of your ability and you should
feel free to modify them as you please. There are also states AC considers to be shared between abilities and they are avaiabled in the ´commonStates´ proprierty. When making
changes to those states be as careful as you would with a normal mod as other abilities are expected to also have access to it.

The abilities that are based on FSM and the ones that share one are:
* Focus, Fireball, Quake and Scream => Spell Controller
* Great Slash, Dash Slash and Cyclone Slash => Nail Arts
* Dream Nail and Dream Gate => Dream Nail
* Crystal Heart => Super Dash

For example, if you wanted the replace what object that Dream Gate spawns you can
´´´

    using AbilityChanger;
    using Satchel;
    using static AbilityChanger.AbilityChanger;
    using AbilityChanger.Base;
    using Satchel.Futils;

    public class WhiteFlowerDG : Dreamgate
    {
        public WhiteFlowerDG()
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.Set,
                    toStateDefault = states.SpawnGate,
                    toStateCustom = AbilityChangerExample.flower == null ? "Set Fail" : "Set Recover",
                    eventName = "FINISHED",
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => GameObject.Instantiate(AbilityChangerExample.flower,HeroController.instance.transform.position + new Vector3(0, -1.42f, -0.0035f),
                                            Quaternion.identity).SetActive(true)

                });
                myFsm.GetValidState("Set").GetAction(4).Enabled = false;
            };
        }

    }
´´´

All examples are available in their complete form [here](https://github.com/randomscorp/AbilityChanger/blob/main/Example/AbilityChangerExample.cs)

## Script based abilities

Non-fsm abilities work as functions in a monobehavior (HeroController) and called every Update/Fixed and you can modify them as you normally would with hooks:

´´´

    public class WhiteFlowerWings : DoubleJump
    {
        public WhiteFlowerWings()
        {
            OnSelect += () => { On.HeroController.DoubleJump += SpawnWingsFlower; };
            OnUnselect += () => { On.HeroController.DoubleJump -= SpawnWingsFlower; };
        }

        private void SpawnWingsFlower(On.HeroController.orig_DoubleJump orig, HeroController self)
        {
            GameObject.Instantiate(AbilityChangerExample.flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true);
            orig(self);
        }
    }
´´´
Since the hooks happen on game time (instead of On.HeroController.Start where most mods add them), your ability will likely be called before other hooks.
Note that for those abilities, you **MUST** undo your changes on ´OnUnselect´ otherwise they will persist between switches. To spare you the process of having to do that
and look for the apropriate functions, Ability Changer offers hooks to the relevant ones:
´´´

    public class RedFlowerWings : DoubleJump
    {
        public RedFlowerWings()
        {
            RegisterDoubleJump(() =>
            {
                GameObject.Instantiate(AbilityChangerExample.flower2, HeroController.instance.transform.position,
                                     Quaternion.identity).SetActive(true);
                return true;
            });
        }
    }
´´´
All function hooks must return wheter they want the default behaviour to continue or not.

The script based abilities are:
* Mantis Claw
* Wings
* Nail
* Dash/Shade Cloak

All examples are available in their complete form [here](https://github.com/randomscorp/AbilityChanger/blob/main/Example/AbilityChangerExample.cs)
