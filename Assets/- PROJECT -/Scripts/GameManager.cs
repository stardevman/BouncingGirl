using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GetScript Get;

    [Header("About UI")]
    public GameObject menuPanel;
    public GameObject gamingPanel;
    public GameObject completePanel;
    public GameObject failedPanel;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    private int level;

    void Start() {
        Application.targetFrameRate = 60;

        level = PlayerPrefs.GetInt("Level");
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();

        menuPanel.SetActive(true);
        gamingPanel.SetActive(true);
    }

    public void CompleteLevel() {
        completePanel.SetActive(true);
    }

    public void FailedLevel() {
        failedPanel.SetActive(true);
    }

    public void StartGame() {
        menuPanel.SetActive(false);
    }

    public void NextLevel() {
        if (level >= 5)
        {
            level = Random.Range(1, 6);
            PlayerPrefs.SetInt("RandomLevel", level);
        }
        else
            level += 1;

        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(level);
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
