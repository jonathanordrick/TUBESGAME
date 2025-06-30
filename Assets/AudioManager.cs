// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AudioManager : MonoBehaviour
// {
//     public static AudioManager instance;

//     private void Awake()
//     {
//         if(instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Subscribe ke event perubahan scene
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fungsi callback saat scene berganti
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Contoh: hancurkan di scene dengan nama "MainMenu" atau "Credit"
        if (scene.name == "Main" || scene.name == "Main1" || scene.name == "Main2")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe event untuk mencegah memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
