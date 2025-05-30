using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class DoubleJumpEx : DoubleJump
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "Double Jump Example"; set { } }

        public override string title { get => "Double Jump Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for Double Jump"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public DoubleJumpEx()
        {
            OnTrigger(() =>
            {
                GameObject.Instantiate(AbilityChangerExample.white_flower, HeroController.instance.transform.position, Quaternion.identity).SetActive(true);
                return true;

            });

            DuringDoubleJump(()=>
            {
                GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position, Quaternion.identity).SetActive(true);
                return true;
            });

        }

    }
}
