using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class QuakeEx : Quake
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "Quake Example"; set { } }

        public override string title { get => "Quake Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for Quake"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public QuakeEx() 
        {
            OnTrigger(() => GameObject.Instantiate(AbilityChangerExample.white_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);
            OnLandQuake1(() => GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);
            OnLandQuake2(() => GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);
        }

    }
}
