using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSetting : MonoBehaviour
{
    public Button btnSave;
    public Button btnExit;
    public Button btnMainMenu;

    void Awake()
    {
        btnSave.onClick.AddListener(DataMgr.Instance.SaveGameData);
        btnExit.onClick.AddListener(GameExit);
        btnMainMenu.onClick.AddListener(LoadTitleScene);
    }

    void GameExit()
    {
        Application.Quit();
    }

    void LoadTitleScene()
    {
        SceneManager.LoadScene(0);
    }
}
