using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Old_Code
{
    public sealed class Inventory : IInitialization
    {
        private List<Weapon> _weapons = new List<Weapon>();
        private PoolObjects _poolObjects;
        private int _selectIndexWeapon;

        public List<Weapon> Weapons => _weapons;

        public FlashLightModel[] FlashLight { get; private set; }

        public Inventory(PoolObjects pool = null)
        {
            if (pool != null) _poolObjects = pool;
        }

        public void Initialization()
        {
            _weapons = ServiceLocatorMonoBehaviour.GetService<BaseCar>().
                GetComponentsInChildren<Weapon>().ToList();

            foreach (var weapon in Weapons)
            {
                weapon.GetPool(_poolObjects);
                weapon.IsVisible = false;
            }

            FlashLight = Object.FindObjectsOfType<FlashLightModel>();            

            foreach (var flash in FlashLight)
            {
                flash.Switch(FlashLightActiveType.Off);
            }

            //FlashLight.Switch(FlashLightActiveType.Off);
        }

        public Weapon SelectWeapon(int weaponNumber)
        {
            if (weaponNumber < 0 || weaponNumber >= Weapons.Count) return null;
            var tempWeapon = Weapons[weaponNumber];
            _selectIndexWeapon = weaponNumber;
            return tempWeapon;
        }

        //public void AddWeapon(Weapon weapon)
        //{
        //}

        public void RemoveWeapon()
        {
            var selectWeapon = SelectWeapon(_selectIndexWeapon);
            if (selectWeapon)
            {
                Weapons.Remove(selectWeapon);
                selectWeapon.transform.parent = null;
                selectWeapon.SetActive(true);
            }
        }

        public void AmmoAdding(AmmunitionType selectedAmmo)
        {
            foreach (var weapon in Weapons)
                weapon.AddAmmo(selectedAmmo);

            ServiceLocator.Resolve<WeaponController>().On(SelectWeapon(_selectIndexWeapon));            
        }
    }
}