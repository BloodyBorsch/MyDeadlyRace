using UnityEngine;
using UnityEngine.UI;


namespace MaksK_Race
{
	public sealed class ToggleUI : MonoBehaviour, IControl
	{
		public Text GetText { get; private set; }
		public Toggle GetControl { get; private set; }

		private void Awake()
		{
			GetControl = GetComponent<Toggle>();
			GetText = transform.GetComponentInChildren<Text>();
		}

		public void Interactable(bool value)
		{
			GetControl.interactable = value;
		}

		public GameObject Instance { get { return gameObject; } }
		public Selectable Control { get { return GetControl; } }
	}
}