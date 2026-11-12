using UnityEngine;

public class GuardTankController : MonoBehaviour
{
    [Header("Guard Settings")]
    [SerializeField] private float rotationAngle = 60f;
    [SerializeField] private float rotationSpeed = 30f;

    private float startRotationY;
    private bool rotatingRight = true;
    private bool isScanning = true;

    private void Start()
    {
        // 시작할 때의 Y축 회전값 저장
        startRotationY = transform.eulerAngles.y;
    }

    private void Update()
    {
        // 감시 상태가 아닐 때 좌우 회전하지 않음.
        if (!isScanning) return;
        RotateGuardTank();
    }

    private void RotateGuardTank()
    {
        // 좌우 회전 범위 계산
        float targetAngle = rotatingRight
            ? startRotationY + rotationAngle
            : startRotationY - rotationAngle;

        // 현재 회전값에서 목표 방향으로 부드럽게 회전
        Quaternion targetRotation = Quaternion.Euler(
            0f,
            targetAngle,
            0f
            );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
            );

        // 목표 회전에 도달하면 회전 방향 변경
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            rotatingRight = !rotatingRight;
    }

    // AI에서 감시 상태를 변경할 때 호출
    public void SetScanning(bool scanning)
    {
        isScanning = scanning;

        // 감시를 다시 시작하면 현재 방향을 기준으로 회전
        if (isScanning)
        {
            startRotationY = transform.eulerAngles.y;
            rotatingRight = true;
        }
    }
}
