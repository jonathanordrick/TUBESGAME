#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class FixBuildSettings : EditorWindow
{
    [MenuItem("Tools/Fix Build Settings")]
    public static void ShowWindow()
    {
        GetWindow<FixBuildSettings>("Fix Build Settings");
    }

    void OnGUI()
    {
        GUILayout.Label("Build Settings Helper", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Add All Scenes to Build Settings"))
        {
            AddAllScenesToBuildSettings();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Check EndGame Scene"))
        {
            CheckEndGameScene();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Show Current Build Settings"))
        {
            ShowCurrentBuildSettings();
        }
        
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox("Use these buttons to fix the scene loading issues.\n\n" +
            "1. 'Add All Scenes' will add all scenes in your project to Build Settings\n" +
            "2. 'Check EndGame Scene' will specifically check if EndGame scene is properly configured\n" +
            "3. 'Show Current Build Settings' will list all scenes currently in Build Settings", 
            MessageType.Info);
    }

    static void AddAllScenesToBuildSettings()
    {
        string[] guids = AssetDatabase.FindAssets("t:Scene");
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        
        if (EditorBuildSettings.scenes != null && EditorBuildSettings.scenes.Length > 0)
        {
            editorBuildSettingsScenes.AddRange(EditorBuildSettings.scenes);
        }

        bool hasChanges = false;
        
        foreach (string guid in guids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            
            bool alreadyExists = editorBuildSettingsScenes.Any(scene => scene.path == scenePath);
            
            if (!alreadyExists)
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                hasChanges = true;
                Debug.Log($"Added scene to Build Settings: {scenePath}");
            }
        }

        if (hasChanges)
        {
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            Debug.Log("✓ Build Settings updated successfully!");
        }
        else
        {
            Debug.Log("✓ All scenes are already in Build Settings.");
        }
        
        ShowCurrentBuildSettings();
    }

    static void CheckEndGameScene()
    {
        string endGameScenePath = "Assets/Scenes/EndGame.unity";
        
        if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(endGameScenePath) != null)
        {
            Debug.Log("✓ EndGame.unity scene found at: " + endGameScenePath);
            
            bool inBuildSettings = EditorBuildSettings.scenes.Any(scene => scene.path == endGameScenePath);
            
            if (inBuildSettings)
            {
                Debug.Log("✓ EndGame scene is in Build Settings");
                
                // Find the index
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    if (EditorBuildSettings.scenes[i].path == endGameScenePath)
                    {
                        Debug.Log($"✓ EndGame scene is at index {i} in Build Settings");
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("✗ EndGame scene is NOT in Build Settings!");
                
                List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                scenes.Add(new EditorBuildSettingsScene(endGameScenePath, true));
                EditorBuildSettings.scenes = scenes.ToArray();
                Debug.Log("✓ EndGame scene added to Build Settings automatically");
            }
        }
        else
        {
            Debug.LogError("✗ EndGame.unity scene not found at: " + endGameScenePath);
        }
    }

    static void ShowCurrentBuildSettings()
    {
        Debug.Log("=== Current Build Settings Scenes ===");
        
        if (EditorBuildSettings.scenes == null || EditorBuildSettings.scenes.Length == 0)
        {
            Debug.LogWarning("No scenes in Build Settings!");
            return;
        }
        
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            string status = scene.enabled ? "✓" : "✗";
            Debug.Log($"[{i}] {status} {scene.path}");
        }
    }
}
#endif
