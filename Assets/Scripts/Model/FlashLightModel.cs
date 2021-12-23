using System;
using UnityEngine;
using static UnityEngine.Random;


namespace MaksK_Race
{
    public sealed class FlashLightModel : BaseObjectScene
    {
        #region Properties

        [SerializeField] private float _speed = 15.0f;
        [SerializeField] private float _intensity = 1.5f;

        private float _share;
        private float _takeAwayTheIntensity;

        private Light _light;        

        public float BatteryChargeCurrent { get; private set; }
        public float BatteryChargeMax = 10.0f;
        public float Charge => BatteryChargeCurrent / BatteryChargeMax;

        #endregion


        #region Methods

        protected override void Awake()
        {
            base.Awake();
            _light = GetComponent<Light>();
            _light.enabled = false;            
            BatteryChargeCurrent = BatteryChargeMax;
            _light.intensity = _intensity;
            _share = BatteryChargeMax / 4.0f;
            _takeAwayTheIntensity = _intensity / (BatteryChargeMax * 200.0f);
        }

        public void Switch(FlashLightActiveType value)
        {
            switch (value)
            {
                case FlashLightActiveType.On:
                    _light.enabled = true;                    
                    break;
                case FlashLightActiveType.None:
                    break;
                case FlashLightActiveType.Off:
                    _light.enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public bool EditBatteryCharge(bool Activated)
        {
            if (BatteryChargeCurrent > 0 && Activated)
            {
                BatteryChargeCurrent -= Time.deltaTime;

                if (BatteryChargeCurrent < _share)
                {
                    _light.enabled = Range(0, 100) >= Range(0, 10);
                }
                else
                {
                    _light.intensity -= _takeAwayTheIntensity;
                }
                return true;
            }

            if (BatteryChargeCurrent < BatteryChargeMax && !Activated)
            {
                BatteryChargeCurrent += Time.deltaTime;
                _light.intensity = _intensity;
                return true;
            }

            return false;
        }

        public bool LowBattery()
        {
            return BatteryChargeCurrent <= BatteryChargeMax * 0.5f;
        }

        #endregion
    }
}
