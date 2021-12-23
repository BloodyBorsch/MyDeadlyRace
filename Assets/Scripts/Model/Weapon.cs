using System.Collections.Generic;
using UnityEngine;


namespace MaksK_Race
{
    public abstract class Weapon : BaseObjectScene, IAmmoAdding
    {
        #region Properties

        public int CountClip => _clips.Count;

        protected int _maxCountAmmunition = 40;
        protected int _minCountAmmunition = 20;

        [SerializeField] private int _countClip = 5;
        
        public bool IsReady;

        protected float _rechargeTime;
        [SerializeField] protected float _tempTimer;
        protected float _damage;
        protected float _range;
        protected float _speedOfRotation;
        protected float _shotLifeTime = 1.0f;
        [SerializeField] protected float _pushForce;

        [SerializeField] public Vector3 BotTarget;

        public Clip Clip;
        public LayerMask WhatIsSolid;        
        
        protected ParticleSystem[] _particles;
        protected UnitSwitcher _ammunition;
        protected AudioSource _sound;
        [SerializeField] protected BaseWeaponSObject _weaponSObject;
        protected GameObject _theShot;
        private Queue<Clip> _clips = new Queue<Clip>();        

        #endregion


        #region UnityMethods

        protected virtual void Start()
        {
            _rechargeTime = _weaponSObject.RapidOfFire;
            _tempTimer = _rechargeTime;
            _damage = _weaponSObject.Damage;
            _range = _weaponSObject.EffectiveDistance;
            _speedOfRotation = _weaponSObject.RotationSpeed;
            _theShot = _weaponSObject.HitEffect;
            _pushForce = _weaponSObject.PushForce;
            _sound = GetComponent<AudioSource>();

            for (var i = 0; i <= _countClip; i++)
            {
                AddClip(new Clip { CountAmmunition = Random.Range(_minCountAmmunition, _maxCountAmmunition) });
            }
            
            _particles = GetComponentsInChildren<ParticleSystem>();

            ReloadClip();
        }

        protected virtual void Update()
        {
            //WeaponReady();            
        }

        #endregion


        #region Methods

        public abstract void Fire();

        protected bool WeaponReady()
        {
            bool value = false;

            if (_tempTimer > 0)
            {
                value = false;
                _tempTimer -= Time.deltaTime;
            }
            else
            {
                value = true;
            }

            return IsReady = value;
        }

        protected bool PoolCheck(UnitSwitcher unit)
        {
            return (_objectPooler != null && _objectPooler.PoolDictionary.ContainsKey(unit));
        }

        protected void AddClip(Clip clip)
        {
            _clips.Enqueue(clip);
        }

        public void ReloadClip()
        {
            if (CountClip <= 0) return;
            Clip = _clips.Dequeue();
        }

        public abstract void AddAmmo(AmmunitionType ammoType);

        public abstract void DoDamage(RaycastHit tempHit);

        #endregion
    }
}