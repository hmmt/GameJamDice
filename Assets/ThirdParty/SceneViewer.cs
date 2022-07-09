#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.SceneManagement;
using System;

public class SceneViewer : EditorWindow
{
    [MenuItem("Window/General/SceneViewer")]
    static void Init()
    {
        SceneViewer viewer = (SceneViewer)EditorWindow.GetWindow(typeof(SceneViewer));
        viewer.Show();
    }

    void OnGUI()
    {
        var sceneCount = EditorSceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            var sceneName = GetSceneName(i);
            if (GUILayout.Button(sceneName))
            {
                var activeScene = EditorSceneManager.GetActiveScene();
                if(activeScene.isDirty)
                {
                    ShowDisplayDialog(activeScene.path, 
                    () =>
                    {
                        EditorSceneManager.SaveScene(activeScene);
                        EditorSceneManager.OpenScene(GetScenePath(i));
                    },
                    () =>
                    {
                        EditorSceneManager.OpenScene(GetScenePath(i));
                    });
                }
                else
                {
                    EditorSceneManager.OpenScene(GetScenePath(i));
                }
            }
        }

        string GetSceneName(int index)
            => System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));

        string GetScenePath(int index)
            => SceneUtility.GetScenePathByBuildIndex(index);
    }

    void ShowDisplayDialog(string scenePath, Action saveCallback, Action notSaveCallback)
    {
        var title   = "Scene(s) Have been Modified";
        var message = $"Do you want to save the changes you made in the\nscenes:\n {scenePath}\n\nYour changes will be lost if you don't save them";
        var ok      = "Save";
        var cancel  = "cancel";
        var alt     = "Don't Save";
        var option  = EditorUtility.DisplayDialogComplex(title, message, ok, cancel, alt);

        switch(option)
        {
            //Save
            case 0:
                saveCallback();
                break;
            //Cancel
            case 1:
                break;
            //Don't Save
            case 2:
                notSaveCallback();
                break;
        }
    }
}
#endif