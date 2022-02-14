using UnityEngine;

namespace Old_Code
{
    public interface IPooledObject
    {
        GameObject SelfGameObject { get; }
        void OnObjectSpawn();
        void GetPool(PoolObjects pool);
    }
}
