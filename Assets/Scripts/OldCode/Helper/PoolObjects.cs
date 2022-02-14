using UnityEngine;
using System.Collections.Generic;
using System;


namespace Old_Code
{
    public class PoolObjects : MonoBehaviour
    {        
        public Dictionary<UnitSwitcher, Queue<IPooledObject>> PoolDictionary;

        public List<Pool> Pools;

        public static PoolObjects Instance;

        private Transform _poolBots;
        private Transform _poolVFX;

        [System.Serializable]
        public class Pool
        {
            public UnitSwitcher UnitType;
            public int Count;
        }

        private void Awake()
        {
            Instance = this;
        }

        public void FirstStarting()
        {
            CreatePool();
        }        

        public IPooledObject SpawnFromPool(UnitSwitcher type, Vector3 position, Quaternion rotation)
        {
            if (!PoolDictionary.ContainsKey(type))
            {
                Debug.LogWarning($"Нету пула с тагом {type}");
                return null;
            }
            
            IPooledObject objectToSpawn = PoolDictionary[type].Dequeue();

            objectToSpawn.SelfGameObject.SetActive(true);
            objectToSpawn.SelfGameObject.transform.position = position;
            objectToSpawn.SelfGameObject.transform.rotation = rotation;            
            objectToSpawn.OnObjectSpawn();            

            PoolDictionary[type].Enqueue(objectToSpawn);

            return objectToSpawn;
        }          

        public void ReturnToPool(UnitSwitcher type, IPooledObject obj)
        {
            PoolDictionary[type].Enqueue(obj);            
            obj.SelfGameObject.SetActive(false);
            obj.SelfGameObject.transform.position = Vector3.zero;
        }                

        private void CreatePool()
        {
            _poolBots = new GameObject(NameManager.Pool_Bots).transform;
            _poolVFX = new GameObject(NameManager.Pool_VFX).transform;

            PoolDictionary = new Dictionary<UnitSwitcher, Queue<IPooledObject>>();
                        
            foreach (Pool pool in Pools)
            {
                Queue<IPooledObject> MainPool = new Queue<IPooledObject>();                

                switch (pool.UnitType)
                {
                    case UnitSwitcher.FlyingBot:
                        for (int i = 0; i < pool.Count; i++)
                        {
                            MainBot tempBot = Instantiate(BotFactory.CreateBot(pool.UnitType), _poolBots);
                            SetValues(MainPool, tempBot);
                        }
                        break;
                    case UnitSwitcher.VFXTrail:
                    case UnitSwitcher.VFXHit:
                    case UnitSwitcher.VFXHitBot:       
                        for (int i = 0; i < pool.Count; i++)
                        {
                            string resourceLoader = ResourceLoadHelper.Loader(pool.UnitType);
                            ParticleHelper tempVFX = Instantiate(Resources.Load<ParticleHelper>(resourceLoader), _poolVFX);
                            SetValues(MainPool, tempVFX);
                        }                        
                        break;
                    default:
                        throw new ArgumentException("Нет такого пула");
                }                

                PoolDictionary.Add(pool.UnitType, MainPool);
            }                   
        } 
        
        private void SetValues(Queue<IPooledObject> Pool, IPooledObject obj)
        {
            obj.SelfGameObject.SetActive(false);
            obj.GetPool(this);
            Pool.Enqueue(obj);
        }
    }
}
