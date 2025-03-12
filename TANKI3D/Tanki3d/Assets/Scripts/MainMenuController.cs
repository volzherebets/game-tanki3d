using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject keMenu;
    public GameObject soundButton;
    public GameObject shadowButton;
    public GameObject antiAliasingButton;
    public GameObject languageButton;
    public GameObject[] mainMenuButtons;
    public GameObject themeSelectionPanel;

    private bool isSoundOn;
    private bool areShadowsOn;
    private int antiAliasingLevel;
    private bool isUkrainian;

    // Референс до таблиці для локалізації
    private string tableReference = "UI_TEXT";

    public static string selectedTheme = "HALLOWEEN";

    private void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        areShadowsOn = PlayerPrefs.GetInt("ShadowsOn", 1) == 1;
        antiAliasingLevel = PlayerPrefs.GetInt("AntiAliasingLevel", 4);
        isUkrainian = PlayerPrefs.GetInt("SelectedLanguage", 0) == 1;

        // Встановлюємо мову
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[isUkrainian ? 1 : 0];

        // Підписуємось на зміну мови, щоб оновити весь UI при зміні
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        UpdateAllButtonTexts();

        AudioListener.volume = isSoundOn ? 1 : 0;
        SetShadows(areShadowsOn);
        SetAntiAliasing(antiAliasingLevel);
    }

    private void OnDestroy()
    {
        // Відписуємось від події при знищенні об'єкта
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale locale)
    {
        // Оновлюємо всі тексти при зміні мови
        UpdateAllButtonTexts();
    }

    private void UpdateAllButtonTexts()
    {
        UpdateSoundButtonText();
        UpdateShadowButtonText();
        UpdateAntiAliasingButtonText();
        UpdateLanguageButtonText();
    }

    public void ToggleLanguage()
    {
        isUkrainian = !isUkrainian;
        int languageIndex = isUkrainian ? 1 : 0;

        // Міняємо мову у LocalizationSettings
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];

        // Зберігаємо вибір
        PlayerPrefs.SetInt("SelectedLanguage", languageIndex);
        PlayerPrefs.Save();
    }

    private void UpdateLanguageButtonText()
    {
        if (languageButton != null)
        {
            TMP_Text buttonText = languageButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                string key = isUkrainian ? "LANGUAGE_UKRAINIAN" : "LANGUAGE_ENGLISH";
                buttonText.text = GetLocalizedString(key);
            }
        }
    }

    public void PlayGame()
    {
        themeSelectionPanel.SetActive(true);
        SetMainMenuButtonsActive(false);
    }

    public void CloseThemeSelectionPanel()
    {
        themeSelectionPanel.SetActive(false);
        SetMainMenuButtonsActive(true);
    }

    public void SelectTheme(string theme)
    {
        selectedTheme = theme;
        themeSelectionPanel.SetActive(false);

        LoadRandomLevelByTheme();
    }

    private void LoadRandomLevelByTheme()
    {
        string[] levelNames = selectedTheme.ToLower() == "halloween" ? new string[] { "halloween", "halloween 1", "halloween 2"} :
                            selectedTheme.ToLower() == "winter" ? new string[] { "winter", "winter 1", "winter 2"} :
                            selectedTheme.ToLower() == "forest" ? new string[] { "forest", "forest 1", "forest 2"} :
                            new string[0];

        if (levelNames.Length > 0)
        {
            string randomLevel = levelNames[Random.Range(0, levelNames.Length)];
            SceneManager.LoadScene(randomLevel);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Вихід з гри");
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        SetMainMenuButtonsActive(false);
    }

    public void CloseSettings()
{
    settingsMenu.SetActive(false); // Було keMenu, має бути settingsMenu
    SetMainMenuButtonsActive(true);
}

    public void OpenKe()
    {
        keMenu.SetActive(true);
        SetMainMenuButtonsActive(false);
    }

    public void CloseKE()
{
    keMenu.SetActive(false);  
    SetMainMenuButtonsActive(true);  
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

    private void UpdateAntiAliasingButtonText()
    {
        if (antiAliasingButton != null)
        {
            TMP_Text buttonText = antiAliasingButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                string key = "ANTIALIASING_FORMAT";
                string format = GetLocalizedString(key);
                buttonText.text = string.Format(format, antiAliasingLevel);
            }
        }
    }

    // Допоміжний метод для отримання локалізованого рядка
    private string GetLocalizedString(string key)
    {
        // Створюємо локалізований рядок
        var localizedString = new LocalizedString(tableReference, key);
        
        // Отримуємо поточне значення
        return localizedString.GetLocalizedString();
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