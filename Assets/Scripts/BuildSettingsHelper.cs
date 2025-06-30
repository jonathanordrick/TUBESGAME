using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
#endif

public class BuildSettingsHelper : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Build Settings Helper")]
    [Tooltip("Click this button to automatically add all scenes to Build Settings")]
    public bool addScenesToBuildSettings = false;

    void OnValidate()
    {
        if (addScenesToBuildSettings)
        {
            addScenesToBuildSettings = false;
            AddScenesToBuildSettings();
        }
    }

    public static void AddScenesToBuildSettings()
    {
        // Get all scene assets in the project
        string[] guids = AssetDatabase.FindAssets("t:Scene");
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        
        // Add existing scenes from build settings (to avoid duplicates)
        if (EditorBuildSettings.scenes != null && EditorBuildSettings.scenes.Length > 0)
        {
            editorBuildSettingsScenes.AddRange(EditorBuildSettings.scenes);
        }

        bool hasChanges = false;
        
        foreach (string guid in guids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            
            // Check if scene is already in build settings
            bool alreadyExists = editorBuildSettingsScenes.Any(scene => scene.path == scenePath);
            
            if (!alreadyExists)
            {
                // Add scene to build settings
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                hasChanges = true;
                Debug.Log($"Added scene to Build Settings: {scenePath}");
            }
        }

        if (hasChanges)
        {
            // Update the build settings
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            Debug.Log("Build Settings updated successfully!");
            
            // Log all scenes in build settings
            Debug.Log("=== Current Build Settings Scenes ===");
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var scene = EditorBuildSettings.scenes[i];
                Debug.Log($"[{i}] {scene.path} - Enabled: {scene.enabled}");
            }
        }
        else
        {
            Debug.Log("All scenes are already in Build Settings.");
        }
    }

    [ContextMenu("Add All Scenes to Build Settings")]
    public void AddAllScenes()
    {
        AddScenesToBuildSettings();
    }

    [ContextMenu("Check EndGame Scene")]
    public void CheckEndGameScene()
    {
        string endGameScenePath = "Assets/Scenes/EndGame.unity";
        
        // Check if EndGame scene exists
        if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(endGameScenePath) != null)
        {
            Debug.Log("✓ EndGame.unity scene found at: " + endGameScenePath);
            
            // Check if it's in build settings
            bool inBuildSettings = EditorBuildSettings.scenes.Any(scene => scene.path == endGameScenePath);
            
            if (inBuildSettings)
            {
                Debug.Log("✓ EndGame scene is in Build Settings");
            }
            else
            {
                Debug.LogWarning("✗ EndGame scene is NOT in Build Settings!");
                
                // Automatically add it
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
#endif

    void Start()
    {
        // Auto-check on play mode (untuk development)
        #if UNITY_EDITOR
        if (Application.isPlaying)
        {
            CheckEndGameScene();
        }
        #endif
    }
}
