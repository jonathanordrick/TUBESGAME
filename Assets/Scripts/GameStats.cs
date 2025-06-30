using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    // Static variables untuk menyimpan data antar scene
    public static int totalDeaths = 0;
    public static int totalRespawns = 0;
    public static float gamePlayTime = 0f;
    public static int checkpointsReached = 0;
    
    private static GameStats instance;
    private float sessionStartTime;
    
    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            sessionStartTime = Time.time;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        // Update play time
        gamePlayTime = Time.time - sessionStartTime;
    }
    
    public static void RecordDeath()
    {
        totalDeaths++;
        Debug.Log($"Total deaths: {totalDeaths}");
    }
    
    public static void RecordRespawn()
    {
        totalRespawns++;
        Debug.Log($"Total respawns: {totalRespawns}");
    }
    
    public static void RecordCheckpoint()
    {
        checkpointsReached++;
        Debug.Log($"Checkpoints reached: {checkpointsReached}");
    }
    
    public static void ResetStats()
    {
        totalDeaths = 0;
        totalRespawns = 0;
        gamePlayTime = 0f;
        checkpointsReached = 0;
        
        if (instance != null)
        {
            instance.sessionStartTime = Time.time;
        }
        
        Debug.Log("Game stats reset");
    }
    
    public static string GetFormattedPlayTime()
    {
        int minutes = Mathf.FloorToInt(gamePlayTime / 60F);
        int seconds = Mathf.FloorToInt(gamePlayTime - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    
    // Method untuk dipanggil saat game restart (untuk reset stats jika diinginkan)
    public static void OnGameRestart()
    {
        // Optional: Reset stats saat restart game dari EndGame scene
        // Uncomment line dibawah jika ingin reset stats setiap restart
        // ResetStats();
        
        Debug.Log("Game restarted from EndGame scene");
    }
}
