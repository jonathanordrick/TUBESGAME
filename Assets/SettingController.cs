using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingController : MonoBehaviour
{
    public GameObject popupPanel;
    public Button settingButton;
    public Button exitButton;
    public Button closeButton;

    void Start()
    {
        popupPanel.SetActive(false);

        settingButton.onClick.AddListener(() =>
        {
            popupPanel.SetActive(true);
            Time.timeScale = 0f; // Pause game
        });

        closeButton.onClick.AddListener(() =>
        {
            popupPanel.SetActive(false);
            Time.timeScale = 1f; // Resume game
        });

        exitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f; // Resume game sebelum pindah scene
            SceneManager.LoadScene("StartScene"); // Ganti sesuai kebutuhan
        });
    }
}
