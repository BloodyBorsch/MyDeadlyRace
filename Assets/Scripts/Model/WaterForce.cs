using UnityEngine;


namespace MaksK_Race
{
    public sealed class WaterForce : BaseObjectScene
    {
        [SerializeField] private float _force = 10.0f;

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.attachedRigidbody.AddForce(Vector3.up * _force, ForceMode.Acceleration);
                other.attachedRigidbody.drag = 5.0f;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Rigidbody>() != null)
            {                
                other.attachedRigidbody.drag = 0.05f;
            }
        }
    }
}
