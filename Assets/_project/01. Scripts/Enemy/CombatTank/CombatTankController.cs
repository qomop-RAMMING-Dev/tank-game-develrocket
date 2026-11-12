using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CombatTankController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float arrivalDistance = 0.5f;

    [Header("Surveillance Settings")]
    [SerializeField] private float scanAngle = 60f;
    [SerializeField] private float scanSpeed = 30f;

    private Rigidbody rb;

    private int currentWaypointIndex = 0;

    private bool isPatrolling = true;
    private bool isScanning = true;
    private bool scanningRight = true;

    private float startRotationY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startRotationY = transform.eulerAngles.y;
    }

    private void FixedUpdate()
    {
        if (!isPatrolling)
            return;

        if (waypoints == null || waypoints.Length == 0)
            return;

        Patrol();
    }

    // 웨이포인트 순찰
    private void Patrol()
    {
        Transform target = waypoints[currentWaypointIndex];

        if (target == null)
            return;

        Vector3 direction = target.position - rb.position;
        direction.y = 0f;

        // 목표 지점 도착 확인
        if (direction.magnitude <= arrivalDistance)
        {
            MoveToNextWaypoint();
            return;
        }

        // 이동 방향 계산
        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        Quaternion nextRotation = Quaternion.RotateTowards(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
            );

        rb.MoveRotation(nextRotation);

        // 탱크 전진
        Vector3 moveDirection = nextRotation * Vector3.forward;

        Vector3 nextPosition = rb.position +
            moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);
    }

    // 다음 웨이포인트 이동
    private void MoveToNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
            currentWaypointIndex = 0;
    }

    // 순찰 활성화 / 비활성화
    public void SetPatrolling (bool value)
    {
        isPatrolling = value;
    }

    // 감시 활성화 / 비활성화
    public void SetScanning (bool value)
    {
        isScanning = value;
    }

    // 현재 감시 각도 계산
    public float GetScanAngle()
    {
        return scanAngle;
    }
}
