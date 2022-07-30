using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameOverUI : MonoBehaviour
{
    private GameObject[] childs;
    private int childCount;
    void Awake()
    {
        childCount = transform.childCount;
        childs = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childs[i] = transform.GetChild(i).gameObject;
        }
    }

    void OnEnable()
    {
        GameManager.Instance.OnGameEnd.AddListener(Activate);
    }

    public void Activate()
    {
        for (int i = 0; i < childCount; i++)
        {
            childs[i].SetActive(true);
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameEnd.RemoveListener(Activate);
        }
    }
}