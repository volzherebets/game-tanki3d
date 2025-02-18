using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu; 
    public GameObject soundButton; 
    public GameObject shadowButton;
    public GameObject antiAliasingButton;
    public GameObject exitPauseButton; 
    public GameObject exitSettingsButton; 
    public GameObject mainMenuButton; 
    public GameObject[] mainMenuButtons; 
    private bool isSoundOn; 
    private bool areShadowsOn; 
     private int antiAliasingLevel;

    private void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        areShadowsOn = PlayerPrefs.GetInt("ShadowsOn", 1) == 1;
        antiAliasingLevel = PlayerPrefs.GetInt("AntiAliasingLevel", 4);


        UpdateSoundButtonText();
        UpdateShadowButtonText();
        UpdateAntiAliasingButtonText();
        AudioListener.volume = isSoundOn ? 1 : 0;
        SetShadows(areShadowsOn);
        SetAntiAliasing(antiAliasingLevel);
    }

    public void OpenPauseMenu()
    {
        Debug.Log("Opening pause menu");
        pauseMenu.SetActive(true);
        SetMainMenuButtonsActive(false);
        Time.timeScale = 0; 
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        SetMainMenuButtonsActive(true);
        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        Debug.Log("Opening settings menu");
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false); 
        SetMainMenuButtonsActive(false); 
        Time.timeScale = 0; 
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true); 
        SetMainMenuButtonsActive(false); 
        Time.timeScale = 0; 
    }

    public void MainMenu()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("MainMenu");
    }
    public void ToggleAntiAliasing()
    {
        antiAliasingLevel = antiAliasingLevel switch
        {
            0 => 2,
            2 => 4,
            4 => 8,
            _ => 0
        };
        
        PlayerPrefs.SetInt("AntiAliasingLevel", antiAliasingLevel);
        UpdateAntiAliasingButtonText();
        SetAntiAliasing(antiAliasingLevel);
    }
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        UpdateSoundButtonText();
        AudioListener.volume = isSoundOn ? 1 : 0;
    }

    public void ToggleShadows()
    {
        areShadowsOn = !areShadowsOn;
        PlayerPrefs.SetInt("ShadowsOn", areShadowsOn ? 1 : 0);
        UpdateShadowButtonText();
        SetShadows(areShadowsOn);
    }

    private void UpdateSoundButtonText()
    {
        if (soundButton != null)
        {
            TMP_Text buttonText = soundButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = isSoundOn ? "SOUND: ON" : "SOUND: OFF";
            }
        }
    }
private void UpdateAntiAliasingButtonText()
    {
        if (antiAliasingButton != null)
        {
            TMP_Text buttonText = antiAliasingButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = $"ANTIALIASING: {antiAliasingLevel}x";
            }
        }
    }
    private void UpdateShadowButtonText()
    {
        if (shadowButton != null)
        {
            TMP_Text buttonText = shadowButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = areShadowsOn ? "SHADOWS: ON" : "SHADOWS: OFF";
            }
        }
    }

    private void SetShadows(bool enable)
    {
        QualitySettings.shadows = enable ? ShadowQuality.All : ShadowQuality.Disable;
    }
private void SetAntiAliasing(int level)
    {
        QualitySettings.antiAliasing = level;
    }
    private void SetMainMenuButtonsActive(bool isActive)
    {
        foreach (GameObject button in mainMenuButtons)
        {
            button.SetActive(isActive);
        }
    }
}
