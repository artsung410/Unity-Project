using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ���� ���� ���θ� �����ϴ� ���� �Ŵ���

public class GameManager : SingletonBehaviour<GameManager>
{
    [HideInInspector]
    public bool IsGameOver;// ���� ���� ����

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


    // �ءءءءءءءءءءءءءءءءءءءء� Event Instance �ءءءءءءءءءءءءءءءءءءءء�
    [HideInInspector]
    public GameEndEvent OnGameEnd = new GameEndEvent();

    [HideInInspector]
    public ScoreChangeEvent OnScoreChange = new ScoreChangeEvent();

    // �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

    private bool isEnd = false;

    private int currentScore = 0;

    public int ScoreIncreaseAmount = 50;


    // �ͷ� ���� 
    const int maxFlakCount = 4;
    public bool[] IsAutoTurretOnWorld = new bool[maxFlakCount];
    public int FlakCount = 0;
    public Transform realTimeTarget;

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
}