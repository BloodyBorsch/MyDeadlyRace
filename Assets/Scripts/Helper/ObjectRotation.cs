using UnityEngine;


namespace Old_Code
{
    public sealed class ObjectRotation : BaseObjectScene
    {
        [SerializeField] private float _speed;

        [SerializeField] private bool _aroundX;
        [SerializeField] private bool _aroundY;
        [SerializeField] private bool _aroundZ;        

        public void RotationOfThis()
        {
            if (_aroundX) Transform.Rotate(_speed * Time.deltaTime, 0, 0);
            if (_aroundY) Transform.Rotate(0, _speed * Time.deltaTime, 0);
            if (_aroundZ) Transform.Rotate(0, 0, _speed * Time.deltaTime);
        }

    }
}
