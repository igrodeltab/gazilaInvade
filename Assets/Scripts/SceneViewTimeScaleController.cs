using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class SceneViewTimeScaleController
{
    static SceneViewTimeScaleController()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(10, 10, 150, 200), "Time Control", GUI.skin.window);

        GUILayout.Label("Control Game Time Scale", EditorStyles.boldLabel);

        if (GUILayout.Button("Normal Speed"))
        {
            Time.timeScale = 1f;
            EditorApplication.isPaused = false;
        }
        if (GUILayout.Button("Slow Down x2"))
        {
            Time.timeScale = 0.5f;
            EditorApplication.isPaused = false;
        }
        if (GUILayout.Button("Slow Down x4"))
        {
            Time.timeScale = 0.25f;
            EditorApplication.isPaused = false;
        }
        if (GUILayout.Button("Slow Down x8"))
        {
            Time.timeScale = 0.125f;
            EditorApplication.isPaused = false;
        }

        GUILayout.Space(10);

        GUILayout.Label("Pause Control", EditorStyles.boldLabel);

        if (GUILayout.Button(EditorApplication.isPaused ? "Unpause" : "Pause"))
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        if (GUILayout.Button("Step to Next Frame"))
        {
            if (EditorApplication.isPaused)
            {
                EditorApplication.Step();
            }
        }

        GUILayout.EndArea();

        Handles.EndGUI();
    }
}