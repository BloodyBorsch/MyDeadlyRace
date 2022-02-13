using UnityEngine;


namespace Old_Code
{
	public sealed class UiInterface
	{
        private SpeedometerUI _speedUi;

        public SpeedometerUI SpeedUI
        {
            get
            {
                if (!_speedUi)
                    _speedUi = Object.FindObjectOfType<SpeedometerUI>();
                return _speedUi;
            }
        }

        private WeaponUiText _weaponUiText;

        public WeaponUiText WeaponUiText
        {
            get
            {
                if (!_weaponUiText)
                    _weaponUiText = Object.FindObjectOfType<WeaponUiText>();
                return _weaponUiText;
            }
        }

        private SelectionObjMessageUi _selectionObjMessageUi;

        public SelectionObjMessageUi SelectionObjMessageUi
        {
            get
            {
                if (!_selectionObjMessageUi)
                    _selectionObjMessageUi = Object.FindObjectOfType<SelectionObjMessageUi>();
                return _selectionObjMessageUi;
            }
        }
    }
}