using UnityEditor;
using UnityEngine;


namespace MaksK_Race
{
    [CustomEditor(typeof(GameController))]
    public class CreateInterfaceEditor : Editor
    {
        private static GameController _gameController;

        private static bool _isPressButtonPoolObjects;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var interfaceTarget = (GameController)target;

            if (EditorApplication.isPlaying) return;

            if (!interfaceTarget.GetComponent<PoolObjects>()) 
                _isPressButtonPoolObjects = GUILayout.Button("Создать Пул объектов", 
                    EditorStyles.miniButton);
            
            if (_isPressButtonPoolObjects)
            {
                interfaceTarget.CreatePoolObjects();
                _isPressButtonPoolObjects = !_isPressButtonPoolObjects;
            }            
        }
    }
}
