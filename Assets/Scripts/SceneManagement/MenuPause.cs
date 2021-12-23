using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace MaksK_Race
{
    public sealed class MenuPause : BaseMenu
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Text _tipsPanel;
        [SerializeField] private Text _counterText;
        [SerializeField] private ButtonUi _resume;
        [SerializeField] private ButtonUi _options;
        [SerializeField] private ButtonUi _quit;
        [SerializeField] private SliderUI _masterAudio;        
        [SerializeField] private SliderUI _soundAudio;        
        [SerializeField] private SliderUI _musicAudio;
        [SerializeField] private ButtonUi _language;

        private int Kills;

        private AudioMixer _audioMixer;        
        private AudioMixerSnapshot _pause;
        private AudioMixerSnapshot _unPause;

        private bool _isPause;

        public InterfaceResources InterfaceResources { get; private set; }
        public FriendlyBotUI FriendlyBotMenu;

        private void Start()
        {            
            _audioMixer = Resources.Load<AudioMixer>("MainMixer");            
            _pause = _audioMixer.FindSnapshot("Paused");
            _unPause = _audioMixer.FindSnapshot("UnPaused");
            InterfaceResources = GetComponent<InterfaceResources>();

            LocalizationPauseMenu();

            _resume.SetInteractable(true);
            _resume.GetControl.onClick.AddListener(delegate
            {
                Pause();
            });

            _options.SetInteractable(true);

            _quit.SetInteractable(true);
            _quit.GetControl.onClick.AddListener(delegate
            {
                //Time.timeScale = 1.0f;
                //_unPause.TransitionTo(0.0001f);
                //SceneManager.LoadScene(0);  
                Application.Quit();
            });

            _language.SetInteractable(true);
            _language.GetControl.onClick.AddListener(delegate
            {
                LangManager.Instance.LangSwitcher();
            });

            _masterAudio.GetText.text = "Master:";  
            _soundAudio.GetText.text = "Sound:";
            _musicAudio.GetText.text = "Music:";

            _pausePanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }

        public void Pause()
        {
            _isPause = !_isPause;
            _pausePanel.SetActive(_isPause);
            GameController.Instance.PauseToggle(_isPause);

            if (_isPause)
            {
                Time.timeScale = 0.0f;
                _pause.TransitionTo(0.0001f);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1.0f;
                _unPause.TransitionTo(0.0001f);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public override void Hide()
        {
            if (!IsShow) return;
            IsShow = false;
        }

        public override void Show()
        {
            if (IsShow) return;
            IsShow = true;
        }

        public void LocalizationPauseMenu()
        {            
            _resume.GetText.text = LangManager.Instance.Text("PauseMenuItems", "Resume");

            if (_options != null)
            {
                _options.GetText.text = LangManager.Instance.Text("PauseMenuItems", "Options");
            }

            _tipsPanel.text = LangManager.Instance.Text("PauseMenuItems", "Tips");
            _quit.GetText.text = LangManager.Instance.Text("PauseMenuItems", "Quit");
            _language.GetText.text = LangManager.Instance.Text("MainMenuItems", "Language");
        }

        public void SetMasterVolume(float volume)
        {
            _audioMixer.SetFloat("master", volume);
        }
        public void SetSoundVolume(float volume)
        {
            _audioMixer.SetFloat("sound", volume);
        }
        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat("music", volume);
        }

        public void DebugWhriter()
        {
            Debug.Log("Нажата кнопка");
        }

        public void KillsCounter()
        {            
            Kills++;
            _counterText.text = $"Kills: {Kills.ToString()}";
        }        

        //private void OnGUI()
        //{
        //    var input = Event.current;
        //    if (input.isKey)
        //    {
        //        var inputText = input.keyCode.ToString();
        //        if (!inputText.Contains("None"))
        //        {
        //            _counterText.text = inputText;
        //        }
        //    }
        //}
    }
}
