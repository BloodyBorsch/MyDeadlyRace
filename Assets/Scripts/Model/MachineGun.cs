using UnityEngine;


namespace MaksK_Race
{
    public sealed class MachineGun : Weapon
    {
        #region Properties        

        private readonly AmmunitionType _ammo = AmmunitionType.Bullet;

        private BaseCar _parent;        

        #endregion


        #region UnityMethods

        protected override void Start()
        {
            base.Start();
            _parent = GetComponentInParent<BaseCar>();
            _ammunition = UnitSwitcher.VFXHit;
        }

        protected override void Update()
        {
            base.Update();
            LookRotation();
            WeaponReady();
        }

        #endregion


        #region Methods

        public override void Fire()
        {
            if (!IsReady) return;

            if (Clip.CountAmmunition <= 0) return;

            EffectsPlay();

            if (hit.rigidbody != null)
            {
                if (hit.collider.GetComponent<FixedJoint>() != null)
                {
                    Destroy(hit.collider.GetComponent<FixedJoint>());

                    if (hit.collider.transform.parent != null)
                    {
                        hit.collider.transform.parent = null;
                    }
                }

                hit.rigidbody.AddForce(-hit.normal * _pushForce, ForceMode.Impulse);
            }

            if (hit.collider != null)
            {
                DoDamage(hit);
            }

            Clip.CountAmmunition--;
            _tempTimer = _rechargeTime;
        }

        public override void DoDamage(RaycastHit tempHit)
        {
            var setDamage = tempHit.collider.GetComponent<ICollision>();

            if (setDamage != null)
            {
                setDamage.OnCollision(new InfoCollision(_damage, _parent.Transform));
            }
        }

        private void LookRotation()
        {
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, _range, WhatIsSolid))
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _speedOfRotation);
            }
        }

        private void EffectsPlay()
        {
            foreach (var part in _particles)
            {
                part.Play();
            }

            ObjectAnimator.Play("TurretBones|Shoot", 0, 0);
            _sound.Play();

            if (PoolCheck(_ammunition))
            {      
                var shot = _objectPooler.SpawnFromPool(_ammunition, 
                    hit.point, Quaternion.LookRotation(hit.normal));
                ParticleHelper tempShot = shot.SelfGameObject.GetComponent<ParticleHelper>();
                tempShot.SelfDestroy(_shotLifeTime, _ammunition, true);   
            }
            else
            {
                GameObject shot = Instantiate(_theShot, hit.point, Quaternion.LookRotation(hit.normal));
                var tempShot = shot.GetComponent<ParticleHelper>();
                tempShot.Play();
                tempShot.SelfDestroy(_shotLifeTime);                
            }            
        }

        public override void AddAmmo(AmmunitionType ammunition)
        {
            if (ammunition == _ammo)
                AddClip(new Clip { CountAmmunition = Random.Range(_minCountAmmunition, _maxCountAmmunition) });
        }

        #endregion
    }
}