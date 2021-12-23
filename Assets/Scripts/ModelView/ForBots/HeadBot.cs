using System;
using UnityEngine;


namespace MaksK_Race
{
    public sealed class HeadBot : MonoBehaviour, ICollision
    {
        public event Action<InfoCollision> OnApplyDamageChange;
        public void OnCollision(InfoCollision info)
        {
            OnApplyDamageChange?.Invoke(new InfoCollision(info.Damage * 3, info.ObjCollision, info.Dir));
        }
    }
}
