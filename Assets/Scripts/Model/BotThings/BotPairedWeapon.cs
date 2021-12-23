using UnityEngine;


namespace MaksK_Race
{
    public sealed class BotPairedWeapon : Weapon
    {
        #region Properties

        private GameObject _trail;
        private UnitSwitcher _trailFromPool;

        private MainBot _parent;
        private TheEye _parentsEye;

        [SerializeField] private Vector3 _fixedOnTarget;

        private readonly AmmunitionType _ammo = AmmunitionType.None;

        [SerializeField] private ParticleHelper[] _weaponBarrels;

        [SerializeField] private bool _oneClicker;

        [SerializeField] private int _index = 0;

        #endregion


        #region UnityMethods

        protected override void Start()
        {
            base.Start();
            _parent = GetComponentInParent<MainBot>();
            _parentsEye = _parent.GetComponentInChildren<TheEye>();
            _weaponBarrels = GetComponentsInChildren<ParticleHelper>();
            _trail = _weaponSObject.Trail;
            _oneClicker = true;
            _shotLifeTime = 2.0f;
            _ammunition = UnitSwitcher.VFXHitBot;
            _trailFromPool = UnitSwitcher.VFXTrail;
        }

        protected override void Update()
        {
            base.Update();

            if (BotTarget != null) LookRotation();
        }

        #endregion


        #region Methods

        public override void Fire()
        {
            if (!IsReady) return;

            BotTarget = _parent.Target.position;

            if (_index > _weaponBarrels.Length - 1)
            {
                _index = 0;
            }

            EffectsPlay();

            IsReady = !IsReady;
            _oneClicker = !_oneClicker;
        }

        private void Reload()
        {
            if (_oneClicker)
            {
                _tempTimer = _rechargeTime;
                _oneClicker = !_oneClicker;
            }

            WeaponReady();
        }

        private void LookRotation()
        {
            if (Physics.Raycast(transform.position, BotTarget, out hit, _range, WhatIsSolid))
            {
                Vector3 direction = (BotTarget - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _speedOfRotation);
            }
        }

        private void EffectsPlay()
        {
            _weaponBarrels[_index].Play();            

            if (PoolCheck(_trailFromPool))
            {
                var trail = _objectPooler.SpawnFromPool(_trailFromPool,
                _weaponBarrels[_index].transform.position,
                Quaternion.LookRotation(_weaponBarrels[_index].transform.forward));
                var poolTrail = trail.SelfGameObject.GetComponent<ParticleHelper>();
                poolTrail.SelfDestroy(1, _trailFromPool, true);
            }
            else
            {
                GameObject trail = Instantiate(_trail,
                _weaponBarrels[_index].transform.position,
                Quaternion.LookRotation(_weaponBarrels[_index].transform.forward));
                Destroy(trail, 1);
            }

            _index++;

            //ObjectAnimator.Play("TurretBones|Shoot", 0, 0);
            _sound.Play();

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit ShootHit, _range, WhatIsSolid))
            {
                if (ShootHit.collider != null)
                {
                    DoDamage(ShootHit);

                    if (PoolCheck(_ammunition))
                    {
                        var shot = _objectPooler.SpawnFromPool(_ammunition,
                            ShootHit.point, Quaternion.LookRotation(ShootHit.normal));
                        ParticleHelper tempShot = shot.SelfGameObject.GetComponent<ParticleHelper>();
                        tempShot.SelfDestroy(_shotLifeTime, _ammunition, true);
                    }
                    else
                    {
                        GameObject shot = Instantiate(_theShot, ShootHit.point, Quaternion.LookRotation(ShootHit.normal));
                        ParticleHelper tempShot = shot.GetComponent<ParticleHelper>();
                        tempShot.Play();
                        tempShot.SelfDestroy(_shotLifeTime);
                    }
                }
            }
        }

        public override void DoDamage(RaycastHit tempHit)
        {
            var setDamage = tempHit.collider.GetComponent<ICollision>();

            if (setDamage != null)
            {
                setDamage.OnCollision(new InfoCollision(_damage, _parentsEye.transform));
            }

            if (tempHit.collider.gameObject.GetComponent<MainBot>() == null &&
                        tempHit.rigidbody != null)
            {
                if (tempHit.collider.GetComponent<FixedJoint>() != null)
                {
                    Destroy(tempHit.collider.GetComponent<FixedJoint>());

                    if (tempHit.collider.transform.parent != null)
                    {
                        tempHit.collider.transform.parent = null;
                    }
                }

                tempHit.rigidbody.AddForce(-tempHit.normal * _pushForce, ForceMode.Impulse);
            }
        }

        public override void AddAmmo(AmmunitionType ammunition)
        {
            if (ammunition == _ammo) Reload();
        }

        #endregion
    }
}