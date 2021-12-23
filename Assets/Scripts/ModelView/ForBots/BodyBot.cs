using System;
using UnityEngine;


namespace MaksK_Race
{
    public sealed class BodyBot : MonoBehaviour, ICollision
    {
        public event Action<InfoCollision> OnApplyDamageChange;
        public void OnCollision(InfoCollision info)
        {
            OnApplyDamageChange?.Invoke(info);
        }
    }
}
