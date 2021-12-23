using UnityEngine;


namespace MaksK_Race
{
    public class MiniMap : MonoBehaviour
    {
        private Transform _player;
        private void Start()
        {            
            _player = FindObjectOfType<BaseCar>().Transform;
            transform.parent = null;

            var rt = Resources.Load<RenderTexture>("Minimap");

            GetComponent<Camera>().targetTexture = rt;
        }

        private void LateUpdate()
        {
            var newPosition = _player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(90, _player.eulerAngles.y, 0);
        }
    }
}
