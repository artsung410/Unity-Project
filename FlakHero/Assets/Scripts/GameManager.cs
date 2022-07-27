using UnityEngine;
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

    public GameObject gameOVerUI;
    public GameOverUI gameOVerUI_restart;

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

}