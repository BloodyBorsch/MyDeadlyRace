using UnityEngine;


namespace MaksK_Race
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "SObject/Weapon")]
    public class BaseWeaponSObject : ScriptableObject
    {
        public float RapidOfFire;
        public float Damage;
        public float EffectiveDistance;
        public float RotationSpeed;
        public float PushForce;

        public GameObject HitEffect;
        public GameObject Trail;
    }
}
