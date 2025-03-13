using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

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
    
    private string tableReference = "UI_TEXT";
    
    private void Awake()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }
    
    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        areShadowsOn = PlayerPrefs.GetInt("ShadowsOn", 1) == 1;
        antiAliasingLevel = PlayerPrefs.GetInt("AntiAliasingLevel", 4);

        UpdateAllButtonTexts();
        AudioListener.volume = isSoundOn ? 1 : 0;
        SetShadows(areShadowsOn);
        SetAntiAliasing(antiAliasingLevel);
    }
    
    private void OnLocaleChanged(Locale locale)
    {
        UpdateAllButtonTexts();
    }
    
    private string GetLocalizedString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(tableReference, key);
    }
    
    private void UpdateAllButtonTexts()
    {
        UpdateSoundButtonText();
        UpdateShadowButtonText();
        UpdateAntiAliasingButtonText();
        
        if (exitPauseButton != null)
        {
            TMP_Text buttonText = exitPauseButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = GetLocalizedString("EXIT_PAUSE");
            }
        }
        
        if (mainMenuButton != null)
        {
            TMP_Text buttonText = mainMenuButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = GetLocalizedString("MAIN_MENU");
            }
        }
    }

    public void OpenPauseMenu()
    {
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
                string key = isSoundOn ? "SOUND_ON" : "SOUND_OFF";
                buttonText.text = GetLocalizedString(key);
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
                string formatText = GetLocalizedString("ANTIALIASING_FORMAT");
                buttonText.text = string.Format(formatText, antiAliasingLevel);
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
                string key = areShadowsOn ? "SHADOWS_ON" : "SHADOWS_OFF";
                buttonText.text = GetLocalizedString(key);
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
    
    public void ToggleLanguage()
    {
        Locale currentLocale = LocalizationSettings.SelectedLocale;
        var locales = LocalizationSettings.AvailableLocales.Locales;
        int currentIndex = locales.IndexOf(currentLocale);
        int nextIndex = (currentIndex + 1) % locales.Count;
        LocalizationSettings.SelectedLocale = locales[nextIndex];
    }
}