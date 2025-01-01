using System;

namespace AbilityChanger
{
    public abstract class Ability {
        /// <summary>
        /// The ability name. It acst as an indentifier for the ability inside the manager. Abilities names must be unique inside each manager
        /// </summary>
        public abstract string name { get; set; }
        /// <summary>
        /// The ability title shown when displayed inside the inventory
        /// </summary>
        public abstract string title { get; set; }
        /// <summary>
        /// The description shown inside the inventory
        /// </summary>
        public abstract string description { get; set; }
        /// <summary>
        /// The sprite shown in the inventory
        /// </summary>
        public abstract Sprite activeSprite { get; set; }
        // Do we still use this??
        public virtual Sprite inactiveSprite { get; set; }
        /// <summary>
        /// The name of the ability it'll replace/ name of the manager where it'll be contained
        /// </summary>
        public abstract string abilityType { get; }
        /// <summary>
        /// The method that controlls if the ability can be use bt AC. Default behaviour is to point to <c>aquiredAbility</c>
        /// </summary>
        /// <returns></returns>
        public virtual bool hasAbility() => aquiredAbility;
        /// <summary>
        /// The flag AC uses to save wheter the ability was aquired in the save file
        /// </summary>
        public virtual bool aquiredAbility { get; set; } = false;
        /// <summary>
        /// The method called aquire/unaquire abilities at gametime. If your ability was the first aquiried on the save (including the vannila one) it'll be auto selected regardless of <c> autoselect</c>
        /// </summary>
        /// <param name="value"> true/false => aquired/unaquired</param>
        /// <param name="autoselect">if the ability should be autoselected on aquired</param>
        public void setAquireAbility(bool value, bool autoselect = false) 
        {
            aquiredAbility = value;
            if(HeroController.instance != null)
            {
                var manager = AC.ManagersMap[abilityType];
                if (!value) manager.nextAbility();
                else if (autoselect ||  manager.acquiredAbilities().Count == 0 ) manager.setAbility(this);
            }
        }
        public bool isCustom { get; protected set; } = true;
        /// <summary>
        /// Delegate called everytime your ability is selected. All changes wanted should be registered here.
        /// </summary>
        public Action OnSelect;

        /// <summary>
        /// Delegate called everytime your ability is unselected. Changes made in manager's FSM are automatically reset while changes made in script/script base managers MUST be undone here.
        /// Changes made in the environment, destroying GameObjects, can be done here
        /// </summary>
        public Action OnUnselect;

        /// <summary>
        /// You can asks AC to switch other abilities (custom or not) when switching to your ability. The format is  { abilityType: AbilityName}
        /// </summary>
        public Dictionary<string,string> relatedAbilities = new Dictionary<string,string>();
        public Ability()
        {
            OnSelect += () =>
            {
                AC.instance.LogDebug($"{name}: Called OnSlect");

            };

            OnUnselect += () =>
            {
                AC.instance.LogDebug($"{name}: Called OnUnselect");
            };

        }
    }

    public class DefaultAbility : Ability
    {
        public override string name { get; set; }
        public override string title { get; set; }
        public override string description { get; set; }
        public Func<bool> _hasAbility { get; set; }
        public override Sprite activeSprite { get; set; }
        public override Sprite inactiveSprite { get; set; }
        public override string abilityType => null;

        public DefaultAbility(string name, Func<bool> hasAbility)
        {
            this.isCustom = false;
            this.name = name;
            this._hasAbility = hasAbility;
        }

        public override bool hasAbility()
        {
            return this._hasAbility();
        }
    }
}