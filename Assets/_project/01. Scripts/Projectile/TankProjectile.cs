using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifeTime = 8f;
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

        // 플레이어 탱크와의 충돌 무시
        IgnorePlayerCollision();

        // 일정 시간이 지나면 포탄 제거
        Destroy(gameObject, lifeTime);
    }

    private void IgnorePlayerCollision()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) return;

        Collider[] projectileColliders =
            GetComponentsInChildren<Collider>();

        Collider[] playerColliders =
            player.GetComponentsInChildren<Collider>();

        foreach (Collider projectileCollider in projectileColliders)
        {
            foreach (Collider playerCollider in playerColliders)
            {
                Physics.IgnoreCollision(
                    projectileCollider,
                    playerCollider
                );
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 충돌할 경우 데미지를 주지 않음
        if (collision.gameObject.CompareTag("Player") ||
            collision.transform.root.CompareTag("Player"))
        {
            // Destroy(gameObject);
            return;
        }

        // 적 탱크와 충돌했는지 확인
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.transform.root.CompareTag("Enemy"))
        {
            // 충돌한 오브젝트 또는 부모에서 공통 체력 스크립트 탐색
            EnemyTankController enemy =
                collision.transform.GetComponentInParent<EnemyTankController>();

            // 적 탱크를 찾았다면 데미지 전달
            if (enemy != null)
                enemy.TakeDamage(damage);
        }

        Debug.Log($"포탄 충돌: {collision.gameObject.name}");

        // 충돌한 대상이 무엇이든 포탄 제거
        Destroy(gameObject);
    }
}
