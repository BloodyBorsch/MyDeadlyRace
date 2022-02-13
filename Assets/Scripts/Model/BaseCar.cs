using UnityEngine;


namespace Old_Code
{
    public sealed class BaseCar : BaseObjectScene
    {
        #region Properties

        [SerializeField] private BaseCarSObject _carSObject;
        [SerializeField] private ParticleHelper _deathFire;
        [SerializeField] private Transform _centerOfMass;
        private HealthBarUI _healthBar;
        private AudioHelper _audio;

        private Wheel[] _wheels;

        [SerializeField] private LayerMask WhatIsSolid;

        [SerializeField] private float _motorTorque { get; set; }
        [SerializeField] private float _steer { get; set; }
        [SerializeField] private float _throttle { get; set; }

        private float _maxSteer = 1400.0f;
        private float _jumpForce = 300.0f;
        private float _KPHcoefficient = 3.6f;
        private float _baseDamage = 10.0f;
        [SerializeField] private float _speedDamage;

        private float _firstGearForce;
        private float _secondGearForce;
        private float _thirdGearForce;
        private float _backGearForce;

        private float _firstGearMagnitude = 20.0f;
        private float _SecondGearMagnitude = 50.0f;
        private float _thirdGearMagnitude = 80.0f;

        private float _firstGearPitch = 1.0f;
        private float _SecondGearPitch = 1.1f;
        private float _thirdGearPitch = 1.2f;
        private float _backGearPitch = 0.8f;

        private float _airTimer = 0.0f;
        private float _groundCheckDistance = 0.6f;

        private const string _audioPitch = "Idle";

        [SerializeField] private bool _inAir;
        [SerializeField] private bool _isGrounded;

        public bool IsDead = false;

        public float Speed;
        public float Health;
        public float MaxHealth = 500;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _motorTorque = _carSObject.MotorTorque;
            _firstGearForce = _carSObject.FirstGear;
            _secondGearForce = _carSObject.SecondGear;
            _thirdGearForce = _carSObject.ThirdGear;
            _backGearForce = _carSObject.BackGear;
            Rigidbody.centerOfMass = _centerOfMass.localPosition;
            _wheels = GetComponentsInChildren<Wheel>();
            Health = MaxHealth;
            _healthBar = FindObjectOfType<HealthBarUI>();
            _healthBar.SetMaxHealth(Health);
            _audio = GetComponent<AudioHelper>();
        }

        public void Ride()
        {           
            Speed = Rigidbody.velocity.magnitude * _KPHcoefficient;
            _speedDamage = _baseDamage + Rigidbody.velocity.magnitude;

            Groundcheck();            

            if (!_isGrounded) return;

            _steer = ServiceLocator.Resolve<InputController>().SteerInput;
            _throttle = ServiceLocator.Resolve<InputController>().ThrottleInput;            
        }

        public void FixedRide()
        {
            foreach (var wheel in _wheels)
            {
                wheel.SteerAngle = _steer * _maxSteer * Time.fixedDeltaTime;
                wheel.Torque = _throttle * _motorTorque * Time.fixedDeltaTime;
            }

            GearShifter(Speed);

            AirRotation();
        }

        private void OnEnable()
        {
            var corpse = GetComponentInChildren<CarCorpse>();
            if (corpse != null) corpse.CarDestruction += CarHealth;
        }

        private void OnDisable()
        {
            var corpse = GetComponentInChildren<CarCorpse>();
            if (corpse != null) corpse.CarDestruction -= CarHealth;
        }

        #endregion


        #region Methods

        private void GearShifter(float speedValue)
        {
            if (_throttle > 0)
            {
                if (speedValue > 0 && speedValue < _firstGearMagnitude) AddGearForce(_firstGearForce, _firstGearPitch);
                if (speedValue > _firstGearMagnitude && speedValue < _SecondGearMagnitude) AddGearForce(_secondGearForce, _SecondGearPitch);
                if (speedValue > _SecondGearMagnitude && speedValue < _thirdGearMagnitude) AddGearForce(_thirdGearForce, _thirdGearPitch);
            }

            if (_throttle < 0) AddGearForce(_backGearForce, _backGearPitch);
        }

        private void AddGearForce(float force, float pitch)
        {
            Rigidbody.AddForce(transform.forward * force, ForceMode.Acceleration);
            _audio.Pitch(_audioPitch, pitch);
        }

        private bool Groundcheck()
        {
            if (Physics.Raycast(_centerOfMass.transform.position, Vector3.down, out RaycastHit hit, _groundCheckDistance, WhatIsSolid))
            {
                Debug.DrawRay(_centerOfMass.transform.position, Vector3.down * _groundCheckDistance, Color.red);
                if (hit.collider != null) _isGrounded = true;
            }
            else _isGrounded = false;

            if (_airTimer >= 0)
            {
                _airTimer -= Time.deltaTime;
            }
            else
            {
                _inAir = false;
            }

            return _isGrounded;
        }

        public void ReturnOnWheels()
        {
            Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Acceleration);
            _airTimer = 2.0f;
            _inAir = true;
        }

        private void AirRotation()
        {
            if (_inAir)
            {
                var eulers = Vector3.zero;
                eulers.z += 2;
                Transform.Rotate(eulers);
            }
        }

        public void BrakesActivation(bool IsActivated)
        {
            foreach (var wheel in _wheels)
            {
                wheel.Breakes = IsActivated;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var setDamage = collision.gameObject.GetComponent<ICollision>();

            if (setDamage != null)
            {
                setDamage.OnCollision(new InfoCollision(_speedDamage, Transform));
            }
        }

        public void CarHealth(InfoCollision info)
        {
            if (Health > 0)
            {
                if (info.ObjCollision.GetComponent<RepairBox>() != null)
                {
                    var allyBot = ServiceLocator.Resolve<BotController>().FriendlyBot;

                    if (Health <= MaxHealth) Health += info.Damage;

                    if (allyBot != null)
                    {
                        allyBot.Hp += info.Damage;
                        allyBot.MenuCanvas.FriendlyBotMenu.SetHealth(allyBot.Hp);
                        if (allyBot.Hp > allyBot.MaximumHp) allyBot.Hp = allyBot.MaximumHp;
                    }
                }
                else
                {
                    Health -= info.Damage;
                }
            }

            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            _healthBar.SetHealth(Health);

            if (Health <= 0)
            {
                ServiceLocator.Resolve<InputController>().Off();
                ServiceLocator.Resolve<WeaponController>().Off();
                BrakesActivation(true);
                _deathFire.Play();
                _deathFire.FreezeRotation = true;
                IsDead = true;
            }
        }

        #endregion
    }
}
