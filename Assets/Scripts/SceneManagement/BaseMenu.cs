using Object = UnityEngine.Object;
using UnityEngine;
using System;


namespace Old_Code
{
	public abstract class BaseMenu : MonoBehaviour
	{
		protected IControl[] _elementsOfInterface;

		public bool IsShow { get; set; }

		protected Interface Interface;

		protected virtual void Awake()
		{
			Interface = FindObjectOfType<Interface>();
		}

		public abstract void Hide();

		public abstract void Show();

		protected void Clear(IControl[] controls)
		{
			foreach (var t in controls)
			{
				if (t == null) continue;
				Destroy(t.Instance);
			}
		}

		protected T CreateControl<T>(T prefab, string text) where T : Object, IControlText
		{
			if (!prefab) throw new Exception(string.Format("Отсутствует ссылка на {0}", typeof(T)));
			var tempControl = Instantiate(prefab, Interface.InterfaceResources.MainPanel.transform.position, Quaternion.identity,
				Interface.InterfaceResources.MainPanel.transform);
			//----
			if (tempControl.GetText != null)
			{
				tempControl.GetText.text = text;
			}
			//---
			return tempControl;
		}

		protected T CreateControlText<T>(T prefab, string text) where T : Object, IControlText
		{
			var tempControl = CreateControl<T>(prefab, text);

			if (tempControl.GetText != null)
			{
				tempControl.GetText.text = text;
			}
			return tempControl;
		}
	}
}