using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class FocusEx : Focus
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "Focus Example"; set { } }

        public override string title { get => "Focus Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for Focus"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public FocusEx() 
        {
            OnTrigger(() => GameObject.Instantiate(AbilityChangerExample.white_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);
            DuringFocus(() => GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
                Quaternion.identity).SetActive(true));
        }

    }
}
