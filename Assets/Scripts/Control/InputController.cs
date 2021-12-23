using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;


namespace MaksK_Race
{
    public sealed class InputController : BaseController, IExecute, IInitialization
    {
        #region Properties       

        private KeyCode _activeFlashLight = KeyCode.F;
        //private KeyCode _TakeAndDrag = KeyCode.E;
        private KeyCode _cancel = KeyCode.Escape;
        private KeyCode _reloadClip = KeyCode.R;
        //private KeyCode _savePlayer = KeyCode.C;
        //private KeyCode _loadPlayer = KeyCode.V;
        //private KeyCode _screenshot = KeyCode.Q;
        private KeyCode _returnOnWheels = KeyCode.G;
        private KeyCode _breakes = KeyCode.Space;

        private int _leftMB = (int)MouseButton.LeftButton;
        private int _rightMB = (int)MouseButton.RightButton;
        private int _selectedWeapon = 0;

        private string _inputSteerAxis = "Horizontal";
        private string _inputThrottleAxis = "Vertical";        

        public float ThrottleInput { get; private set; }
        public float SteerInput { get; private set; }

        private ParticleHelper _botWayMarker;

        public InputController()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        #endregion


        #region Methods    

        public void Initialization()
        {
            _botWayMarker = Object.Instantiate(ServiceLocatorMonoBehaviour.GetService<GameController>().Marker,
                ServiceLocatorMonoBehaviour.GetService<GameController>().transform.position, 
                Quaternion.identity);
        }
        
        public void Execute()
        {            
            if (!IsActive) return;

            SteerInput = Input.GetAxis(_inputSteerAxis);
            ThrottleInput = Input.GetAxis(_inputThrottleAxis);

            if (Input.GetKey(_breakes))
            {
                ServiceLocator.Resolve<CarController>().Car.BrakesActivation(true);
            }
            else ServiceLocator.Resolve<CarController>().Car.BrakesActivation(false);

            if (Input.GetKeyDown(_returnOnWheels))
            {
                ServiceLocator.Resolve<CarController>().Car.ReturnOnWheels();
            }

            if (Input.GetKeyDown(_activeFlashLight))
            {
                ServiceLocator.Resolve<FlashLightController>().Switch(ServiceLocator.Resolve<Inventory>().FlashLight);
            }

            if (Input.GetMouseButton(_leftMB))
            {
                if (ServiceLocator.Resolve<WeaponController>().IsActive)
                {
                    ServiceLocator.Resolve<WeaponController>().Fire();
                }
            }

            if (Input.GetMouseButtonDown(_rightMB))
            {
                MainBot allyBot = ServiceLocator.Resolve<BotController>().FriendlyBot;                
                LayerMask mask = LayerMask.GetMask("Bot", "Solid");
                float range = 1000.0f;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, range, mask))
                {
                    MoveMarker(hit.point);                    

                    if (allyBot != null)
                    {
                        BodyBot enemyBot = hit.collider.GetComponent<BodyBot>();

                        allyBot.ResetStateBot();                        
                        allyBot.Target = null;

                        if (enemyBot != null)
                        {
                            allyBot.Target = enemyBot.transform;
                        }                        
                        else
                        {
                            allyBot.MovePoint(hit.point);
                        }
                    }                    
                }
            }

            if (Input.GetKeyDown(_reloadClip))
            {
                if (ServiceLocator.Resolve<WeaponController>().IsActive)
                {
                    ServiceLocator.Resolve<WeaponController>().ReloadClip();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectWeapon(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && ServiceLocator.Resolve<Inventory>().Weapons.Count > 1)
            {
                SelectWeapon(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && ServiceLocator.Resolve<Inventory>().Weapons.Count > 2)
            {
                SelectWeapon(2);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            {
                if (!ServiceLocator.Resolve<WeaponController>().IsActive ||
                    _selectedWeapon >= ServiceLocator.Resolve<Inventory>().Weapons.Count - 1)
                {
                    _selectedWeapon = 0;
                }
                else
                {
                    _selectedWeapon++;
                }

                SelectWeapon(_selectedWeapon);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            {
                if (!ServiceLocator.Resolve<WeaponController>().IsActive ||
                    _selectedWeapon <= 0)
                {
                    _selectedWeapon = ServiceLocator.Resolve<Inventory>().Weapons.Count - 1;
                }
                else
                {
                    _selectedWeapon--;
                }

                SelectWeapon(_selectedWeapon);
            }

            //if (ServiceLocator.Resolve<SelectionController>().IsActive)
            //{
            //    if (Input.GetKey(_TakeAndDrag))
            //        ServiceLocator.Resolve<SelectionController>()._canBeDragged = true;

            //    if (Input.GetKeyUp(_TakeAndDrag))
            //        ServiceLocator.Resolve<SelectionController>()._canBeDragged = false;
            //}

            //if (Input.GetKeyDown(_savePlayer))
            //{
            //    ServiceLocator.Resolve<SaveDataRepository>().Save();
            //}

            //if (Input.GetKeyDown(_loadPlayer))
            //{
            //    ServiceLocator.Resolve<SaveDataRepository>().Load();
            //}            
        }

        public void MoveMarker(Vector3 point)
        {
            _botWayMarker.transform.position = point;
            _botWayMarker.Play();
        }

        private void SelectWeapon(int i)
        {
            ServiceLocator.Resolve<WeaponController>().Off();

            var tempWeapon = ServiceLocator.Resolve<Inventory>().SelectWeapon(i);

            if (tempWeapon != null)
            {
                ServiceLocator.Resolve<WeaponController>().On(tempWeapon);
            }
        }

        #endregion
    }
}
