using UnityEngine;
using UnityEngine.SceneManagement;

// 繊呪人 惟績 神獄 食採研 淫軒馬澗 惟績 古艦煽

public class GameManager : SingletonBehaviour<GameManager>
{
    [HideInInspector]
    public bool IsGameOver;// 惟績 神獄 雌殿

    [HideInInspector]
    public string Enemy = "Enemy";

    [HideInInspector]
    public string EffectiveRange = "EffectiveRange";

    [HideInInspector]
    public string Player = "Player";

    [HideInInspector]
    public string ImpactNormal = "ImpactNormal";

    [HideInInspector]
    public string DropBox = "DropBox";

    [System.Serializable]
    public class GameEndEvent : UnityEngine.Events.UnityEvent{ }

    [System.Serializable]
    public class ScoreChangeEvent : UnityEngine.Events.UnityEvent<int> { }


    // 『『『『『『『『『『『『『『『『『『『『『 Event Instance 『『『『『『『『『『『『『『『『『『『『『
    [HideInInspector]
    public GameEndEvent OnGameEnd = new GameEndEvent();

    [HideInInspector]
    public ScoreChangeEvent OnScoreChange = new ScoreChangeEvent();

    // 『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『『

    private bool isEnd = false;
    private int currentScore = 0;
    public int ScoreIncreaseAmount = 50;

    // 斗型 淫恵 
    public const int maxFlakCount = 4;
    public int flakCount;
    public bool[] IsFlakOnWorlds = new bool[maxFlakCount];
    public Vector3 RealTargetPos;

    [HideInInspector]
    public int CurrentScore
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;
            OnScoreChange.Invoke(currentScore);
        }
    }

    private void Update()
    {
        if (isEnd && Input.GetKeyDown(KeyCode.R))
        {
            Reset();
            SceneManager.LoadScene(0);
        }
    }

    public void AddScore()
    {
        CurrentScore += ScoreIncreaseAmount;
    }

    public void End()
    {
        isEnd = true;
        OnGameEnd.Invoke();
    }

    private void Reset()
    {
        currentScore = 0;
        isEnd = false;
    }


    public int calculateFlakCount()
    {
        flakCount = 0;
        for (int i = 0; i < maxFlakCount; i++)
        {
            if (IsFlakOnWorlds[i] == true)
            {
                flakCount++;
            }
        }

        return flakCount;
    }
}