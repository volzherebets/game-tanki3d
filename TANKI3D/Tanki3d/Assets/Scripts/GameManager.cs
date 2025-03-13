using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class GameManager : MonoBehaviour
{
    public GameObject tank1;
    public GameObject tank2;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI scoreTextTank1;
    public TextMeshProUGUI scoreTextTank2;

    private bool gameStarted = false;
    private float countdownTime = 3f;

    void Start()
    {
        DisableTanks();
        StartCoroutine(StartCountdown());
        UpdateScoreUI();
    }

    void Update()
    {
        if (gameStarted && (tank1 == null || tank2 == null))
        {
            StartCoroutine(LoadRandomLevelWithDelay());
        }
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreTextTank1 != null)
        {
            scoreTextTank1.text = $"{ScoreManager.Tank1Score}";
        }
        if (scoreTextTank2 != null)
        {
            scoreTextTank2.text = $"{ScoreManager.Tank2Score}";
        }
    }

    IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString("0");
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownText.text = GetLocalizedString("FIGHT");
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        gameStarted = true;

        EnableTanks();
    }

    IEnumerator LoadRandomLevelWithDelay()
    {
        yield return new WaitForSeconds(3f);
        LoadRandomLevelByTheme();
    }

    private void LoadRandomLevelByTheme()
    {
        string[] levelNames = MainMenuController.selectedTheme == "halloween" ? new string[] { "halloween", "halloween 1", "halloween 2"} :
                              MainMenuController.selectedTheme == "winter" ? new string[] { "winter", "winter 1", "winter 2"} :
                              MainMenuController.selectedTheme == "forest" ? new string[] { "forest", "forest 1", "forest 2"} :
                              new string[0];

        if (levelNames.Length > 0)
        {
            string currentLevel = SceneManager.GetActiveScene().name;
            string randomLevel;

            do
            {
                randomLevel = levelNames[Random.Range(0, levelNames.Length)];
            }
            while (randomLevel == currentLevel);

            SceneManager.LoadScene(randomLevel);
        }
    }

    void DisableTanks()
    {
        if (tank1 != null)
        {
            tank1.GetComponent<TankController>().enabled = false;
        }
        if (tank2 != null)
        {
            tank2.GetComponent<TankController>().enabled = false;
        }
    }

    void EnableTanks()
    {
        if (tank1 != null)
        {
            tank1.GetComponent<TankController>().enabled = true;
        }
        if (tank2 != null)
        {
            tank2.GetComponent<TankController>().enabled = true;
        }
    }

    private string GetLocalizedString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString("UI_TEXT", key);
    }
}
