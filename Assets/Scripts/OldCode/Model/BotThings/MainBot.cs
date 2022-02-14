using System;
using UnityEngine;


namespace Old_Code
{
    public sealed class MainBot : BaseObjectScene, IPooledObject
    {
        #region Properties 

        public float Hp;
        public float MaximumHp = 500;

        public Weapon Weapon;
        public Transform Target;
        public GameObject FloatingText;
        public MenuPause MenuCanvas;

        private TheEye _eye;
        private ObjectRotation _engine;               

        [SerializeField] private AmmoPack _packOfAmmo;
        [SerializeField] private RepairBox _repairBox;
        [SerializeField] private ParticleHelper _selfExplosion;
        
        public event Action<MainBot> OnDieChange;

        private float _waitTime = 3.0f;
        private float _deathTime = 2.0f;
        private float _stoppingDistance = 2.0f;
        private float _nearDistance = 20.0f;
        private float _farDistance = 60.0f;
        private float _maxLeftTurnAngle = 315.0f;
        private float _maxRightTurnAngle = 45.0f;
        private float _startChasingTime = 3.0f;
        private float _chasingTime;
        private float _rotationSpeed = 0.5f;
        private float _dropLootForce = 1000.0f;
        private float _friendlyBotSpeedIncrease = 10.0f;        

        //private static readonly int _speed = Animator.StringToHash("Speed");

        private readonly int MaxRandomNumber = 11; //min 2;

        private Vector3 _point;
        private Vector3 _targetsLastSpot;
        private Vector3 _offsetForText = new Vector3(0.0f, 1.0f, 0.0f);

        private ITimeRemaining _timeRemaining;
        private ITimeRemaining _deathTimeRemaining;

        [SerializeField] private BotTeams _typeOfBot;
        [SerializeField] private StateBot _stateOfBot;

        public GameObject SelfGameObject => gameObject;

        public StateBot StateOfBot
        {
            get => _stateOfBot;
            set
            {
                _stateOfBot = value;
            }
        }

        #endregion


        #region Teams of bot

        public BotTeams TypeOfBot
        {
            get => _typeOfBot;
            set
            {
                _typeOfBot = value;
                switch (value)
                {
                    case BotTeams.Friendly:
                        ColoredMesh = Color.green;                        
                        break;
                    case BotTeams.TeamOne:
                        ColoredMesh = Color.red;
                        break;
                    case BotTeams.TeamTwo:
                        ColoredMesh = Color.blue;
                        break;
                    case BotTeams.TeamThree:
                        ColoredMesh = Color.yellow;
                        break;
                    case BotTeams.TeamFour:
                        ColoredMesh = Color.cyan;
                        break;
                    default:
                        ColoredMesh = Color.grey;
                        break;
                }
            }
        }        

        #endregion


        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();            
            _eye = GetComponentInChildren<TheEye>();
            _engine = GetComponentInChildren<ObjectRotation>();
            _timeRemaining = new TimeRemaining(ResetStateBot, _waitTime);
            _deathTimeRemaining = new TimeRemaining(DestroyBot, _deathTime);            
            Weapon = GetComponentInChildren<BotPairedWeapon>();            
            MenuCanvas = FindObjectOfType<MenuPause>();            
        }

        private void Start()
        {
            Weapon.GetPool(_objectPooler);
        }

        private void OnEnable()
        {           
            var bodyBot = GetComponentInChildren<BodyBot>();
            if (bodyBot != null) bodyBot.OnApplyDamageChange += SetDamage;

            var headBot = GetComponentInChildren<HeadBot>();
            if (headBot != null) headBot.OnApplyDamageChange += SetDamage;
        }

        private void OnDisable()
        {       
            var bodyBot = GetComponent<BodyBot>();
            if (bodyBot != null) bodyBot.OnApplyDamageChange -= SetDamage;

            var headBot = GetComponentInChildren<HeadBot>();
            if (headBot != null) headBot.OnApplyDamageChange -= SetDamage;
        }

        #endregion


        #region Methods

        public void Tick()
        {
            if (StateOfBot == StateBot.Died) return;

            LookRotation();
            //Moving();

            if (StateOfBot != StateBot.Detected)
            {
                if (!Agent.hasPath)
                {
                    if (StateOfBot != StateBot.Inspection)
                    {
                        if (StateOfBot != StateBot.Patrol)
                        {
                            StateOfBot = StateBot.Patrol;
                            _point = Patrol.GenericPoint(transform);
                            MovePoint(_point);
                            Agent.stoppingDistance = 0;
                        }
                        else
                        {
                            if ((_point - transform.position).sqrMagnitude <= _stoppingDistance * _stoppingDistance)
                            {
                                StateOfBot = StateBot.Inspection;
                                _timeRemaining.AddTimeRemaining();
                            }
                        }
                    }
                }

                if (CanSeeEnemy())
                {
                    StateOfBot = StateBot.Detected;
                }

            }
            else
            {
                if (Target != null && (Vector3.SqrMagnitude(Target.position - transform.position) <=
                    _farDistance * _farDistance))
                {
                    if (CanSeeEnemy()) ShootingTime();
                    Agent.stoppingDistance = _nearDistance;
                    MovePoint(Target.position);

                    if (Target.GetComponentInParent<MainBot>() != null &&
                        Target.GetComponentInParent<MainBot>().StateOfBot == StateBot.Died)
                    {
                        StopChasingTarget();
                    }                    
                }
                else
                {
                    StopChasingTarget();
                }
            }
        }

        //private void Moving()
        //{
        //    if (ObjectAnimator != null)
        //    {
        //        if (Agent.remainingDistance > Agent.stoppingDistance)
        //            ObjectAnimator.SetFloat(_speed, Agent.velocity.magnitude, 0.1f, Time.deltaTime);
        //        else ObjectAnimator.SetFloat(_speed, Vector3.zero.z, 0.1f, Time.deltaTime);
        //    }
        //}

        public void ResetStateBot()
        {
            StateOfBot = StateBot.None;
        }

        private void SetDamage(InfoCollision info)
        {
            if (Hp > 0)
            {
                Hp -= info.Damage;
                Target = info.ObjCollision.transform;
                if (FloatingText != null) ShowFloatingText();
                if (TypeOfBot == BotTeams.Friendly) MenuCanvas.FriendlyBotMenu.SetHealth(Hp);
            }            

            if (Hp <= 0 && _stateOfBot != StateBot.Died)
            {               
                EffectsPlay(true);                

                if (TypeOfBot == BotTeams.Friendly)
                {
                    MenuCanvas.FriendlyBotMenu.gameObject.SetActive(false);
                    ServiceLocator.Resolve<BotController>().FriendlyBot = null;
                }

                //if (ObjectAnimator == null)
                //{
                //    foreach (var child in GetComponentsInChildren<Transform>())
                //    {
                //        child.parent = null;

                //        var tempRbChild = child.GetComponent<Rigidbody>();
                //        if (!tempRbChild)
                //        {
                //            tempRbChild = child.gameObject.AddComponent<Rigidbody>();
                //        }
                //        tempRbChild.AddForce(info.Dir * UnityEngine.Random.Range(1, 5));

                //        Destroy(child.gameObject, 4);
                //    }
                //}
                //else
                //{
                //    Destroy(transform.gameObject, 1);
                //}
                
                _deathTimeRemaining.AddTimeRemaining();
                OnDieChange?.Invoke(this);

                if (info.ObjCollision.GetComponentInChildren<BaseCar>())
                {
                    MenuCanvas.KillsCounter();
                }

                DropLoot();

                _stateOfBot = StateBot.Died;
            }
        }

        private bool CanSeeEnemy()
        {
            bool val = false;

            if (Target != null)
            {
                val = true;
                _chasingTime = _startChasingTime;
            }

            else
            {
                val = false;
            }

            return val;
        }

        private void StopChasingTarget()
        {
            if (Target != null) _targetsLastSpot = Target.position;

            MovePoint(_targetsLastSpot);

            if (_chasingTime <= 0)
            {
                Target = null;

                Agent.stoppingDistance = _nearDistance;

                if ((Vector3.SqrMagnitude(_targetsLastSpot - transform.position) <= _nearDistance * _nearDistance) && Target == null)
                {
                    Agent.ResetPath();
                    StateOfBot = StateBot.Inspection;
                    _timeRemaining.AddTimeRemaining();
                }

            }
            else
            {
                _chasingTime -= Time.deltaTime;
            }
        }

        private void LookRotation()
        {
            if (StateOfBot == StateBot.Detected && Target != null)
            {
                Vector3 direction = (Target.transform.position - transform.position).normalized;
                Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                Quaternion LookOnY = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, LookOnY, Time.deltaTime * 5.0f);
                _eye.transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * 5.0f);
            }

            else
            {
                var eulers = Vector3.zero;

                if (_eye.transform.localEulerAngles.y < _maxLeftTurnAngle &&
                    _eye.transform.localEulerAngles.y > _maxRightTurnAngle)
                {
                    _rotationSpeed *= -1.0f;
                }

                float deltaY = _rotationSpeed;
                eulers.y += deltaY;
                _eye.transform.Rotate(eulers);
            }
        }

        private void ShootingTime()
        {
            if (Weapon.IsReady) Weapon.Fire();
            else Weapon.AddAmmo(AmmunitionType.None);
        }

        private void ShowFloatingText()
        {
            GameObject tempText = Instantiate(FloatingText, transform.position + _offsetForText, Quaternion.LookRotation(-Camera.main.transform.forward), transform);
            tempText.GetComponent<TextMesh>().text = Hp.ToString();
        }

        public void MovePoint(Vector3 point)
        {
            Agent.SetDestination(point);
        }

        public string GetMessage()
        {
            return Hp.ToString();
        }

        public void DropLoot()
        {
            int random = UnityEngine.Random.Range(0, MaxRandomNumber);
            
            MainBot tempFriend = ServiceLocator.Resolve<BotController>().FriendlyBot;

            switch (random)
            {
                case 0:
                    if (tempFriend == null)
                    {
                        MainBot friendlyBot = ServiceLocator.Resolve<BotController>().DropBot(Transform, BotTeams.Friendly, true);
                        MenuCanvas.FriendlyBotMenu.gameObject.SetActive(true);
                        MenuCanvas.FriendlyBotMenu.SetMaxHealth(friendlyBot.Hp);
                        friendlyBot.Agent.speed = _friendlyBotSpeedIncrease;                        
                        ServiceLocator.Resolve<BotController>().GetFriendlyBot(friendlyBot);
                    }
                    break;
                case 1:
                case 2:
                case 3:
                    AmmoPack Ammo = Instantiate(_packOfAmmo, _engine.transform.position, Quaternion.identity);
                    Ammo.Rigidbody.AddForce(_engine.transform.position + (UnityEngine.Random.insideUnitSphere * _dropLootForce), ForceMode.Impulse);                    
                    break;
                case 4:
                case 5:
                case 6:
                    RepairBox Repair = Instantiate(_repairBox, _engine.transform.position, Quaternion.identity);
                    Repair.Rigidbody.AddForce(_engine.transform.position + (UnityEngine.Random.insideUnitSphere * _dropLootForce), ForceMode.Impulse);                    
                    break;
                default:
                    break;
            }
        }

        public void OnObjectSpawn()
        {            
            EffectsPlay(false);
            ResetStateBot();
        }

        private void EffectsPlay(bool IsDead)
        {
            Agent.enabled = !IsDead;            

            var rigidBody = gameObject.GetComponent<Rigidbody>();
            rigidBody.isKinematic = !IsDead;
            rigidBody.useGravity = IsDead;

            if (IsDead)
            {
                Target = null;
                ServiceLocator.Resolve<RotationController>().RemoveRotatedObj(_engine);
                ParticleHelper death = Instantiate(_selfExplosion, _engine.transform.position, Quaternion.identity);
                death.SelfDestroy(4);
            }
            else
            {
                Hp = MaximumHp;
                ServiceLocator.Resolve<RotationController>().AddRotatedObj(_engine);
            }
        }

        private void DestroyBot()
        {
            PoolObjects.Instance.ReturnToPool(UnitSwitcher.FlyingBot, this);
        }
    }

    #endregion
}

