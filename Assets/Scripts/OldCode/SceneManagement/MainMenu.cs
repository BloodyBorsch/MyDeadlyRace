using UnityEngine;
using UnityEngine.SceneManagement;


namespace Old_Code
{
    public class MainMenu : BaseMenu
    {
        #region Properties

        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private ButtonUi _newGame;
        [SerializeField] private ButtonUi _continue;
        [SerializeField] private ButtonUi _options;
        [SerializeField] private ButtonUi _quit;
        [SerializeField] private ButtonUi _language;
        [SerializeField] private SliderUI _audioSettings;
        [SerializeField] private DropDownUI _resolutionSettings;
        [SerializeField] private ObjectRotation[] _rotatedObjects;

        public InterfaceResources InterfaceResources { get; private set; }

        #endregion


        #region UnityMethods

        private void Start()
        {
            InterfaceResources = GetComponent<InterfaceResources>();
            _rotatedObjects = FindObjectsOfType<ObjectRotation>();

            LocalizationMainMenu();

            _newGame.GetControl.onClick.AddListener(delegate
            {
                //LoadNewGame(SceneManagerHelper.Instance.Scenes.Game.SceneAsset.name);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);                
            });

            if (_continue != null)
            {
                _continue.SetInteractable(false);
            }

            _options.SetInteractable(true);
            _options.GetControl.onClick.AddListener(delegate
            {
                ShowOptions();
            });

            _quit.GetControl.onClick.AddListener(delegate
            {
                Interface.QuitGame();
            });

            _language.SetInteractable(true);
            _language.GetControl.onClick.AddListener(delegate
            {
                LangManager.Instance.LangSwitcher();                
            });

            _audioSettings.GetText.text = "Volume:";

            _resolutionSettings.GetText.text = "Resolution:";
        }

        private void Update()
        {
            foreach (ObjectRotation obj in _rotatedObjects)
            {
                obj.RotationOfThis();
            }            
        }

        #endregion


        #region Methods

        public void LocalizationMainMenu()
        {
            _newGame.GetText.text = LangManager.Instance.Text("MainMenuItems", "NewGame");

            if (_continue != null)
            {
                _continue.GetText.text = LangManager.Instance.Text("MainMenuItems", "Continue");
            }

            _options.GetText.text = LangManager.Instance.Text("MainMenuItems", "Options");
            _quit.GetText.text = LangManager.Instance.Text("MainMenuItems", "Quit");
            _language.GetText.text = LangManager.Instance.Text("MainMenuItems", "Language");
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutionSettings.resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public override void Hide()
        {
            if (!IsShow) return;
            _mainPanel.gameObject.SetActive(false);
            IsShow = false;
        }

        public override void Show()
        {
            if (IsShow) return;
            _mainPanel.gameObject.SetActive(true);
            IsShow = true;
        }

        public void SetVolume(float volume)
        {
            InterfaceResources.AudioMixer.SetFloat("master", volume);            
        }

        private void ShowOptions()
        {
            Interface.Execute(InterfaceObject.OptionsMenu);
        }

        //private void LoadNewGame(string lvl)
        private void LoadNewGame(int lvl)
        {
            //SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            Interface.LoadSceneAsync(lvl);
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            // init game

            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

        #endregion
    }
}