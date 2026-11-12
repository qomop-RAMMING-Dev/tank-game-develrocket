using UnityEngine;

public class EnemyTankController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float maxHp = 100f;

    private float currentHp;
    private bool isDead = false;

    private void Awake()
    {
        // 시작할 때 최대 체력 초기화
        currentHp = maxHp;
    }

    // 외부에서 적 탱크에 데미지를 전달하는 함수
    public void TakeDamage(int damage)
    {
        // 이미 사망했거나 잘못된 데미지인 경우 무시
        if (isDead || damage <= 0) return;

        // 체력 감소
        currentHp -= damage;

        // 체력이 음수가 되지 않도록
        currentHp = Mathf.Max(currentHp, 0f);

        Debug.Log($"{gameObject.name} 체력: {currentHp}/{maxHp}");

        // 체력이 0이면 사망 처리
        if (currentHp <= 0f)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} 파괴!");

        // 적 탱크 제거
        Destroy(gameObject);
    }
}
