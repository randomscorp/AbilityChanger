using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class DreamGateEx : Dreamgate
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "DreamGate Example"; set { } }

        public override string title { get => "DreamGate Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for DreamGate"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public DreamGateEx() 
        {
            ReplaceSpawn(AbilityChangerExample.white_flower);

        }

    }
}
