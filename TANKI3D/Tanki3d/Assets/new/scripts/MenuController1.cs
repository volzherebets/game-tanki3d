using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu; // Панель паузи
    public GameObject settingsMenu; // Панель налаштувань
    public GameObject soundButton; // Кнопка звуку
    public GameObject shadowButton; // Кнопка тіней
    public GameObject antiAliasingButton;
    public GameObject exitPauseButton; // Кнопка виходу з паузи
    public GameObject exitSettingsButton; // Кнопка виходу з налаштувань
    public GameObject mainMenuButton; // Кнопка повернення в головне меню
    public GameObject[] mainMenuButtons; // Масив кнопок головного меню
    private bool isSoundOn; // Стан звуку (ввімкнено або вимкнено)
    private bool areShadowsOn; // Стан тіней (ввімкнено або вимкнено)
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

    // Відкриває меню паузи
    public void OpenPauseMenu()
    {
        Debug.Log("Opening pause menu");
        pauseMenu.SetActive(true);
        SetMainMenuButtonsActive(false);
        Time.timeScale = 0; // Ставимо гру на паузу
    }

    // Закриває меню паузи
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        SetMainMenuButtonsActive(true);
        Time.timeScale = 1; // Відновлюємо гру
    }

    // Відкриває меню налаштувань
    public void OpenSettings()
    {
        Debug.Log("Opening settings menu");
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false); // Закриваємо меню паузи при відкритті налаштувань
        SetMainMenuButtonsActive(false); // Опціонально приховати кнопки головного меню
        Time.timeScale = 0; // Ставимо гру на паузу
    }

    // Закриває меню налаштувань і повертається до меню паузи
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true); // Повертаємо меню паузи
        SetMainMenuButtonsActive(false); // Приховуємо кнопки головного меню, якщо треба
        Time.timeScale = 0; // Гра на паузі поки меню не закрили
    }

    // Перехід в головне меню
    public void MainMenu()
    {
        Time.timeScale = 1; // Відновлюємо час перед переходом
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
    // Перемикання звуку
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        UpdateSoundButtonText();
        AudioListener.volume = isSoundOn ? 1 : 0;
    }

    // Перемикання тіней
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
