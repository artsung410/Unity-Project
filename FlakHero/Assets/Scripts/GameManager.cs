using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// ������ ���� ���� ���θ� �����ϴ� ���� �Ŵ���

public class GameManager : SingletonBehaviour<GameManager>
{
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

    // score ����
    public UnityEvent<int> OnScoreChange = new UnityEvent<int>();

    private int currentScore = 0;

    public int ScoreIncreaseAmount = 50;

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

    public GameObject gameOVerUI;
    //public GameOverUI gameOVerUI_restart;

    private void Update()
    {
        if (IsGameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            IsGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        IsGameOver = true;
        // ���� ���� UI�� Ȱ��ȭ
        if (gameOVerUI != null)
        {
            gameOVerUI.SetActive(true);
        }
    }

    public void AddScore()
    {
        CurrentScore += ScoreIncreaseAmount;
    }

}