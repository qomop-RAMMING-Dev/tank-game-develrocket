using UnityEngine;

public class GoalPointController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 GoalPoint에 진입했는지 확인
        if (!other.transform.root.CompareTag("Player"))
            return;

        // GameManager가 존재하는지 확인
        if (GameManager.Instance == null)
            return;

        // 스테이지 클리어 처리
        GameManager.Instance.StageClear();
    }
}
