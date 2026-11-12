using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ReconTankController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float arrivalDistance = 0.5f;

    private Rigidbody rb;

    private int currentWaypointIndex = 0;
    private bool isPatrolling = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isPatrolling) return;

        // 웨이포인트가 설정되지 않았다면 이동하지 않음
        if (waypoints == null || waypoints.Length == 0) 
            return;

        // 현재 목표 웨이포인트 가져오기
        Transform target = waypoints[currentWaypointIndex];

        if (target == null) return;

        // 목표 지점까지의 방향 계산
        Vector3 direction = target.position - rb.position;

        // Y축 높이 차이는 이동 방향에서 제외
        direction.y = 0f;

        // 목표 지점에 도착했는지 확인
        if (direction.magnitude <= arrivalDistance)
        {
            MoveToNextWaypoint();
            return;
        }

        // 이동 방향을 바라보도록 회전
        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        Quaternion nextRotation = Quaternion.RotateTowards(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
            );

        rb.MoveRotation(nextRotation);

        // 탱크 정면 방향으로 이동
        Vector3 moveDirection = nextRotation * Vector3.forward;

        Vector3 nextPosition = rb.position +
            moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);
    }

    private void MoveToNextWaypoint()
    {
        // 다음 웨이포인트로 이동
        currentWaypointIndex++;

        // 마지막 지점에 도달하면 처음으로 돌아감.
        if (currentWaypointIndex >= waypoints.Length)
            currentWaypointIndex = 0;
    }

    // 순찰 상태 변경
    public void SetPatrolling(bool value)
    {
        isPatrolling = value;
    }
}
