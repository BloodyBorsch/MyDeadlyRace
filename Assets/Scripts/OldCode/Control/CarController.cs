using UnityEngine;


namespace Old_Code
{
    public sealed class CarController : BaseController, IExecute, IFixedExecute
    {
        public BaseCar Car;

        public CarController(BaseCar car)
        {
            Car = car;
        }

        public void Execute()
        {
            if (!IsActive) { return; }

            if (Car != null)
            {
                Car.Ride();
                UiInterface.SpeedUI.UpdateNeedle(Car.Speed);
            }            
        }

        public void FixedExecute()
        {
            Car.FixedRide();
        }
    }
}