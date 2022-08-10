using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameData
{
    public int BGM_Volume = 0;
    public int Effect_Volume = 0;
    public int highScore = 0;
    public int score = 0;
    public int hp = 0;
    public List<EnemyData> EnemyOfWorldDatas;

    public GameData(int _highScore, int _score, int _hp)
    {
        highScore = _highScore;
        score = _score;
        hp = _hp;
        EnemyOfWorldDatas = new List<EnemyData>();
    }
}

[Serializable]
public class EnemyData
{
    public int index;
    public string name;
    public float posX;
    public float posY;
    public float posZ;
    public float moveSpeed;
    public string description;

    public EnemyData(int index, string name, float posX, float posY, float posZ, float moveSpeed, string description)
    {
        this.index = index;
        this.name = name;
        this.posX = posX;
        this.posY = posY;
        this.posZ = posZ;
        this.moveSpeed = moveSpeed;
        this.description = description;
    }
}

public class DataMgr : MonoBehaviour
{
    //public int Index { get; private set; }

    static GameObject container;
    static GameObject Container { get => container; }

    static DataMgr instance;

    public bool onLoad = false;

    public static DataMgr Instance
    {
        get
        {
            if (instance == null)
            {
                container = new GameObject();
                container.name = "DataMgr";
                instance = container.AddComponent(typeof(DataMgr)) as DataMgr;

                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    public GameData gameDatas;

    public void InitGameData()
    {
        gameDatas = new GameData(0, 0, 100);
    }

    public void SaveGameData()
    {
        InitGameData();

        LoadGameData();

        gameDatas.highScore = gameDatas.highScore < GameManager.Instance.CurrentScore ? GameManager.Instance.CurrentScore : gameDatas.highScore;
        gameDatas.score = GameManager.Instance.CurrentScore;
        gameDatas.hp = GameManager.Instance.status.CurrentHP;

        string toJsonData = JsonUtility.ToJson(gameDatas, true);
        // 모바일 : Application.persistentDataPath, 에디터 : DataPath
        // filePath => C:\Users\[UserName]\AppData\LocalLow\DefaultCompany
        string filePath = Application.persistentDataPath + GameDataFileName;
        File.WriteAllText(filePath, toJsonData);
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        if (File.Exists(filePath))
        {
            onLoad = true;

            string fromJsonData = File.ReadAllText(filePath);
            gameDatas = JsonUtility.FromJson<GameData>(fromJsonData);

 
            if (gameDatas == null)
            {
                InitGameData();
            }
        }

        else
        {
            InitGameData();
        }
    }



    public void ExitGame()
    {
        Application.Quit();
    }

    public string GameDataFileName = ".json";
}
