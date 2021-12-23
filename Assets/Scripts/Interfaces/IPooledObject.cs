using UnityEngine;

namespace MaksK_Race
{
    public interface IPooledObject
    {
        GameObject SelfGameObject { get; }
        void OnObjectSpawn();
        void GetPool(PoolObjects pool);
    }
}
