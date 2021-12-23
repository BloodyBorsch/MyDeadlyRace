using UnityEngine;


namespace MaksK_Race
{
    public sealed class ColorRandomizer : BaseObjectScene
    {
        private void Start()
        {            
            Color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }        
    }
}
