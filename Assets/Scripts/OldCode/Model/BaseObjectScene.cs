using UnityEngine;
using UnityEngine.AI;


namespace Old_Code
{
    public abstract class BaseObjectScene : MonoBehaviour
    {
        #region Properties

        protected int _layer;

        protected Color _color;
        protected Color _coloredMesh;
        protected Material _material;
        protected Material _meshMaterial;
        protected PoolObjects _objectPooler;
        protected Vector3 _scale;
        protected RaycastHit hit;

        protected string _name;

        protected bool _isVisible;

        [HideInInspector] public GameObject InstanceObject;
        [HideInInspector] public Rigidbody Rigidbody;
        [HideInInspector] public Transform Transform;
        [HideInInspector] public NavMeshAgent Agent;
        [HideInInspector] public Animator ObjectAnimator;
        [HideInInspector] public Camera Camera;

        [SerializeField] private Transform _MeshForPainting;

        public int Layer
        {
            get => _layer;
            set
            {
                _layer = value;

                AskLayer(Transform, _layer);
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                if (Transform != null)
                {
                    if (_material != null)
                    {
                        _material.color = _color;
                    }

                    AskColor(Transform, _color);
                }
            }
        }

        public Color ColoredMesh
        {
            get => _coloredMesh;
            set
            {
                _coloredMesh = value;
                if (_MeshForPainting != null)
                {
                    if (_meshMaterial != null)
                    {
                        _meshMaterial.color = _coloredMesh;
                    }

                    AskColor(_MeshForPainting, _coloredMesh);
                }
            }
        }

        public Material GetMaterial
        {
            get => _material;
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                RendererSetActive(transform);
                if (transform.childCount <= 0) return;
                foreach (Transform t in transform)
                {
                    RendererSetActive(t);
                }
            }
        }

        public Vector3 Scale
        {
            get
            {
                if (InstanceObject != null)
                {
                    _scale = Transform.localScale;
                }

                return _scale;
            }

            set
            {
                _scale = value;

                if (InstanceObject != null)
                {
                    Transform.localScale = _scale;
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                InstanceObject.name = _name;
            }
        }

        #endregion


        #region UnityMethods

        protected virtual void Awake()
        {
            InstanceObject = gameObject;
            Camera = Camera.main;

            if (InstanceObject.GetComponent<MeshRenderer>())
            {
                _material = InstanceObject.GetComponent<MeshRenderer>().material;
            }

            if (_MeshForPainting != null)
            {
                if (_MeshForPainting.GetComponent<MeshRenderer>())
                {
                    _meshMaterial = _MeshForPainting.GetComponent<MeshRenderer>().material;
                }
            }

            if (InstanceObject.GetComponent<Rigidbody>())
            {
                Rigidbody = InstanceObject.GetComponent<Rigidbody>();
            }

            Transform = InstanceObject.transform;

            if (InstanceObject.GetComponent<NavMeshAgent>())
            {
                Agent = InstanceObject.GetComponent<NavMeshAgent>();
            }

            if (InstanceObject.GetComponent<Animator>() != null)
            {
                ObjectAnimator = InstanceObject.GetComponent<Animator>();
            }
        }

        #endregion


        #region Methods

        private void AskLayer(Transform obj, int layer)
        {
            obj.gameObject.layer = layer;
            if (obj.childCount <= 0) return;

            foreach (Transform child in obj)
            {
                AskLayer(child, layer);
            }
        }

        private void RendererSetActive(Transform renderer)
        {
            if (renderer.gameObject.TryGetComponent<Renderer>(out var component))
            {
                component.enabled = _isVisible;
            }
        }

        private void AskColor(Transform obj, Color color)
        {
            if (obj.gameObject.GetComponent<MeshRenderer>() != null)
            {
                obj.gameObject.GetComponent<MeshRenderer>().material.color = color;
            }
            else return;

            //if (obj.childCount <= 0) return;

            //foreach (Transform child in obj)
            //{
            //    AskColor(child, color);
            //}
        }

        public void DisableRigidBody()
        {
            var rigidbodies = GetComponentsInChildren<Rigidbody>();

            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = true;
            }
        }

        public void EnableRigidBody(float force)
        {
            EnableRigidBody();

            Rigidbody.AddForce(transform.forward * force);
        }

        public void EnableRigidBody()
        {
            var rigidbodies = GetComponentsInChildren<Rigidbody>();

            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = false;
            }
        }

        public void ConstraintsRigidBody(RigidbodyConstraints rigidbodyConstraints)
        {
            var rigidbodies = GetComponentsInChildren<Rigidbody>();

            foreach (var rb in rigidbodies)
            {
                rb.constraints = rigidbodyConstraints;
            }
        }

        public void SetActive(bool value)
        {
            IsVisible = value;

            if (TryGetComponent<Collider>(out var component))
            {
                component.enabled = value;
            }
        }

        public void GetPool(PoolObjects pool)
        {
            _objectPooler = pool;
        }

        #endregion
    }
}
