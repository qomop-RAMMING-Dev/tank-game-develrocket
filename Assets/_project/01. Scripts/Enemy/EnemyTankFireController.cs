using UnityEngine;

public class EnemyTankFireController : MonoBehaviour
{
    [Header("Cannon Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform muzzle;

    [Header("Fire Settings")]
    [SerializeField] private float fireInterval = 2f;

    private float nextFireTime;

    // 외부 AI에서 호출할 적 탱크 발사 함수
    public void Fire()
    {
        // 포탄 프리팹과 포구가 설정되어 있는지 확인
        if (projectilePrefab == null || muzzle == null)
        {
            Debug.LogWarning("적 탱크 포탄 또는 포구가 설정되지 않았습니다.");
            return;
        }

        // 발사 간격 확인
        if (Time.time < nextFireTime) return;

        // 다음 발사 가능 시간 설정
        nextFireTime = Time.time + fireInterval;

        // 포구의 정면 방향을 기준으로 회전값 설정
        Quaternion fireRotation =
            Quaternion.LookRotation(muzzle.forward, Vector3.up);

        // 포탄 생성
        GameObject projectile = Instantiate(
            projectilePrefab,
            muzzle.position,
            fireRotation
            );

        // 발사한 탱크와 포탄의 충돌 무시
        Collider[] tankColliders =
            GetComponentsInChildren<Collider>();

        Collider[] projectileColliders =
            projectile.GetComponentsInChildren<Collider>();

        foreach (Collider tankCollider in tankColliders)
        {
            foreach (Collider projectileCollider in projectileColliders)
            {
                Physics.IgnoreCollision(
                    tankCollider,
                    projectileCollider
                    );
            }
        }

        Debug.Log($"{gameObject.name} 포탄 발사!");
    }
}
