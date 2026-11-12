using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 게임 상태
    public enum GameState
    {
        Playing, // 게임 진행 중
        GameOver, // 게임 오버
        StageClear, // 스테이지 클리어
        GameClear // 전체 게임 클리어
    }

    [Header("Game Settings")]
    [SerializeField] private int maxStage = 3;
    [SerializeField] private int currentStage = 1;

    // 현재 게임 상태
    private GameState currentState = GameState.Playing;

    // 외부에서 현재 상태 확인
    public GameState CurrentState => currentState;

    // 현재 스테이지 확인
    public int CurrentStage => currentStage;

    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // 중복 생성 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // 게임 시작
    public void StartGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;

        Debug.Log($"Stage {currentStage} 시작!");
    }

    // 게임 오버
    public void GameOver()
    {
        if (currentState != GameState.Playing)
            return;

        currentState = GameState.GameOver;

        Debug.Log("GAME OVER");

        Time.timeScale = 0f;
    }

    // 스테이지 클리어
    public void StageClear()
    {
        if (currentState != GameState.Playing)
            return;

        currentState = GameState.StageClear;

        Debug.Log($"Stage {currentStage} 클리어!");

        Time.timeScale = 0f;
    }

    // 다음 스테이지 진행
    public void NextStage()
    {
        if (currentStage >= maxStage)
        {
            GameClear();
            return;
        }

        currentStage++;

        Time.timeScale = 1f;
        currentState = GameState.Playing;

        Debug.Log($"Stage {currentStage} 시작!");
    }

    // 전체 게임 클리어
    private void GameClear()
    {
        currentState = GameState.GameClear;

        Debug.Log("GAME CLEAR");

        Time.timeScale = 0f;
    }
}
