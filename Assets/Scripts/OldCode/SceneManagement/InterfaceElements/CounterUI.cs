using UnityEngine;
using UnityEngine.UI;


namespace Old_Code
{
    public sealed class CounterUI : MonoBehaviour
    {
        private Text _text;
        private int _countPoint;

        private void Awake()
        {            
            _text = GetComponent<Text>();
        }

        //private void OnEnable()
        //{
        //    foreach (var aim in _aims)
        //    {
        //        aim.OnPointChange += UpdatePoint;
        //    }
        //}

        //private void OnDisable()
        //{
        //    foreach (var aim in _aims)
        //    {
        //        aim.OnPointChange -= UpdatePoint;
        //    }
        //}

        //private void UpdatePoint()
        //{
        //    var pointTxt = "очков";
        //    ++_countPoint;
        //    if (_countPoint >= 5) pointTxt = "очков";
        //    else if (_countPoint == 1) pointTxt = "очко";
        //    else if (_countPoint < 5) pointTxt = "очка";
        //    _text.text = $"Вы заработали {_countPoint} {pointTxt}";

        //    //todo отписаться удалить и списка
        //}
    }
}
