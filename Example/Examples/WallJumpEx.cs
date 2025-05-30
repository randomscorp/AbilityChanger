using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class WallJumpEx : WallJump
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "WallJump Example"; set { } }

        public override string title { get => "WallJump Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for WallJump"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public WallJumpEx()
        {
            OnTrigger(() =>
            {
                GameObject.Instantiate(AbilityChangerExample.white_flower, HeroController.instance.transform.position, Quaternion.identity).SetActive(true);
                return true;

            });

        }

    }
}
