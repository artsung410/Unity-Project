using UnityEngine;
using UnityEngine.SceneManagement;

// 점수와 게임 오버 여부를 관리하는 게임 매니저

public class GameManager : SingletonBehaviour<GameManager>
{
    public bool IsGameOver;// 게임 오버 상태

    [HideInInspector]
    public string Enemy = "Enemy";

    [HideInInspector]
    public string EffectiveRange = "EffectiveRange";

    [HideInInspector]
    public string Player = "Player";

    [HideInInspector]
    public string ImpactNormal = "ImpactNormal";

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
        // 게임 오버 상태를 참으로 변경
        IsGameOver = true;
        // 게임 오버 UI를 활성화
        if (gameOVerUI != null)
        {
            gameOVerUI.SetActive(true);
        }
    }

}