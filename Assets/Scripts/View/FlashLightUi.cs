using UnityEngine;
using UnityEngine.UI;


namespace MaksK_Race
{
    public sealed class FlashLightUi : MonoBehaviour
    {
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        public void SetActive(bool value)
        {
            _slider.gameObject.SetActive(value);
        }

        public void SetMaxEnergy(float energy)
        {
            _slider.maxValue = energy;
            _slider.value = energy;
        }

        public void SetEnergy(float energy)
        {
            _slider.value = energy;
        }
    }
}
