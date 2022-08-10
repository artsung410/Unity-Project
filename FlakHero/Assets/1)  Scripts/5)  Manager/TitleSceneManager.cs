using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private DataMgr instance;

    private void Awake()
    {
        instance = DataMgr.Instance;
        SetText();
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
        instance.InitGameData();
    }

    public void GameLoad()
    {
        SceneManager.LoadScene(1);
        instance.LoadGameData();
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void SetText()
    {
        instance.LoadGameData();
        GameData gameData = DataMgr.Instance.gameDatas;
        scoreText.text = string.Format("HIGH SCORE : {0}", gameData.highScore);
    }
}
