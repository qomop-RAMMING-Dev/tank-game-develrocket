using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 30;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 포탄을 정면 방향으로 발사
        rb.linearVelocity = transform.forward * speed;

        // 일정 시간이 지나면 포탄 제거
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 충돌할 경우 데미지를 주지 않음
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            return;
        }

        // 적 탱크와 충돌할 경우
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // TODO: EnemyTankController를 통해 데미지 적용
            // EnemyTankController enemy =
            //    collision.gameObject.GetComponent<EnemyTankController>();

            // if (enemy != null)
            //    enemy.TakeDamage(damage);
        }

        // 충돌한 대상이 무엇이든 포탄 제거
        Destroy(gameObject);
    }
}
