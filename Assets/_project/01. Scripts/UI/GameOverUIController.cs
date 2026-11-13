using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text survivalTimeText;

    private void Awake()
    {
        // 게임 시작 시 GameOver 패널 비활성화
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int stage, string survivalTime)
    {
        gameOverPanel.SetActive(true);

        stageText.text = $"STAGE {stage}";
        survivalTimeText.text = survivalTime;
    }

    public void RetryStage()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
            );
    }
}
