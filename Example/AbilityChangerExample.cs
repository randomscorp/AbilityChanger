using Modding;
using System;
using UnityEngine;
using AbilityChanger;
using Satchel;
using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using Satchel.Futils;
using System.Collections.Generic;

namespace AbilityChangerExample
{
    public class RedFlowerDG : Dreamgate
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("red_flower.png"); }
        public override string name { get => "Red Flower"; set { } }

        public override string title { get => "Red Flower"; set { } }
        public override string description { get => "Plants a little red delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public RedFlowerDG()
        {
            OnSelect += () =>
            {
                myFsm.Intercept(new TransitionInterceptor()
                {
                    fromState = states.Set,
                    toStateDefault = states.SpawnGate,
                    toStateCustom = AbilityChangerExample.red_flower == null ? "Set Fail" : "Set Recover",
                    eventName = "FINISHED",
                    shouldIntercept = () => true,
                    onIntercept = (a, b) => GameObject.Instantiate(AbilityChangerExample.red_flower,HeroController.instance.transform.position + new Vector3(0, -1.42f, -0.0035f),
                                            Quaternion.identity).SetActive(true)

                });
                myFsm.GetValidState("Set").GetAction(4).Enabled = false;
            };
        }

    }
    public class GreenFlowerDG : Dreamgate
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("green_flower.png"); }
        public override string name { get => "Green Flower"; set { } }
        public override string title { get => "Green Flower"; set { } }
        public override string description { get => "Plants a little green delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public GreenFlowerDG()
        {
            ReplaceSpawn(AbilityChangerExample.green_flower);
            relatedAbilities.Add(AbilityChanger.Abilities.DREAMNAIL, "Green Flower");
        }
    }

    public class RedFlowerWings : DoubleJump
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("red_flower.png"); }
        public override string name { get => "Red Flower"; set { } }
        public override string title { get => "Red Flower"; set { } }
        public override string description { get => "Plants a little red delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public RedFlowerWings()
        {
            OnSelect += () => { On.HeroController.DoubleJump += SpawnWingsFlower; };
            OnUnselect += () => { On.HeroController.DoubleJump -= SpawnWingsFlower; };
        }

        private void SpawnWingsFlower(On.HeroController.orig_DoubleJump orig, HeroController self)
        {
            GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true);
            orig(self);
        }
    }

    public class AbilityChangerExample : Mod, IMenuMod
    {
        public static GameObject white_flower, red_flower, green_flower;
        public override string GetVersion() => "1.0";

        List<Ability> abilities;
        public override void Initialize()
        {
            #region Defining GOs
            white_flower = new GameObject()
            {
                name = "white_flower"
            };
            SpriteRenderer sr = white_flower.AddComponent<SpriteRenderer>();
            Texture2D tex = AssemblyUtils.GetTextureFromResources("white_flower.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            white_flower.SetActive(false);
            GameObject.DontDestroyOnLoad(white_flower);

            green_flower = new GameObject()
            {
                name = "green_flower"
            };
            sr = green_flower.AddComponent<SpriteRenderer>();
            tex = AssemblyUtils.GetTextureFromResources("green_flower.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            green_flower.SetActive(false);
            GameObject.DontDestroyOnLoad(green_flower);

            red_flower = new GameObject()
            {
                name = "red_flower"
            };
            sr = red_flower.AddComponent<SpriteRenderer>();
            tex = AssemblyUtils.GetTextureFromResources("red_flower.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            red_flower.SetActive(false);
            GameObject.DontDestroyOnLoad(red_flower);
            #endregion

            abilities = new() {
                    new RedFlowerDG (),
                    new GreenFlowerDG(),
                    new RedFlowerWings(),
                    new Examples.CycloneSlashEx(),
                    new Examples.DashEx(),
                    new Examples.DashSlashEx(),
                    new Examples.DoubleJumpEx(),
                    new Examples.DreamGateEx(),
                    new Examples.DreamNailEx(),
                    new Examples.FireballEx(),
                    new Examples.FocusEx(),
                    new Examples.GreatSlashEx(),
                    new Examples.NailEx(),
                    new Examples.QuakeEx(),
                    new Examples.ScreamEx(),
                    new Examples.SuperDashEx(),
                    new Examples.WallJumpEx(),
                };

            foreach (var ability in abilities)
            {
                RegisterAbility(ability);
            }
        }

        public bool ToggleButtonInsideMenu => false;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            List<IMenuMod.MenuEntry> entries = new();
            {
                foreach (Ability ability in abilities)
                {
                    entries.Add(new IMenuMod.MenuEntry()
                    {
                        Name = ability.name,
                        Description = $"[{ability.abilityType}]" + ability.description,
                        Values = new string[] { "Unaquired", "Aquired" },
                        Saver = opt =>
                        {
                            ability.setAquireAbility(opt switch { 0 => false, 1 => true, _ => throw new InvalidOperationException() });
                        },
                        Loader = () => ability.hasAbility() switch { false => 0, true => 1 }
                    });
                }
            }
            return entries;
        }
    }
}