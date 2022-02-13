using UnityEngine.UI;
using UnityEngine;


namespace Old_Code
{
	public sealed class HealthBarUI : MonoBehaviour, IControl
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

		public void SetMaxHealth(float health)
		{
			_control.maxValue = health;
			_control.value = health;
		}

		public void SetHealth(float health)
		{
			_control.value = health;
		}
		public void Interactable(bool value)
		{
			GetControl.interactable = value;
		}

		public GameObject Instance => gameObject;
		public Selectable Control => GetControl;
	}
}
