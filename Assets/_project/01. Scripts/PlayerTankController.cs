using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTankController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource moveAudioSrc;
    [SerializeField] private AudioClip idleClip;
    [SerializeField] private AudioClip drivingClip;

    [Header("Player Settings")]
    [SerializeField] private float playerHp = 100f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 60f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 탱크 이동
        Vector3 move = transform.forward
            * moveSpeed
            * moveInput.y
            * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);

        // 탱크 회전
        float angle = moveInput.x
            * turnSpeed
            * Time.fixedDeltaTime;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, angle, 0f));
    }

    private void Update()
    {
        // 키보드 입력 초기화
        moveInput = Vector2.zero;

        Keyboard keyboard = Keyboard.current;

        // 키보드가 연결되어 있지 않다면 입력 처리 종료
        if (keyboard == null)
        {
            UpdateMovementAudio();
            return;
        }

        // 전진 및 후진 입력
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            moveInput.y = 1f;

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            moveInput.y = -1f;

        // 좌회전 및 우회전 입력
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            moveInput.x = -1f;

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            moveInput.x = 1f;

        UpdateMovementAudio();
    }

    private void UpdateMovementAudio()
    {
        // AudioSource가 연결되지 않았다면 처리하지 않음.
        if (moveAudioSrc == null) return;

        // 이동 입력에 따라 재생할 오디오 클립 선택
        AudioClip targetClip = (moveInput.x != 0 || moveInput.y != 0)
            ? drivingClip
            : idleClip;

        // 재생할 클립이 없다면 기존 사운드 정지
        if (targetClip == null)
        {
            moveAudioSrc.Stop();
            return;
        }

        // 현재 클립과 다를 때만 변경 및 재생
        if (moveAudioSrc.clip != targetClip)
        {
            moveAudioSrc.clip = targetClip;
            moveAudioSrc.loop = true;
            moveAudioSrc.Play();
        }
    }

    public void TakeDamage(int damage)
    {
        // 사망 상태이거나 유효하지 않은 데미지라면 무시
        if (playerHp <= 0f || damage <= 0) return;

        playerHp -= damage;
        playerHp = Mathf.Max(playerHp, 0f);

        Debug.Log($"플레이어 체력: {playerHp}");

        if (playerHp <= 0f)
        {
            Debug.LogWarning("플레이어 사망");
            // TODO: GameManager에 게임 오버 요청
        }
    }
}
