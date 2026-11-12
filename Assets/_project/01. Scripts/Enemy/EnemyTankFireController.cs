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
    // 기본 방식: 포탄 정면으로 발사
    // GuardTank 등 기존 호출과의 호환성을 유지
    public void Fire()
    {
        if (muzzle == null)
            return;

        FireInDirection(muzzle.forward);
    }

    // 목표물을 지정하여 발사
    public void Fire(Transform target)
    {
        if (muzzle == null || target == null)
            return;

        // 플레이어의 Collider 중심을 조준 지점으로 사용
        Collider targetCollider =
            target.GetComponentInChildren<Collider>();

        Vector3 targetPosition = targetCollider != null
            ? targetCollider.bounds.center
            : target.position + Vector3.up;

        // 포구 위치에서 플레이어 중심까지의 방향
        Vector3 direction =
            targetPosition - muzzle.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        FireInDirection(direction.normalized);
    }

    // 실제 포탄 생성 및 발사
    private void FireInDirection(Vector3 direction)
    {
        if (projectilePrefab == null || muzzle == null)
            return;

        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireInterval;

        Quaternion fireRotation =
            Quaternion.LookRotation(direction, Vector3.up);

        GameObject projectile = Instantiate(
            projectilePrefab,
            muzzle.position,
            fireRotation
            );

        // 발사한 탱크와 자신의 포탄 충돌 방지
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
