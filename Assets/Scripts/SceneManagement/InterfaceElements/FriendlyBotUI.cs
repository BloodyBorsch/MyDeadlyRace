using UnityEngine.UI;
using UnityEngine;


namespace MaksK_Race
{
    public sealed class FriendlyBotUI : BaseObjectScene, IControl
    {
        private Text _text;
        private Slider _control;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _control = GetComponentInChildren<SliderUI>().GetComponent<Slider>();    
            gameObject.SetActive(false);
        }

        public void SetMaxHealth(float health)
        {
            _control.maxValue = health;
            _control.value = health;
        }

        public void SetHealth(float health)
        {
            _control.value = health;
        }

        public Text GetText => _text;
        public Slider GetControl => _control;
        public GameObject Instance => gameObject;
        public Selectable Control => GetControl;
    }
}
