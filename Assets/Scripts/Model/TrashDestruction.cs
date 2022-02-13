using UnityEngine;


namespace Old_Code
{
    public sealed class TrashDestruction : BaseObjectScene, ICollision
    {
        public void OnCollision(InfoCollision info)
        {
            foreach (var child in GetComponentsInChildren<Transform>())
            {
                child.parent = null;

                var tempRbChild = child.GetComponent<Rigidbody>();
                var tempFJChild = child.GetComponent<FixedJoint>();

                if (!tempRbChild) tempRbChild = child.gameObject.AddComponent<Rigidbody>();

                if (tempFJChild) Destroy(child.gameObject.GetComponent<FixedJoint>());

                tempRbChild.AddForce(child.up * Random.Range(20, 30), ForceMode.Impulse);

                //Destroy(child.gameObject, 4);
            }
        }
    }
}
