using UnityEngine;

public class StageTimer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isRunning = false;

    public float ElapsedTime => elapsedTime;

    // 스테이지 시간 측정 시작
    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;

        Debug.Log("스테이지 타이머 시작!");
    }

    // 스테이지 시간 측정 중지
    public void StopTimer()
    {
        isRunning = false;

        Debug.Log($"스테이지 클리어 시간: {GetFormattedTime()}");
    }

    private void Update()
    {
        if (!isRunning)
            return;

        elapsedTime += Time.unscaledDeltaTime;
    }

    // 시간을 MM:SS 형식으로 반환
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        return $"{minutes:00}:{seconds:00}";
    }
}
