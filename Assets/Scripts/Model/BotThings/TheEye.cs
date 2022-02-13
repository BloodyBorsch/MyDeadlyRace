using UnityEngine;


namespace Old_Code
{
    public sealed class TheEye : MonoBehaviour
    {
        [SerializeField] private Transform _botTarget;
        [SerializeField] private Transform _carTarget;
        [SerializeField] private MainBot _parent;

        private void Awake()
        {
            _parent = GetComponentInParent<MainBot>();
        }

        private void OnTriggerEnter(Collider other)
        {              
            if (other != null)
            {
                //MainBot BotObject = other.gameObject.GetComponent<MainBot>();
                BodyBot BotObject = other.gameObject.GetComponent<BodyBot>();
                CarCorpse CarObject = other.gameObject.GetComponent<CarCorpse>();                

                if (BotObject != null &&
                BotObject.GetComponentInParent<MainBot>().TypeOfBot != _parent.TypeOfBot &&
                BotObject.GetComponentInParent<MainBot>().StateOfBot != StateBot.Died)
                {                    
                    _botTarget = BotObject.transform;
                    _parent.Target = _botTarget;
                }

                if (_parent.TypeOfBot != BotTeams.Friendly)
                {
                    if (CarObject != null)
                    {
                        _carTarget = CarObject.transform;
                        _parent.Target = _carTarget;
                    }
                }                
            }            
        }
    }
}
