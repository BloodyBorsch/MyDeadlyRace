using UnityEngine;
using UnityEngine.UI;


namespace MaksK_Race
{
	public class OptionsMenu : BaseMenu
	{
		[SerializeField] private GameObject _optionsMenu;
		[SerializeField] private ButtonUi _back;
		[SerializeField] private Text _tips;

		private void Start()
		{			
			LocalizationOptions();
			_back.GetControl.onClick.AddListener(delegate
			{
				Back();
			});
		}

		public void LocalizationOptions()
		{
			_back.GetText.text = LangManager.Instance.Text("OptionsMenuItems", "Back");
			_tips.text = LangManager.Instance.Text("OptionsMenuItems", "Tips");
		}

		private void LoadVideoOptions()
		{
			Interface.Execute(InterfaceObject.VideoOptions);
		}

		private void LoadSoundOptions()
		{
			Interface.Execute(InterfaceObject.AudioOptions);
		}

		private void LoadGameOptions()
		{
			Interface.Execute(InterfaceObject.GameOptions);
		}

		private void Back()
		{
			Interface.Execute(InterfaceObject.MainMenu);
		}

		public override void Hide()
		{
			if (!IsShow) return;
			_optionsMenu.gameObject.SetActive(false);
			IsShow = false;
		}

		public override void Show()
		{
			if (IsShow) return;
			_optionsMenu.gameObject.SetActive(true);
			IsShow = true;
		}
	}
}
