using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject tank1;
    public GameObject tank2;
    public TextMeshProUGUI countdownText;

    private bool gameStarted = false;
    private float countdownTime = 3f;

    void Start()
    {
        DisableTanks();
        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        if (gameStarted && (tank1 == null || tank2 == null))
        {
            StartCoroutine(LoadRandomLevelWithDelay());
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

        countdownText.text = "FIGHT!";
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
        string[] levelNames = MainMenuController.selectedTheme == "HALLOWEEN" ? new string[] { "LVL1H", "LVL2H"} :

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
}
