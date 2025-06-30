using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverStats : MonoBehaviour
{
    [Header("Stats UI Elements")]
    public Text deathCountText;
    public Text respawnCountText;
    public Text playTimeText;
    public Text checkpointsText;
    public Text finalScoreText;
    
    void Start()
    {
        DisplayStats();
    }
    
    void DisplayStats()
    {
        // Display death count
        if (deathCountText != null)
        {
            deathCountText.text = $"Deaths: {GameStats.totalDeaths}";
        }
        
        // Display respawn count
        if (respawnCountText != null)
        {
            respawnCountText.text = $"Respawns Used: {GameStats.totalRespawns}";
        }
        
        // Display play time
        if (playTimeText != null)
        {
            playTimeText.text = $"Play Time: {GameStats.GetFormattedPlayTime()}";
        }
        
        // Display checkpoints reached
        if (checkpointsText != null)
        {
            checkpointsText.text = $"Checkpoints: {GameStats.checkpointsReached}";
        }
        
        // Calculate and display final score
        if (finalScoreText != null)
        {
            int score = CalculateFinalScore();
            finalScoreText.text = $"Final Score: {score}";
        }
    }
    
    int CalculateFinalScore()
    {
        // Simple scoring system
        int baseScore = 1000;
        int checkpointBonus = GameStats.checkpointsReached * 100;
        int deathPenalty = GameStats.totalDeaths * 50;
        int timePenalty = Mathf.FloorToInt(GameStats.gamePlayTime);
        
        int finalScore = baseScore + checkpointBonus - deathPenalty - timePenalty;
        return Mathf.Max(0, finalScore); // Ensure score is not negative
    }
}
