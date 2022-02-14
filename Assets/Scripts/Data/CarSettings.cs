using System;
using UnityEngine;


namespace MyRacing
{
    [Serializable]
    public sealed class CarSettings
    {
        public CarType Type;

        public float MotorTorque;
        public float FirstGear;
        public float SecondGear;
        public float ThirdGear;
        public float BackGear;

        public CarType AskTypeOfData()
        {
            return Type;
        }
    }
}