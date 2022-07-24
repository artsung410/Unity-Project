using UnityEngine;

// ������ ���� ���� ���θ� �����ϴ� ���� �Ŵ���
public class GameManager : SingletonBehaviour<GameManager>
{
    public bool IsGameOver { get; set; } // ���� ���� ����

    public string Enemy = "Enemy";
    public string EffectiveRange = "EffectiveRange";
    public string Player = "Player";
    public string ImpactNormal = "ImpactNormal";
}