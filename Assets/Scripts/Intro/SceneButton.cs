using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneButton : MonoBehaviour
{
    [Header("Drag & Drop Scene Asset di Sini")]
    public Object sceneAsset;

    private string sceneName;

    void Start()
    {
#if UNITY_EDITOR
        if (sceneAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(sceneAsset);
            sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
        }
#endif
    }

    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene belum diisi atau tidak valid.");
        }
    }
}
