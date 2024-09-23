using UnityEngine;

[CreateAssetMenu(menuName = "ゲーム変数/Create")]
public class VariablesSO : ScriptableObject
{
    [Header("スコア")]
    public int score = 0;

    [Header("風の強さ")]
    public int wind = 0;
}
