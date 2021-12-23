using System;
using UnityEngine;


namespace MaksK_Race
{
    public sealed class SelectionController : BaseController, IExecute
    {
        #region Properties

        private readonly Camera _mainCamera;
        private readonly Vector2 _center;
        private readonly float _dedicateDistance = 20.0f;

        private GameObject _dedicatedObj;
        private ISelectObj _selectedObj;

        private bool _nullString;
        private bool _isSelectedObj;

        public bool _canBeDragged = false;

        private float _mouseZCoordinates;

        #endregion


        #region Methods

        public SelectionController()
        {
            _mainCamera = Camera.main;
            _center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        }

        public void Execute()
        {
            if (!IsActive) return;

            if (Physics.Raycast(_mainCamera.ScreenPointToRay(_center),
                out var hit, _dedicateDistance))
            {
                SelectObject(hit.collider.gameObject);

                if (_canBeDragged && _isSelectedObj) DragObject();

                _nullString = false;
            }

            else if (!_nullString)
            {
                //UiInterface.SelectionObjMessageUi.Text = String.Empty;
                _nullString = true;
                _dedicatedObj = null;
                _isSelectedObj = false;
            }

            //if (_isSelectedObj)
            //{
            //    // Действие над объектом

            //    switch (_selectedObj)
            //    {
            //        case Weapon aim:

            //            // в инвентарь


            //            //Inventory.AddWeapon(aim);
            //            break;

            //        case Wall wall:
            //            break;
            //    }
            //}
        }

        private void SelectObject(GameObject obj)
        {
            //if (obj == _dedicatedObj) return;
            _selectedObj = obj.GetComponent<ISelectObj>();

            if (_selectedObj != null)
            {
                //UiInterface.SelectionObjMessageUi.Text = _selectedObj.GetMessage();
                _isSelectedObj = true;
            }

            else
            {
                //UiInterface.SelectionObjMessageUi.Text = String.Empty;
                _isSelectedObj = false;
            }

            _dedicatedObj = obj;
        }

        private void DragObject()
        {
            _mouseZCoordinates = Camera.main.WorldToScreenPoint(_dedicatedObj.transform.position).z;
            _dedicatedObj.transform.position = GetMouseWorldPosition();
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = _mouseZCoordinates;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        #endregion
    }
}
