using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;


namespace MaksK_Race
{
    public sealed class DropDownUI : MonoBehaviour, IControl
    {
        #region Properties

        private Text _text;
        private Dropdown _dropDown;

        public Text GetText => _text;

        public Dropdown GetControl => _dropDown;

        public GameObject Instance => gameObject;
        public Selectable Control => GetControl;

        public Resolution[] resolutions;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _dropDown = transform.GetComponentInChildren<Dropdown>();
            _text = transform.GetComponentInChildren<Text>();

            resolutions = Screen.resolutions;
            _dropDown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            _dropDown.AddOptions(options);
            _dropDown.value = currentResolutionIndex;
            _dropDown.RefreshShownValue();
        }

        #endregion


        #region Methods

        public void Interactable(bool value)
        {
            GetControl.interactable = value;
        }

        #endregion
    }
}
