using UnityEngine;

public class GuardTankAIController : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float detectionAngle = 45f;

    [Header("Combat Settings")]
    [SerializeField] private float aimSpeed = 90f;
    [SerializeField] private float fireAngle = 3f;

    [SerializeField] private EnemyTankFireController fireController;
    [SerializeField] private GuardTankController guardController;

    private Transform player;
    private bool hasTarget = false;
    
    private void Start()
    {
        // Player 태그를 가진 오브젝트 탐색
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        // 필요한 컴포넌트 자동 탐색
        if (fireController == null)
            fireController = GetComponent<EnemyTankFireController>();

        if (guardController == null)
            guardController = GetComponent<GuardTankController>();
    }

    private void Update()
    {
        // 플레이어 또는 발사 컴포넌트가 없으면 실행하지 않음.
        if (player == null || 
            fireController == null ||
            guardController == null) return;

        // 플레이어 발견 여부에 따라 행동 구분
        if (!hasTarget) DetectPlayer();
        else AimAndAttack();
    }

    private void DetectPlayer()
    {
        // 적 탱크와 플레이어 사이의 방향 계산
        Vector3 direction = player.position - transform.position;

        // 높이 차이는 탐지 방향에서 제외
        direction.y = 0f;

        // 탐지 거리 확인
        if (direction.magnitude > detectionRange) return;

        // 탱크 정면과 플레이어 방향 사이의 각도 확인
        float angle = Vector3.Angle(transform.forward, direction);

        // 시야각 안에 플레이어가 있다면 공격
        if (angle <= detectionAngle)
        {
            hasTarget = true;

            // 경보 발생
            EnemyAlertSystem.RaiseAlert(player.position);

            // 기존 좌우 감시 회전 중단
            guardController.SetScanning(false);

            Debug.Log("GuardTank: 플레이어 발견!");
        }
    }

    private void AimAndAttack()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        // 플레이어가 탐지 범위 밖으로 나가면 감시 재개
        if (direction.magnitude > detectionRange)
        {
            hasTarget = false;
            guardController.SetScanning(true);

            Debug.Log("GuardTank: 플레이어 탐지 해제");

            return;
        }

        if (direction.sqrMagnitude < 0.001f) return;

        // 플레이어 방향으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(
            direction.normalized
            );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            aimSpeed * Time.deltaTime
            );

        // 목표 방향과 거의 일치하면 방향
        float angle = Quaternion.Angle(
            transform.rotation,
            targetRotation
            );

        if (angle <= fireAngle)
            fireController.Fire();
    }
}
