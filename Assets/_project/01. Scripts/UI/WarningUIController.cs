using System.Collections;
using TMPro;
using UnityEngine;

public class WarningUIController : MonoBehaviour
{
    [Header("Warning UI")]
    [SerializeField] private GameObject warningPanel;

    [Header("Warning Text")]
    [SerializeField] private TMP_Text warningTitleText;
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text subText;

    [Header("Display Settings")]
    [SerializeField] private float displayDuration = 3f;

    private Coroutine warningCoroutine;

    private void Awake()
    {
        if (warningPanel != null)
            warningPanel.SetActive(false);
    }

    private void OnEnable()
    {
        EnemyAlertSystem.OnAlertRaised += ShowWarning;

        if (EnemyAlertSystem.IsAlertActive)
            ShowWarning(EnemyAlertSystem.AlertPosition);
    }

    private void OnDisable()
    {
        EnemyAlertSystem.OnAlertRaised -= ShowWarning;
    }

    private void ShowWarning(Vector3 playerPosition)
    {
        if (warningPanel == null) return;

        warningPanel.SetActive(true);

        if (warningTitleText != null)
            warningTitleText.text = "경고!";

        if (mainText != null)
            mainText.text = "적에게 발각되었습니다!";

        if (subText != null)
            subText.text = "적 탱크가 플레이어를 추적하고 있습니다.";

        if (warningCoroutine != null)
            StopCoroutine(warningCoroutine);

        warningCoroutine = StartCoroutine(HideWarningAfterDelay());
    }

    private IEnumerator HideWarningAfterDelay()
    {
        yield return new WaitForSecondsRealtime(displayDuration);

        warningPanel.SetActive(false);
        warningCoroutine = null;
    }
}
