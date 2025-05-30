using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class ScreamEx : Scream
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "Scream Example"; set { } }

        public override string title { get => "Scream Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for Scream"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public ScreamEx() 
        {
            OnTrigger(() => GameObject.Instantiate(AbilityChangerExample.white_flower, HeroController.instance.transform.position,
                     Quaternion.identity).SetActive(true), true);
            ReplaceSpawnScream1(AbilityChangerExample.red_flower);
            ReplaceSpawnScream2(AbilityChangerExample.red_flower);
        }

    }
}
