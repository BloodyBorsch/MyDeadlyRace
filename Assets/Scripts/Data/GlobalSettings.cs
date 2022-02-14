using UnityEngine;


namespace MyRacing
{
    [CreateAssetMenu(fileName = nameof(GlobalSettings), menuName = NameManager.CREATE_DATA_MENU_NAME + nameof(GlobalSettings))]
    public sealed class GlobalSettings : ScriptableObject
    {
        public WeaponData WeaponsData;
        public CarData CarsData;
    }
}
