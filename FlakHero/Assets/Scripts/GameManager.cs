using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ���� ���� ���θ� �����ϴ� ���� �Ŵ���

public class GameManager : SingletonBehaviour<GameManager>
{
    public bool IsGameOver { get; set; } // ���� ���� ����

    [HideInInspector]
    public string Enemy = "Enemy";

    [HideInInspector]
    public string EffectiveRange = "EffectiveRange";

    [HideInInspector]
    public string Player = "Player";

    [HideInInspector]
    public string ImpactNormal = "ImpactNormal";

    [SerializeField]
    private GameObject GameOverUI;

    private void Update()
    {
        if (IsGameOver == true && Input.GetKeyDown(KeyCode.P))
        {
            IsGameOver = false;
            SceneManager.LoadScene(0);
        }
    }

    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        IsGameOver = true;
        // ���� ���� UI�� Ȱ��ȭ
        GameOverUI.SetActive(true);
    }

}