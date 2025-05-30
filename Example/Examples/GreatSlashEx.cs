using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class GreatSlashEx : GreatSlash
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "GreatSlash Example"; set { } }

        public override string title { get => "GreatSlash Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for GreatSlash"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public GreatSlashEx() 
        {
            ReplaceSpawn(AbilityChangerExample.white_flower);
            OnTrigger(() => GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);

        }

    }
}
