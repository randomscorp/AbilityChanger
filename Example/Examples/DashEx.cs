﻿using static AbilityChanger.AbilityChanger;
using AbilityChanger.Base;
using UnityEngine;

namespace AbilityChangerExample.Examples
{
    public class DashEx: Dash
    {
        static Sprite getActiveSprite() { return Satchel.AssemblyUtils.GetSpriteFromResources("white_flower.png"); }
        public override string name { get => "Dash Example"; set { } }

        public override string title { get => "Dash Example"; set { } }
        public override string description { get => "This example showcases the default hooks available for Dash"; set { } }
        public override Sprite activeSprite { get => getActiveSprite(); set { } }

        public DashEx()
        {
            OnTrigger(() =>
            {
                GameObject.Instantiate(AbilityChangerExample.white_flower, HeroController.instance.transform.position, Quaternion.identity).SetActive(true);
                return true;

            });

            OnDashing(()=>
            {
                GameObject.Instantiate(AbilityChangerExample.red_flower, HeroController.instance.transform.position, Quaternion.identity).SetActive(true);
                return true;
            });

        }

    }
}
