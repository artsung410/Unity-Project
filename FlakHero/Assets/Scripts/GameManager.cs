using UnityEngine;
using UnityEngine.SceneManagement;

// 점수와 게임 오버 여부를 관리하는 게임 매니저

public class GameManager : SingletonBehaviour<GameManager>
{
    public bool IsGameOver { get; set; } // 게임 오버 상태

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
        // 게임 오버 상태를 참으로 변경
        IsGameOver = true;
        // 게임 오버 UI를 활성화
        GameOverUI.SetActive(true);
    }

}