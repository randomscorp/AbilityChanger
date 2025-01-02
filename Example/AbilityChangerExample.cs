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
    public class WhiteFlowerDG : Dreamgate
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png"); }
        public override string name { get => "White Flower"; set { } }

        public override string title { get => "White Flower"; set { } }
        public override string description { get => "Plants a little white delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

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
    public class GreenFlowerDG : Dreamgate
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower3.png"); }
        public override string name { get => "Green Flower"; set { } }
        public override string title { get => "Green Flower"; set { } }
        public override string description { get => "Plants a little green delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public GreenFlowerDG()
        {
            ReplaceSpawn(AbilityChangerExample.flower3);
            relatedAbilities.Add(AbilityChanger.Abilities.DREAMNAIL, "Green Flower");
        }
    }

    public class RedFlowerDN : DreamNail
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower2.png"); }
        public override string name { get => "Green Flower"; set { } }
        public override string title { get => "Green Flower"; set { } }
        public override string description { get => "Plants a little green delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public RedFlowerDN()
        {
            OnSelect += () =>
                {
                    myFsm.AddAction("Slash", new CustomFsmAction(() =>
                    {
                        GameObject.Instantiate(AbilityChangerExample.flower2,
                                                HeroController.instance.transform.position, Quaternion.identity).SetActive(true);
                    }));
                };

            relatedAbilities.Add(AbilityChanger.Abilities.DREAMGATE, "Green Flower");

        }
    }

    public class WhiteFlowerWings : DoubleJump
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower.png"); }
        public override string name { get => "White Flower"; set { } }
        public override string title { get => "White Flower"; set { } }
        public override string description { get => "Plants a little green delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

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

    public class RedFlowerWings : DoubleJump
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower2.png"); }
        public override string name { get => "Red Flower"; set { } }
        public override string title { get => "Red Flower"; set { } }
        public override string description { get => "Plants a little green delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public RedFlowerWings()
        {
            RegisterOnDoDoubleJump(() =>
            {
                GameObject.Instantiate(AbilityChangerExample.flower2, HeroController.instance.transform.position,
                                     Quaternion.identity).SetActive(true);
                return true;
            });
        }
    }

    public class GreenFlowerScream : Scream
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("flower3.png"); }
        public override string name { get => "Green Flower"; set { } }
        public override string title { get => "Green Flower"; set { } }
        public override string description { get => "Plants a little green delicate flower"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public GreenFlowerScream()
        {
            RegisterTrigger(() =>
            {
                GameObject.Instantiate(AbilityChangerExample.flower3, HeroController.instance.transform.position,
                                     Quaternion.identity).SetActive(true);
            },false);
        }
    }

    public class AbilityChangerExample : Mod, IMenuMod
    {
        public static GameObject flower, flower2, flower3;
        public override string GetVersion() => "1.0";
        Ability[] abilities = { 
            new WhiteFlowerDG(),
            new GreenFlowerDG(),
            new RedFlowerDN(),
            new RedFlowerWings(),
            new WhiteFlowerWings(),
            new GreenFlowerScream()
        };
        public override void Initialize()
        {
            #region Defining GOs
            flower = new GameObject()
            {
                name = "flower"
            };
            SpriteRenderer sr = flower.AddComponent<SpriteRenderer>();
            Texture2D tex = AssemblyUtils.GetTextureFromResources("flower.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower.SetActive(false);
            GameObject.DontDestroyOnLoad(flower);

            flower2 = new GameObject()
            {
                name = "flower2"
            };
            sr = flower2.AddComponent<SpriteRenderer>();
            tex = AssemblyUtils.GetTextureFromResources("flower2.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower2.SetActive(false);
            GameObject.DontDestroyOnLoad(flower2);

            flower3 = new GameObject()
            {
                name = "flower3"
            };
            sr = flower3.AddComponent<SpriteRenderer>();
            tex = AssemblyUtils.GetTextureFromResources("flower3.png");
            sr.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 128f, 0, SpriteMeshType.FullRect);
            sr.color = new Color(1f, 1f, 1f, 1.0f);
            flower3.SetActive(false);
            GameObject.DontDestroyOnLoad(flower3);
            #endregion

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
                        Loader = () => ability.aquiredAbility switch { false => 0, true => 1 }
                    });
                }
            }
            return entries;
        }
    }
}