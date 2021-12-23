using UnityEngine;
using UnityEngine.UI;


namespace MaksK_Race
{
    public sealed class SpeedometerUI : MonoBehaviour
    {
        private GameObject _needle;        

        private float _startPosition = 182.0f;
        private float _endPosition = -60.0f;
        private float _desiredPosition;

        public float VehicleSpeed;

        private void Awake()
        {
            _needle = GetComponent<Image>().gameObject;
        }

        public void UpdateNeedle(float speedValue)
        {
            VehicleSpeed = speedValue;
            _desiredPosition = _startPosition - _endPosition;
            float temp = VehicleSpeed / 90;
            _needle.transform.eulerAngles = new Vector3(0, 0, (_startPosition - temp * _desiredPosition));
        }
    }
}
