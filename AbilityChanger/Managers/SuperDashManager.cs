using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityChanger
{
    public class SuperDashManager : AbilityManagerFSM
    {
        public override string abilityName { get; protected set; } = Abilities.SUPERDASH;
        public override bool hasDefaultAbility()=> PlayerDataPatcher.GetBoolInternal(PlayerDataPatcher.hasSuperDash);
        public override string inventoryTitleKey { get; protected set; } = "INV_NAME_SUPERDASH";
        public override string inventoryDescKey { get; protected set; } = "INV_DESC_SUPERDASH";
        public override string fsmName => AbilitiesFSMs.SUPERDASH;

        public SuperDashManager():base()
        {
            
        }
        public override GameObject getIconGo()=>  InvGo.Find("Super Dash");
    }
}
