    using UnityEngine;
    using UnityEngine.SceneManagement;
    using TMPro;

    public class MainMenuController : MonoBehaviour
    {
        public GameObject settingsMenu;
        public GameObject soundButton;
        public GameObject shadowButton;
        public GameObject antiAliasingButton;
        public GameObject[] mainMenuButtons;
        public GameObject themeSelectionPanel;

        private bool isSoundOn;
        private bool areShadowsOn;
        private int antiAliasingLevel;

        public static string selectedTheme = "HALLOWEEN";

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
            string[] levelNames = selectedTheme == "HALLOWEEN" ? new string[] { "LVL1H", "LVL2H"} :
                                selectedTheme == "NEWYEAR" ? new string[] { "LVL1N", "LVL2N"} :
                                selectedTheme == "STANDART" ? new string[] { "LVL1N", "LVL2N"} :
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
            settingsMenu.SetActive(false);
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
                    buttonText.text = isSoundOn ? "SOUND: ON" : "SOUND: OFF";
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
