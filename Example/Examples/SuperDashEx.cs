using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class SuperDashEx : SuperDash
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "SuperDash Example"; set { } }

        public override string title { get => "SuperDash Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for SuperDash"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public SuperDashEx() 
        {
            OnTrigger(() => GameObject.Instantiate(AbilityChangerExample.white_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);
            DuringCharging(() => GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
                Quaternion.identity).SetActive(true));
            DuringSuperDash(() => GameObject.Instantiate(AbilityChangerExample.green_flower, HeroController.instance.transform.position,
                Quaternion.identity).SetActive(true));

        }

    }
}
