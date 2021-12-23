using UnityEngine.UI;
using UnityEngine;


namespace MaksK_Race
{
	public sealed class SliderUI : MonoBehaviour, IControl
	{
		private Text _text;
		private Slider _control;

		private void Awake()
		{
			_control = transform.GetComponentInChildren<Slider>();
			_text = transform.GetComponentInChildren<Text>();
		}

		public Text GetText => _text;

		public Slider GetControl => _control;

		public void Interactable(bool value)
		{
			GetControl.interactable = value;
		}

		public GameObject Instance => gameObject;
		public Selectable Control => GetControl;
	}
}
