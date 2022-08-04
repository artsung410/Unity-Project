using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI _ui;

    public void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnScoreChange.AddListener(UpdateText);
    }

    public void UpdateText(int score)
    {
        _ui.text = $"Score : {score}";
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChange.RemoveListener(UpdateText);
        }
    }
}