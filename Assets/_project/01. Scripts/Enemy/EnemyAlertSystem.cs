using System;
using UnityEngine;

public class EnemyAlertSystem : MonoBehaviour
{
    // 경보 발생 여부
    public static bool IsAlertActive { get; private set; }

    // 경보 발생 시 전달할 플레이어 위치
    public static Vector3 AlertPosition { get; private set; }

    // 경보 발생 이벤트
    public static event Action<Vector3> OnAlertRaised;

    // 경보 발생
    public static void RaiseAlert(Vector3 playerPosition)
    {
        // 이미 경보가 발생했다면 중복 실행하지 않음.
        if (IsAlertActive) return;

        IsAlertActive = true;
        AlertPosition = playerPosition;

        Debug.Log("EnemyAlertSystem; 적 전체 경보 발생!");

        OnAlertRaised?.Invoke(playerPosition);
    }

    // 경보 초기화
    public static void ResetAlert()
    {
        IsAlertActive = false;
        AlertPosition = Vector3.zero;

        Debug.Log("EnemyAlertSystem: 경보 초기화 완료!");
    }
}
