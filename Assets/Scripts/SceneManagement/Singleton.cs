using UnityEngine;


namespace Old_Code
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		private static object _lock = new object();
		private static bool _applicationIsQuitting;

		public static T Instance
		{
			get
			{
				if (_applicationIsQuitting)
				{
					return null;
				}
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = (T)FindObjectOfType(typeof(T));
						if (FindObjectsOfType(typeof(T)).Length > 1)
						{

							return _instance;
						}
						if (_instance == null)
						{
							GameObject singleton = new GameObject();
							_instance = singleton.AddComponent<T>();
							singleton.name = $"(singleton)  {typeof(T)}";
							DontDestroyOnLoad(singleton);

						}
						else
						{

						}
					}
					return _instance;
				}
			}
		}
		public void OnDestroy()
		{
			_applicationIsQuitting = true;
		}
	}
}
