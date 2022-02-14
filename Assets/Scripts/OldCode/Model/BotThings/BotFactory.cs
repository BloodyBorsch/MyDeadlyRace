using UnityEngine;


namespace Old_Code
{
    internal static class BotFactory
    {
        public static MainBot CreateBot(UnitSwitcher unit, Transform root = null)
        {
            string resourceLoader = ResourceLoadHelper.Loader(unit);
            return Resources.Load<MainBot>(resourceLoader);
        }
    }
}
