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

        GUILayout.BeginArea(new Rect(10, 10, 150, 130), "Time Scale", GUI.skin.window);

        GUILayout.Label("Control Game Time Scale", EditorStyles.boldLabel);

        if (GUILayout.Button("Normal Speed"))
        {
            Time.timeScale = 1f;
        }
        if (GUILayout.Button("Slow Down x2"))
        {
            Time.timeScale = 0.5f;
        }
        if (GUILayout.Button("Slow Down x4"))
        {
            Time.timeScale = 0.25f;
        }
        if (GUILayout.Button("Slow Down x8"))
        {
            Time.timeScale = 0.125f;
        }

        GUILayout.EndArea();

        Handles.EndGUI();
    }
}