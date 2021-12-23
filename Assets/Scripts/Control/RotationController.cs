using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MaksK_Race
{
    public sealed class RotationController : BaseController, IExecute, IInitialization
    {
        private ObjectRotation[] _countedRotObjects;
        private readonly HashSet<ObjectRotation> _rotatedObjects = new HashSet<ObjectRotation>();

        public void Initialization()
        {
            _countedRotObjects = Object.FindObjectsOfType<ObjectRotation>();            

            foreach (var obj in _countedRotObjects)
            {
                AddRotatedObj(obj);                
            }            
        }

        public void AddRotatedObj(ObjectRotation engine)
        {            
            if (!_rotatedObjects.Contains(engine))
            {
                _rotatedObjects.Add(engine);                
            }
        }

        public void RemoveRotatedObj(ObjectRotation engine)
        {
            if (!_rotatedObjects.Contains(engine))
            {
                return;
            }

            _rotatedObjects.Remove(engine);
        }

        public void Execute()
        {
            if (!IsActive)
            {
                return;
            }     

            for (int i = 0; i < _rotatedObjects.Count; i++)
            {
                ObjectRotation obj = _rotatedObjects.ElementAt(i);
                obj.RotationOfThis();
            }
        }
    }
}
