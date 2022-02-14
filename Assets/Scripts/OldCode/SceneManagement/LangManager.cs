using System.Xml;
using UnityEngine;


namespace Old_Code
{
    public class LangManager : Singleton<LangManager>
    {
        #region Properties

        private XmlDocument _root;
        private MainMenu _mainMenu;
        private OptionsMenu _optionsMenu;
        private MenuPause _pauseMenu;

        public string LanguageCode { get; private set; }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            if (FindObjectOfType<MainMenu>() != null)
            {
                _mainMenu = FindObjectOfType<MainMenu>();
            }
            if (FindObjectOfType<OptionsMenu>() != null)
            {
                _optionsMenu = FindObjectOfType<OptionsMenu>();
            }
            if (FindObjectOfType<MenuPause>() != null)
            {
                _pauseMenu = FindObjectOfType<MenuPause>();
            }


            LanguageCode = "En";
            Init("Language", LanguageCode);
        }                

        #endregion


        #region Methods

        public void Init(string file, string languageCode = "")
        {
            _root = new XmlDocument();
            if (languageCode == "")
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Russian:
                        LanguageCode = "Ru";
                        break;
                    default:
                        LanguageCode = "En";
                        break;
                }
            }
            var config = LoadResource(file);
            if (!config) return;
            _root.LoadXml(config.text);
        }

        public string Text(string level, string id)
        {
            if (_root == null)
            {
                return "[not init]";
            }
            string path = "Settings/group[@id='" + level + "']/string[@id='" + id +
            "']/text";
            XmlNode value = _root.SelectSingleNode(path);
            if (value == null)
            {
                return "[not found]";
            }
            return value.InnerText;
        }

        public void LangSwitcher()
        {
            if (LanguageCode != "En")
            {
                LanguageCode = "En";
            }

            else
            {
                LanguageCode = "Ru";
            }

            Init("Language", LanguageCode);
            if (_mainMenu != null) _mainMenu.LocalizationMainMenu();
            if (_optionsMenu != null) _optionsMenu.LocalizationOptions();
            if (_pauseMenu != null) _pauseMenu.LocalizationPauseMenu();
        }

        private string LocalizeResourceName(string resourceName)
        {
            return $"{resourceName}{LanguageCode}";
        }

        private TextAsset LoadResource(string resourceName)
        {
            return Resources.Load(LocalizeResourceName(resourceName),
            typeof(TextAsset)) as TextAsset;
        }

        #endregion
    }
}
