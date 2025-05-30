using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class FireballEx : Fireball
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "Fireball Example"; set { } }

        public override string title { get => "Fireball Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for Fireball"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public FireballEx() 
        {
            OnTrigger(() => GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);
            ReplaceSpawnFireball1(AbilityChangerExample.red_flower);
            ReplaceSpawnFireball2(AbilityChangerExample.red_flower);
        }

    }
}
