using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject stageClearPanel;
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text clearTimeText;

    private void Awake()
    {
        // 게임 시작 시 클리어 패널 비활성화
        stageClearPanel.SetActive(false);
    }

    // 스테이지 클리어 시 UI 표시
    public void ShowStageClear(int stage, string clearTime)
    {
        stageClearPanel.SetActive(true);

        stageText.text = $"STAGE {stage}";
        clearTimeText.text = clearTime;
    }

    // 스테이지 재시작
    public void RetryStage()
    {
        Time.timeScale = 1f;

        EnemyAlertSystem.ResetAlert();

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
            );
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("TitleScene");
    }

}
