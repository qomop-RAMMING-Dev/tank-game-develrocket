using UnityEngine;

public class CombatTankAIController : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float detectionAngle = 90f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float attackRange = 10f;

    [Header("Attack Settings")]
    [SerializeField] private float fireAngle = 3f;

    [Header("Controller References")]
    [SerializeField] private CombatTankController combatController;
    [SerializeField] private EnemyTankFireController fireController;

    private Rigidbody rb;
    private Transform player;

    private bool hasTarget = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 플레이어 오브젝트 탐색
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        // 필요한 컴포넌트 자동 연결
        if (combatController == null)
            combatController = GetComponent<CombatTankController>();

        if (fireController == null)
            fireController = GetComponent<EnemyTankFireController>();
    }

    private void FixedUpdate()
    {
        if (player == null ||
            combatController == null ||
            fireController == null) return;

        if (!hasTarget)
            DetectPlayer();
        else
            ChaseAndAttack();
    }

    // 플레이어 발견
    private void DetectPlayer()
    {
        Vector3 direction = player.position - rb.position;
        direction.y = 0f;

        // 감지 범위 확인
        if (direction.magnitude > detectionRange)
            return;

        // 시야각 확인
        float angle = Vector3.Angle(
            transform.forward,
            direction
            );

        if (angle <= detectionAngle)
        {
            hasTarget = true;

            // 기존 순찰 중지
            combatController.SetPatrolling(false);

            Debug.Log("CombatTank: 플레이어 발견!");
        }
    }

    // 플레이어 추적 및 공격
    private void ChaseAndAttack()
    {
        Vector3 direction = player.position - rb.position;
        direction.y = 0f;

        // 플레이어가 감지 범위를 벗어나면 순찰 재개
        if (direction.magnitude > detectionRange)
        {
            hasTarget = false;
            combatController.SetPatrolling(true);

            Debug.Log("CombatTank: 플레이어 탐지 해제");

            return;
        }

        if (direction.sqrMagnitude < 0.001f)
            return;

        // 플레이어 방향으로 회전
        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        Quaternion nextRotation = Quaternion.RotateTowards(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
            );

        rb.MoveRotation(nextRotation);

        // 공격 범위 밖이면 추적
        if (direction.magnitude > attackRange)
        {
            Vector3 moveDirection =
                nextRotation * Vector3.forward;

            Vector3 nextPosition = rb.position +
                moveDirection * chaseSpeed * Time.fixedDeltaTime;

            rb.MovePosition(nextPosition);

            return;
        }

        // 공격 범위 안이면 조준 후 발사
        float angle = Quaternion.Angle(
            nextRotation,
            targetRotation
            );

        if (angle <= fireAngle)
            fireController.Fire(player);
    }
}
