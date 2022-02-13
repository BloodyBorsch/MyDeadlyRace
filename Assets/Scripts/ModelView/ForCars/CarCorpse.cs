using System;
using UnityEngine;


namespace Old_Code
{
    public sealed class CarCorpse : MonoBehaviour, ICollision
    {
        public event Action<InfoCollision> CarDestruction;
        public void OnCollision(InfoCollision info)
        {
            CarDestruction?.Invoke(info);
        }
    }
}
