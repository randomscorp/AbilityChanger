using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class CycloneSlashEx: CycloneSlash
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "Cyclone Example"; set { } }

        public override string title { get => "Cyclone Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for Cyclone"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public CycloneSlashEx() 
        {
            //ReplaceSpawn(AbilityChangerExample.white_flower);
            OnTrigger(() => GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);
            DuringSlash(() => GameObject.Instantiate(AbilityChangerExample.green_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true));

        }

    }
}
