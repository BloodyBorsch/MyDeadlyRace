using System.Linq;
using UnityEngine;


namespace MyRacing
{
    [CreateAssetMenu(fileName = nameof(WeaponData), menuName = NameManager.CREATE_DATA_MENU_NAME + nameof(WeaponData))]
    public sealed class WeaponData : ScriptableObject
    {
        public WeaponSettings[] Weapons;

        public WeaponSettings LoadWeaponSettings(WeaponType type)
        {
            return Weapons.Where(w => w.AskTypeOfData() == type).FirstOrDefault();
        }
    }
}