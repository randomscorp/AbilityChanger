using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using static Satchel.FsmUtil;

namespace AbilityChanger
{
    public abstract class AbilityManager {

        protected List<Ability> options;

        public Ability currentAbility;

        protected GameObject InvGo;

        public abstract string abilityName { get; protected set; }

        public abstract string inventoryTitleKey { get; protected set; }

        public abstract string inventoryDescKey { get; protected set; }
        public virtual List<string> relatedManagers { get => new(); }
        public abstract bool hasDefaultAbility();
        public void addAbility(Ability ability){
            options.Add(ability);
        }

        public void removeAbility(string abilityName){
            options = options.Where( a => a.name != abilityName).ToList();
        }        

        public bool isCustom(){
            return hasAcquiredAbility() && getCurrentAbility().isCustom;
        }

        public Ability getCurrentAbility(){
            var validOptions = acquiredAbilities();
            return currentAbility ?? validOptions[0];
        }

        public Ability getAbility(string abilityName)
        {
            return acquiredAbilities().Find(a => a.name == abilityName);
        }
        public Ability getRegisteredAbility(string abilityName)
        {
            return options.Find(a => a.name == abilityName);
        }
        public Ability getDefaultAbility(){
            return options.First(a => a.isCustom == false);
        }


        public Ability nextAbility(){

            List<Ability> validOptions = acquiredAbilities();
            if(validOptions.Count > 0)
            {
                int currentIndex = currentAbility != null ? validOptions.FindIndex(a => a.name == currentAbility.name) : 0;
                Ability nextAbility = validOptions.Count() > currentIndex + 1 ? validOptions[currentIndex + 1] : validOptions[0];
                setAbility(nextAbility);
            } 
            return currentAbility;
        }

        [Obsolete]
        public  Ability prevAbility()
        {
            var validOptions = acquiredAbilities();
            var currentIndex = currentAbility != null ? validOptions.FindIndex(a => a.name == currentAbility.name) : 0;
            Ability prevAbility = currentIndex != 0 ? validOptions[currentIndex  -1] : validOptions.Last();
            setAbility(prevAbility);
            return currentAbility;
        }

        public void setAbility(string abilityName)
        {
            Ability ability = acquiredAbilities().Find(a => a.name == abilityName);
            if (ability != null)
            {
                setAbility(ability);
            }
            else AC.instance.Log($"Ability with name {ability} not found");

        }
        internal virtual Ability setAbility(Ability ability, List<AbilityManager> prevManagers = null)
        {
            prevManagers ??= new List<AbilityManager>();
            try
            {
                currentAbility.OnUnselect();
                foreach ( KeyValuePair<string,string> managerName_relatedAbiltyName in ability.relatedAbilities)
                {
                    AbilityManager manager = AC.ManagersMap[managerName_relatedAbiltyName.Key];
                    // Don't allow the ability to require an ability from the same manager or from a prev called manager
                    if (manager != this && !prevManagers.Contains(this))
                    {
                        Ability relatedAbility = manager.acquiredAbilities().Find(a => a.name == managerName_relatedAbiltyName.Value);
                        if(relatedAbility != null)
                        {
                            // Prevents circular references
                            prevManagers.Add(this);
                            manager.currentAbility = setAbility(relatedAbility, prevManagers);
                            manager.updateInventory();
                        }
                    }
                    else AC.instance.LogWarn($"Circular reference found when setting {ability.name}");
                }
                // We need to redo all changes made by abilities in the same FSM/code
                foreach (string relatedManager in relatedManagers)
                {
                    AC.ManagersMap[relatedManager].currentAbility.OnSelect();
                }
                currentAbility = ability;
                currentAbility.OnSelect();
            }
            catch (Exception ex)
            {
                AC.instance.LogError($"Erro in setting {ability.name} ability");
                AC.instance.LogError(ex);
            }
            return currentAbility;

        }

        public bool hasAcquiredAbility(){
            return options.Any( a => a.hasAbility());
        }

        public List<Ability> acquiredAbilities(){
            return options.Where( a => a.hasAbility()).ToList();
        }

        internal List<Ability> registeredAbilities() => options;

        public AbilityManager(){
            currentAbility = new DefaultAbility(abilityName, hasDefaultAbility);
            options = new(){ currentAbility };
            On.PlayMakerFSM.OnEnable += InventoryManagement;
            On.PlayMakerFSM.OnEnable += OnFsmEnable;
            ModHooks.LanguageGetHook += LanguageGet;
        }
        
        public abstract GameObject getIconGo();
        
        public virtual void OnFsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self){
            orig(self);
        }

        public virtual void updateIcon(GameObject icon){
            var defaultAbility = getDefaultAbility();
            var itemdisplay = icon.GetComponent<InvItemDisplay>();
            if(itemdisplay != null){
                if(defaultAbility.activeSprite == null){
                    defaultAbility.activeSprite = itemdisplay.activeSprite;
                }
                if(defaultAbility.inactiveSprite == null){
                    defaultAbility.inactiveSprite = itemdisplay.inactiveSprite;
                }
                if(currentAbility.activeSprite != null){
                    
                    itemdisplay.activeSprite = currentAbility.activeSprite;
                }
                if(currentAbility.inactiveSprite != null){
                    itemdisplay.inactiveSprite = currentAbility.inactiveSprite;
                }
                
                itemdisplay.SendMessage("OnEnable");
            } else {
                var spriteRenderer = icon.GetComponent<SpriteRenderer>();
                if(defaultAbility.activeSprite == null){
                    defaultAbility.activeSprite = spriteRenderer.sprite;
                }
                if(currentAbility.activeSprite != null){
                    spriteRenderer.sprite = currentAbility.activeSprite;
                }
            }
        }

        private void updateText(){
            if(InvGo != null){
                InvGo.LocateMyFSM("Update Text").Fsm.Event("UPDATE TEXT");
            }
        }
        
        private void InventoryManagement(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.gameObject.name == "Inv" && self.FsmName == "Update Text")
            {
                InvGo = self.gameObject;
            }
            if (self.gameObject.name == "Inventory" && self.FsmName == "Inventory Control")
            {
                self.AddCustomAction("Opened",() => {
                    if(hasAcquiredAbility()){
                        //Add propper inventory management later
                        try { updateInventory(); }
                        catch { }
                        
                    }
                });
            }
        }
        
        public void updateInventory(){
            updateText();
            if (InvGo != null)
            {
                var icon = getIconGo();
                if(icon != null){
                    updateIcon(icon);
                }
            }
        }
        
        private string LanguageGet(string title, string sheet, string orig){
            if(sheet == "UI" && title == inventoryTitleKey && isCustom()){
                   return currentAbility.title;
            }
            
            if(sheet == "UI" && title == inventoryDescKey && options.Count() > 1){
                string final = isCustom() ? orig: currentAbility.description;
                return $"Press the confirm button to cycle abilities. <br><br>{final}";
            }
            return orig;
        }

    }
}