using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Fungsi untuk mengganti scene berdasarkan nama scene
    public void GantiScene(string namaScene)
    {
        SceneManager.LoadScene(namaScene);
    }

    // Fungsi untuk keluar dari game
    public void KeluarGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
