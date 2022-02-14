namespace Old_Code
{
	public class Main : Singleton<Main>
	{
		protected Main() { }

		[System.Serializable]
		public struct SceneDate
		{
			public SceneField MainMenu;
			public SceneField Game;
		}

		public SceneDate Scenes;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void InitGame()
		{
			// Add Controllers
		}
	}
}
