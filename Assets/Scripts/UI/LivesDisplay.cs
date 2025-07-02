using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Text livesText; // Legacy Text component
    public TMPro.TextMeshProUGUI livesTextTMP; // TextMeshPro component
    public Image[] livesIcons; // Array of life icons (hearts, stars, etc.)
    
    private PlayerHealth playerHealth;
    
    private void Start()
    {
        // Find PlayerHealth component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component not found on Player!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
        
        UpdateLivesDisplay();
    }
    
    private void Update()
    {
        UpdateLivesDisplay();
    }
    
    private void UpdateLivesDisplay()
    {
        if (playerHealth == null) return;
        
        int currentLives = playerHealth.GetCurrentLives();
        int maxLives = playerHealth.maxLives;
        
        // Update text displays
        string livesText = $"Lives: {currentLives}/{maxLives}";
        
        if (this.livesText != null)
            this.livesText.text = livesText;
            
        if (livesTextTMP != null)
            livesTextTMP.text = livesText;
        
        // Update icon displays
        if (livesIcons != null && livesIcons.Length > 0)
        {
            for (int i = 0; i < livesIcons.Length; i++)
            {
                if (livesIcons[i] != null)
                {
                    livesIcons[i].gameObject.SetActive(i < currentLives);
                }
            }
        }
    }
}
