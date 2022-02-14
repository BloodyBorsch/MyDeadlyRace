using System;


namespace Old_Code
{
    internal static class ResourceLoadHelper
    {
        private static string _pathToFlyingBotPref = "Bots/FlyingBot";
        private static string _pathToBotWeaponSO = "Weapons/BotWeapon";
        private static string _pathToTrail = "VFX/Trail";
        private static string _pathToHitVFX = "VFX/HitVFX";
        private static string _pathToHitBotVFX = "VFX/HitVFXBot";

        public static string Loader(UnitSwitcher unit)
        {
            switch (unit)
            {
                case UnitSwitcher.FlyingBot:
                    return _pathToFlyingBotPref;
                case UnitSwitcher.BotWeapon:
                    return _pathToBotWeaponSO;
                case UnitSwitcher.VFXTrail:
                    return _pathToTrail;
                case UnitSwitcher.VFXHit:
                    return _pathToHitVFX;
                case UnitSwitcher.VFXHitBot:
                    return _pathToHitBotVFX;
                default:
                    throw new ArgumentException("Ошибка в Загрузчике ресурсов");
            }
        }
    }
}
