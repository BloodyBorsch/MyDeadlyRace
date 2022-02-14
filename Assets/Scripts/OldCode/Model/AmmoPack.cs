using UnityEngine;


namespace Old_Code
{
    public sealed class AmmoPack : BaseDropLoot
    {
        #region Fields 

        [SerializeField] private AmmunitionType _ammoType;        

        private readonly int MaxCountOfAmmoTypes = 2; //значит один объект

        #endregion


        #region Methods     

        protected override void Start()
        {
            base.Start();
            RandomAmmo();
        }        

        private void OnCollisionEnter(Collision collision)
        {
            var getAmmo = collision.gameObject.GetComponent<BaseCar>();

            if (getAmmo != null)
            {
                if (collision.gameObject.GetComponentInChildren<Weapon>() != null)
                {
                    AddAmmo();
                }                
            }
        }

        public AmmunitionType RandomAmmo()
        {
            int random = Random.Range(0, MaxCountOfAmmoTypes);

            switch (random)
            {
                //case 0:
                //    return _ammoType = AmmunitionType.Pellet;
                default:
                    return _ammoType = AmmunitionType.Bullet;
            }
        }

        public void AddAmmo()
        {
            ServiceLocator.Resolve<Inventory>().AmmoAdding(_ammoType);
            Destroy(gameObject);
        }

        #endregion
    }
}
