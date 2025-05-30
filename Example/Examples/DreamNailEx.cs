using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class DreamNailEx : DreamNail
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "DreamNailEx Example"; set { } }

        public override string title { get => "DreamNailEx Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for DreamNailEx"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public DreamNailEx() 
        {
            //ReplaceSpawn(AbilityChangerExample.white_flower);
            OnInpact((GameObject go) => { GameObject.Instantiate(AbilityChangerExample.red_flower, go.transform.position, Quaternion.identity).SetActive(true); return true; });
        }

    }
}
