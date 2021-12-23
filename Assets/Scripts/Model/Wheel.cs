using UnityEngine;


namespace MaksK_Race
{
    public sealed class Wheel : MonoBehaviour
    {
        [SerializeField] private bool _steer;
        [SerializeField] private bool _invertSteer;
        [SerializeField] private bool _power;

        [SerializeField] private float _brakesPower = 200.0f;

        public float SteerAngle { get; set; }
        public float Torque { get; set; }

        public bool Breakes;

        private WheelCollider _wheelCollider;
        private Transform _wheelMesh;

        private void Start()
        {
            _wheelCollider = GetComponentInChildren<WheelCollider>();
            _wheelMesh = GetComponentInChildren<MeshRenderer>().GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            _wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            _wheelMesh.position = pos;
            _wheelMesh.rotation = rot;

            if (_steer)
            {
                _wheelCollider.steerAngle = SteerAngle * (_invertSteer ? -1 : 1);
            }

            if (_power)
            {
                _wheelCollider.motorTorque = Breakes ? 0.0f : Torque;
            }

            _wheelCollider.brakeTorque = Breakes ? _brakesPower : 0.0f;   
        }
    }
}
