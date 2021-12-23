using UnityEngine;


namespace MaksK_Race
{
    public sealed class GameController : MonoBehaviour
    {
        private Controllers _controllers;
        private PoolObjects _objectPooler;

        public ParticleHelper Marker;

        public BaseCar Car;        
        public Transform Player { get; private set; }
        public Camera MainCamera { get; private set; }
        public static GameController Instance { get; private set; }

        public int AmountOfBots;
        public int TeamsCount; // не менее 2, не более 4

        public float SpawnTimer;

        public bool IsPaused;
        public bool SpawnRepeat;

        private void Awake()
        {
            Instance = this;
            MainCamera = Camera.main;                       
        }

        private void Start()
        {
            FirstStarting();
        }

        private void Update()
        {
            if (!IsPaused)
            {
                for (var i = 0; i < _controllers.ExecuteLength; i++)
                {
                    _controllers[i].Execute();
                }
            }            
        }

        private void FixedUpdate()
        {
            if (!IsPaused)
            {
                foreach (var fixedCon in _controllers.FixedExecutes)
                {
                    fixedCon.FixedExecute();
                }
            }
        }

        public void FirstStarting()
        {            
            Car = FindObjectOfType<BaseCar>();

            if (GetComponent<PoolObjects>() != null)
            {
                _objectPooler = GetComponent<PoolObjects>();
                Car.GetPool(_objectPooler);
            }

            Player = FindObjectOfType<BaseCar>().Transform;
            IsPaused = false;
            _controllers = new Controllers(Car, transform, _objectPooler);
            _controllers.Initialization();            
        }

        public void CreatePoolObjects()
        {
            gameObject.AddComponent<PoolObjects>();            
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 100, 100), $"{1 / Time.deltaTime:0.0}");
        }

        public bool PauseToggle(bool pause)
        {
            return IsPaused = pause;
        }
    }
}
