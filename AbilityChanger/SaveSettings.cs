using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityChanger
{
    public class SaveSettings
    {
        public List<SavedAbility> savedAbilities = new List<SavedAbility>();
        public Dictionary<string,string> equipedAbilities = new Dictionary<string,string>();
    }

    [Serializable]
    public class SavedAbility
    {
        public string abilityName { get; set; }
        public string managerName { get; set; }
        public bool aquiredAbility { get; set; }
    }
}
