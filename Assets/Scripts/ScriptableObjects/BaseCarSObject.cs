using UnityEngine;


namespace MaksK_Race
{
    [CreateAssetMenu(fileName = "New Car", menuName = "SObject/Car")]
    public class BaseCarSObject : ScriptableObject
    {
        public float MotorTorque;
        public float FirstGear;
        public float SecondGear;
        public float ThirdGear;
        public float BackGear;
    }
}
