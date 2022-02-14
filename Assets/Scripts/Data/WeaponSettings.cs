using System;
using UnityEngine;


namespace MyRacing
{ 
    [Serializable]
    public sealed class WeaponSettings
    {
        public WeaponType Type;

        public float RapidOfFire;
        public float Damage;
        public float EffectiveDistance;
        public float RotationSpeed;
        public float PushForce;

        public GameObject HitEffect;
        public GameObject Trail;

        public WeaponType AskTypeOfData()
        {
            return Type;
        }
    }
}