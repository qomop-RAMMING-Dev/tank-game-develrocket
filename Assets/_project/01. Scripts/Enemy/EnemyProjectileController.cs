using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectileController : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 20;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 포탄의 정면 방향으로 발사
        rb.linearVelocity = transform.forward * speed;

        // 일정 시간이 지나면 포탄 제거
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 충돌했는지 확인
        if (collision.transform.CompareTag("Player") ||
            collision.transform.root.CompareTag("Player"))
        {
            // 플레이어 체력 스크립트 탐색
            PlayerTankController player =
                collision.transform.GetComponent<PlayerTankController>();

            // 플레이어에게 데미지 전달
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("적 포탄이 플레이어에게 명중!");
            }
        }

        // 충돌한 포탄 제거
        Destroy(gameObject);
    }
}
