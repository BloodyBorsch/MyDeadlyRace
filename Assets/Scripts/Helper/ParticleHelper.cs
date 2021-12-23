using UnityEngine;


namespace MaksK_Race
{
    public sealed class ParticleHelper : MonoBehaviour, IPooledObject
    {
        private ParticleSystem[] _particles;
        private UnitSwitcher _unit;        

        private const string _backToPool = "BackToPool";

        private bool _freezeRotation = false;        

        public bool FreezeRotation { get => _freezeRotation; set => _freezeRotation = value; }

        public GameObject SelfGameObject => gameObject;
        public PoolObjects ObjectPooler;

        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (FreezeRotation) transform.rotation = Quaternion.identity;
        }

        public void SelfDestroy(float timer, UnitSwitcher unit = default, bool PoolExist = false)
        {            
            _unit = unit;            

            if (PoolExist)
            {                
                Invoke(_backToPool, timer);                
            }
            else Destroy(gameObject, timer);
        }

        public void Play()
        {
            foreach (var part in _particles)
            {
                part.Play();
            }
        }

        public void OnObjectSpawn()
        {
            Play();
        }

        public void GetPool(PoolObjects pool)
        {
            ObjectPooler = pool;
        }                

        private void BackToPool()
        {
            ObjectPooler.ReturnToPool(_unit, this);
        }
    }
}
