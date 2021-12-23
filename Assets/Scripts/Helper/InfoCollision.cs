using UnityEngine;


namespace MaksK_Race
{
    public readonly struct InfoCollision
    {
        private readonly Vector3 _dir;
        private readonly float _damage;
        //private readonly ContactPoint _contact;
        private readonly Transform _objCollision;        

        //public InfoCollision(float damage, ContactPoint contact, Transform objCollision, Vector3 dir = default)
        public InfoCollision(float damage, Transform objCollision, Vector3 dir = default)
        {
            _damage = damage;
            _dir = dir;
            //_contact = contact;
            _objCollision = objCollision;            
        }

        public Vector3 Dir => _dir;

        public float Damage => _damage;

        //public ContactPoint Contact => _contact;

        public Transform ObjCollision => _objCollision;        
    }
}
