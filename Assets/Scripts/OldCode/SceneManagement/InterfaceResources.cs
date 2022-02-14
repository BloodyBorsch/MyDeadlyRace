using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


namespace Old_Code
{
    public class InterfaceResources : MonoBehaviour
    {

        public ButtonUi ButtonPrefab { get; private set; }
        public Canvas MainCanvas { get; private set; }
        public LayoutGroup MainPanel { get; private set; }
        public SliderUI ProgressbarPrefab { get; private set; }
        public AudioMixer AudioMixer { get; private set; }

        private void Awake()
        {
            ButtonPrefab = Resources.Load<ButtonUi>("Button");
            MainCanvas = FindObjectOfType<Canvas>();
            ProgressbarPrefab = Resources.Load<SliderUI>("Progressbar");
            MainPanel = MainCanvas.GetComponentInChildren<LayoutGroup>();
            AudioMixer = Resources.Load<AudioMixer>("MainMixer");
        }
    }
}
