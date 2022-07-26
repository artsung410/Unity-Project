using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private void Update()
    {
        if(GameManager.Instance.IsGameOver == true)
        {
            gameObject.SetActive(true);
        }
    }

}
