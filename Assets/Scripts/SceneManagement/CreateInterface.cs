using UnityEngine;


namespace MaksK_Race
{
    public class CreateInterface : MonoBehaviour
    {
        #region Editor

        public void CreateMainMenu()
        {
            CreateComponent();
            gameObject.AddComponent<MainMenu>();
            gameObject.AddComponent<OptionsMenu>();
            //gameObject.AddComponent<TestMenu>();
            Clear();
        }

        public void CreateMenuPause()
        {
            CreateComponent();
            Clear();
        }

        private void Clear()
        {
            DestroyImmediate(this);
        }

        private void CreateComponent()
        {
            gameObject.AddComponent<Interface>();
            gameObject.AddComponent<InterfaceResources>();
        }

        #endregion
    }
}
