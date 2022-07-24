using UnityEngine;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : SingletonBehaviour<GameManager>
{
    public bool IsGameOver { get; set; } // 게임 오버 상태

    public string Enemy = "Enemy";
    public string EffectiveRange = "EffectiveRange";
    public string Player = "Player";
    public string ImpactNormal = "ImpactNormal";
}