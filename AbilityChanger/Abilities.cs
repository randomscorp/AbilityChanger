namespace AbilityChanger
{
    public static class Abilities {

        public static string DASH { get; } = "Dash";
        public static string DREAMGATE { get; } = "Dream Gate";
        public static string WALLJUMP { get; } = "WallJump";
        public static string DASHSLASH { get; } = "Dash Slash";
        public static string CYCLONESLASH { get; } = "Cyclone Slash";
        public static string GREATSLASH { get; } = "Great Slash";
        public static string SCREAM { get; } = "Scream";
        public static string NAIL { get; } = "Nail";
        public static string FIREBALL { get; } = "Fireball";
        public static string QUAKE { get; } = "Quake";
        public static string FOCUS { get; } = "Focus";
        public static string DOUBLEJUMP { get; } = "DoubleJump";
        public static string DREAMNAIL { get; } = "Dream Nail";
        public static string SUPERDASH { get; } = "Super Dash";

        // pending to implement 
        public static string ACIDARMOUR { get; } = "Acid Armour";
        public static string LANTERN { get; } = "Lantern";
    }
    
    public static class AbilitiesFSMs
    {
        public static string DREAMNAIL { get; } = "Dream Nail";
        public static string NAILARTS { get; } = "Nail Arts";
        public static string SPELLCONTROL { get; } = "Spell Control";
        public static string SUPERDASH { get; } = "Superdash";

        public static string[] names = new string[] { NAILARTS, DREAMNAIL, SPELLCONTROL, SUPERDASH };

    }
}