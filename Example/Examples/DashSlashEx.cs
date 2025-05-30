using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class DashSlashEx: DashSlash
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "Dash Slash Example"; set { } }

        public override string title { get => "Dash Slash Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for Dash Slash"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public DashSlashEx() 
        {
            ReplaceSpawn(AbilityChangerExample.white_flower);
            //OnTrigger(() => GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
            //         Quaternion.identity).SetActive(true), true);

        }

    }
}
