using System.Collections.Generic;
using UnityEngine;


namespace Old_Code
{
    public sealed class Controllers : IInitialization
    {       
        private readonly IExecute[] _executeControllers;

        private List<Controllers> _allControllers = new List<Controllers>();
        private List<IFixedExecute> fixedExecutes;        
        internal List<IFixedExecute> FixedExecutes { get => fixedExecutes; set => fixedExecutes = value; }
                
        public List<Controllers> AllControllers => _allControllers;
        public int ExecuteLength => _executeControllers.Length;       
        public IExecute this[int index] => _executeControllers[index];        

        public Controllers(BaseCar car, Transform gameControllerPos, PoolObjects pool = null)
        {
            ServiceLocator.SetService(new TimeRemainingController());
            ServiceLocator.SetService(new Inventory(pool));
            ServiceLocator.SetService(new CarController(car));
            ServiceLocator.SetService(new WeaponController());
            ServiceLocator.SetService(new FlashLightController());            
            ServiceLocator.SetService(new InputController());
            ServiceLocator.SetService(new RotationController());
            //ServiceLocator.SetService(new SelectionController());
            ServiceLocator.SetService(new BotController(car, gameControllerPos, pool));            
            //ServiceLocator.SetService(new SaveDataRepository());

            _executeControllers = new IExecute[6];

            _executeControllers[0] = ServiceLocator.Resolve<TimeRemainingController>();

            _executeControllers[1] = ServiceLocator.Resolve<CarController>();

            _executeControllers[2] = ServiceLocator.Resolve<InputController>();            

            _executeControllers[3] = ServiceLocator.Resolve<FlashLightController>();

            _executeControllers[4] = ServiceLocator.Resolve<BotController>();

            _executeControllers[5] = ServiceLocator.Resolve<RotationController>();

            //_executeControllers[6] = ServiceLocator.Resolve<SelectionController>();

            FixedExecutes = new List<IFixedExecute>
            {
                ServiceLocator.Resolve<CarController>()
            };                        
        }

        public void Initialization()
        {
            foreach (var controller in _executeControllers)
            {
                if (controller is IInitialization initialization)
                {
                    initialization.Initialization();
                }
            }            

            ServiceLocator.Resolve<InputController>().On();            
            ServiceLocator.Resolve<Inventory>().Initialization();
            //ServiceLocator.Resolve<SelectionController>().On();
            ServiceLocator.Resolve<CarController>().On();
            ServiceLocator.Resolve<BotController>().On();
            ServiceLocator.Resolve<RotationController>().On();
        }
    }
}
