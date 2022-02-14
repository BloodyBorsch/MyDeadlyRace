using System.Linq;
using UnityEngine;


namespace MyRacing
{
    [CreateAssetMenu(fileName = nameof(CarData), menuName = NameManager.CREATE_DATA_MENU_NAME + nameof(CarData))]
    public class CarData : ScriptableObject
    {
        public CarSettings[] Cars;

        public CarSettings LoadCarSettings(CarType type)
        {
            return Cars.Where(w => w.AskTypeOfData() == type).FirstOrDefault();
        }
    }
}