using UnityEngine;


namespace Old_Code
{
    public sealed class RepairBox : BaseDropLoot
    {
        private float _repair = 100.0f;     

        private void OnCollisionEnter(Collision collision)
        {
            var getRepair = collision.gameObject.GetComponent<BaseCar>();

            if (getRepair != null)
            {
                getRepair.CarHealth(new InfoCollision(_repair, Transform));
                Destroy(gameObject);
            }
        }
    }
}
