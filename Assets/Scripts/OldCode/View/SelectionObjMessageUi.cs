using UnityEngine;
using UnityEngine.UI;


namespace Old_Code
{
	public sealed class SelectionObjMessageUi : MonoBehaviour
	{
		private Text _text;

		private void Awake()
		{
			_text = GetComponent<Text>();
		}

		public string Text
		{
			set => _text.text = $"{value}";
		}

		public void SetActive(bool value)
		{
			_text.gameObject.SetActive(value);
		}
	}
}