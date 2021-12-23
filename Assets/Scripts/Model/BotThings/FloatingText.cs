using UnityEngine;


namespace MaksK_Race
{
    public sealed class FloatingText : MonoBehaviour
    {
        private Vector3 _scaleChange;

        private void Awake()
        {
            _scaleChange = new Vector3(-0.02f, -0.02f, -0.02f);
        }

        private void Update()
        {
            transform.localScale += _scaleChange;

            if (transform.localScale.y < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
