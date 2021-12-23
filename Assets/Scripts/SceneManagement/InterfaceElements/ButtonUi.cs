using UnityEngine;
using UnityEngine.UI;


namespace MaksK_Race
{
    public sealed class ButtonUi : MonoBehaviour, IControlText
    {

        private Text _text;
        private Button _control;

        public Text GetText
        {
            get
            {
                if (!_text)
                {
                    _text = transform.GetComponentInChildren<Text>();
                }
                return _text;
            }
        }
        public Button GetControl
        {
            get
            {
                if (!_control)
                {
                    _control = transform.GetComponentInChildren<Button>();
                }
                return _control;
            }
        }
        public void SetInteractable(bool value)
        {
            GetControl.interactable = value;
        }
        public GameObject Instance => gameObject;
        public Selectable Control => GetControl;
    }
}
