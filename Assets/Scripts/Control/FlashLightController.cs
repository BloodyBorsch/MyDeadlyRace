using UnityEngine;


namespace MaksK_Race
{
    public sealed class FlashLightController : BaseController, IExecute, IInitialization

    {
        private FlashLightModel[] _flashLightModel;
        private FlashLightUi _flashLightUi;

        public void Initialization()
        {
            _flashLightModel = Object.FindObjectsOfType<FlashLightModel>();
            _flashLightUi = Object.FindObjectOfType<FlashLightUi>();
            _flashLightUi.SetMaxEnergy(_flashLightModel[0].BatteryChargeMax);
            _flashLightUi.SetActive(false);
        }

        public override void On(params BaseObjectScene[] flashLight)
        {
            if (IsActive) return;
            //if (flashLight.Length > 0) _flashLightModel[0] = flashLight[0] as FlashLightModel;
            if (_flashLightModel == null) return;
            if (_flashLightModel[0].BatteryChargeCurrent <= 0) return;
            base.On(_flashLightModel);

            foreach (var flash in _flashLightModel)
            {
                flash.Switch(FlashLightActiveType.On);
            }
            
            _flashLightUi.SetActive(true);
        }

        public override void Off()
        {
            if (!IsActive) return;
            base.Off();

            foreach (var flash in _flashLightModel)
            {
                flash.Switch(FlashLightActiveType.Off);
            }            
        }

        public void Execute()
        {
            foreach (var flash in _flashLightModel)
            {
                if (!IsActive && flash.EditBatteryCharge(false))
                {
                    _flashLightUi.SetEnergy(_flashLightModel[0].BatteryChargeCurrent);
                }

                else if (!IsActive && !flash.EditBatteryCharge(false))
                {
                    _flashLightUi.SetActive(false);
                }

                else
                {

                }

                if (IsActive && flash.EditBatteryCharge(true))
                {
                    _flashLightUi.SetEnergy(_flashLightModel[0].BatteryChargeCurrent);
                }

                else
                {
                    Off();
                }
            }            
        }
    }
}
