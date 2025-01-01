namespace AbilityChanger
{
    internal static class PlayerDataPatcher{
        internal static string hasNailArt = nameof(PlayerData.hasNailArt);
        internal static string hasAllNailArts = nameof(PlayerData.hasAllNailArts);
        internal static string hasCyclone = nameof(PlayerData.hasCyclone);
        internal static string hasUpwardSlash = nameof(PlayerData.hasUpwardSlash); //tc weirdness
        internal static string hasDashSlash = nameof(PlayerData.hasDashSlash); //tc weirdness
        internal static string hasDreamNail = nameof(PlayerData.hasDreamNail);
        internal static string hasDreamGate = nameof(PlayerData.hasDreamGate);
        internal static string canDash = nameof(PlayerData.canDash);
        internal static string hasDash = nameof(PlayerData.hasDash);
        internal static string hasWalljump = nameof(PlayerData.hasWalljump);
        internal static string hasSuperDash = nameof(PlayerData.hasSuperDash);
        internal static string hasDoubleJump = nameof(PlayerData.hasDoubleJump);
        internal static string hasAcidArmour = nameof(PlayerData.hasAcidArmour);
        internal static string hasLantern = nameof(PlayerData.hasLantern);
        internal static string fireballLevel = nameof(PlayerData.fireballLevel);
        internal static string quakeLevel = nameof(PlayerData.quakeLevel);
        internal static string screamLevel = nameof(PlayerData.screamLevel);
       // internal static string screamLevel = nameof(PlayerData.has);

        static PlayerDataPatcher(){
        }
        public static bool GetBool(string name){
            return PlayerData.instance.GetBool(name);
        }
        public static bool GetBoolInternal(string name){
            return PlayerData.instance.GetBoolInternal(name);
        }

        public static int GetIntInternal(string name)
        {
            return PlayerData.instance.GetIntInternal(name);
        }


        public static bool OnGetPlayerBoolHook(string name,bool orig){

            if(name == hasNailArt){
                return (AbilityChanger.ManagersMap[Abilities.CYCLONESLASH].hasAcquiredAbility() ||
                 AbilityChanger.ManagersMap[Abilities.DASHSLASH].hasAcquiredAbility() || 
                 AbilityChanger.ManagersMap[Abilities.GREATSLASH].hasAcquiredAbility());
            }
            if(name == hasAllNailArts){
                return (AbilityChanger.ManagersMap[Abilities.CYCLONESLASH].hasAcquiredAbility() &&
                 AbilityChanger.ManagersMap[Abilities.DASHSLASH].hasAcquiredAbility() && 
                 AbilityChanger.ManagersMap[Abilities.GREATSLASH].hasAcquiredAbility());
            }
            if(name == hasCyclone){
                return AbilityChanger.ManagersMap[Abilities.CYCLONESLASH].hasAcquiredAbility(); 
            }
            if(name == hasUpwardSlash){
                return AbilityChanger.ManagersMap[Abilities.DASHSLASH].hasAcquiredAbility(); 
            }
            if(name == hasDashSlash){
                return AbilityChanger.ManagersMap[Abilities.GREATSLASH].hasAcquiredAbility();
            }
            if(name == hasDreamGate){
                return AbilityChanger.ManagersMap[Abilities.DREAMGATE].hasAcquiredAbility(); 
            }
            if (name == hasDreamNail)
            {
                return AbilityChanger.ManagersMap[Abilities.DREAMGATE].hasAcquiredAbility();
            }
            if (name == hasDash || name == canDash){
                return AbilityChanger.ManagersMap[Abilities.DASH].hasAcquiredAbility(); 
            }
            if (name == hasDoubleJump)
            {
                return AbilityChanger.ManagersMap[Abilities.DOUBLEJUMP].hasAcquiredAbility();
            }
            if (name == hasSuperDash)
            {
                return AbilityChanger.ManagersMap[Abilities.SUPERDASH].hasAcquiredAbility();
            }

            return orig;
        }
    }
}