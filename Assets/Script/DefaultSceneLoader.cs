#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Script
{
    [InitializeOnLoadAttribute]
    public static class DefaultSceneLoader
    {
        static DefaultSceneLoader()
        {
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

        private static void LoadDefaultScene(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            if (state == PlayModeStateChange.EnteredPlayMode) SceneManager.LoadScene(0);
        }
    }
}
#endif