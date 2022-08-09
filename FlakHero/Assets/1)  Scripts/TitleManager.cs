using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void GameLoad()
    {
        SceneManager.LoadScene(1);
        DataMgr.Instance.LoadGameData();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
