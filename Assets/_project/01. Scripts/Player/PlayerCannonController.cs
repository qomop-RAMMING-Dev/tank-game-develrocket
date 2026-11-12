using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerCannonController : MonoBehaviour
{
    [Header("Cannon Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform muzzle;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource fireAudioSrc;
    [SerializeField] private AudioClip fireClip;

    [Header("Fire Settings")]
    [SerializeField] private float fireInterval = 1f;

    private float nextFireTime = 1f;

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        // 키보드 입력 장치가 없으면 종료
        if (keyboard == null) return;

        // Space 키를 처음 누른 순간 발사
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Fire();
        }
    }

    private void Fire()
    {
        // 필수 오브젝트가 연결되지 않았다면 발사하지 않음.
        if (projectilePrefab == null || muzzle == null) return;

        // 연사 간격 확인
        if (Time.time < nextFireTime) return;

        // 다음 발사 가능 시간 설정
        nextFireTime = Time.time + fireInterval;

        // 포탄 생성
        Instantiate(
            projectilePrefab,
            muzzle.position,
            muzzle.rotation
            );

        // 발사 사운드 재생
        if (fireAudioSrc != null && fireClip != null)
        {
            fireAudioSrc.PlayOneShot(fireClip);
            Debug.Log("발사 사운드 재생 요청!");
        }
        else
        {
            Debug.LogWarning("발사 AudioSource 또는 AudioClip이 연결되지 않았습니다.");
        }
            
    }
}
