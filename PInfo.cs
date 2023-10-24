using HarmonyLib;

namespace ShiningExhibitCollection
{
    public static class PInfo
    {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "alexi.lbol.shiningexhibitcollection";
        public const string Name = "Shining Exhibit Collection";
        public const string version = "1.0.0";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
