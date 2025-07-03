using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleDoor : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [SerializeField] private string sceneToLoad = "Level 2"; // Nama scene yang akan dimuat
    [SerializeField] private bool requireInteraction = false; // Apakah perlu menekan tombol untuk masuk
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // Tombol untuk berinteraksi
    [SerializeField] private float transitionDelay = 0.5f; // Delay sebelum pindah scene
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject interactionPrompt; // UI prompt untuk interaksi

    [Header("Boss Condition")]
    [SerializeField] private GameObject minotaurBoss; // Assign in Inspector or auto-find
    
    private bool playerInRange = false;
    private bool isTransitioning = false;
    
    void Start()
    {
        // Pastikan interaction prompt tidak aktif di awal
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (minotaurBoss == null)
        {
            GameObject bossObj = GameObject.Find("MinotaurBoss");
            if (bossObj != null)
                minotaurBoss = bossObj;
        }
    }

    void Update()
    {
        // Jika player dalam jangkauan dan memerlukan interaksi
        if (playerInRange && requireInteraction && !isTransitioning)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                if (IsBossDead())
                {
                    StartSceneTransition();
                }
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah yang masuk adalah player
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            // Tampilkan prompt jika diperlukan interaksi
            if (requireInteraction && interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
            // Jika tidak memerlukan interaksi, langsung pindah scene
            else if (!requireInteraction && !isTransitioning && IsBossDead())
            {
                StartSceneTransition();
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        // Cek apakah yang keluar adalah player
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            // Sembunyikan prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
    
    void StartSceneTransition()
    {
        if (isTransitioning) return;
        
        isTransitioning = true;
        
        // Sembunyikan prompt jika ada
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        // Mulai coroutine untuk transisi dengan delay
        StartCoroutine(TransitionToScene());
    }
    
    IEnumerator TransitionToScene()
    {
        // Tunggu sesuai delay yang ditentukan
        yield return new WaitForSeconds(transitionDelay);
        
        // Cek apakah scene ada dalam build settings
        if (IsSceneInBuildSettings(sceneToLoad))
        {
            // Muat scene baru
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError($"Scene '{sceneToLoad}' tidak ditemukan dalam Build Settings!");
            isTransitioning = false;
        }
    }
    
    bool IsSceneInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneNameFromPath == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    bool IsBossDead()
    {
        // Boss dianggap mati jika GameObject null (destroyed) atau tidak aktif di scene
        return minotaurBoss == null || !minotaurBoss.activeInHierarchy;
    }
}
