using UnityEngine;

[CreateAssetMenu(menuName = "ゲーム変数/Create")]
public class VariablesSO : ScriptableObject
{
    [Header("スコア")]
    public int score = 0;

    [Header("風の強さ")]
    public int wind = 0;

    [Header("Hit テキストを表示するか？")]
    public bool showHitText = false;

    [Header("時間切れテキストを表示するか？")]
    public bool showTimeupText = false;

    [Header("滞空時間テキストを表示するか？")]
    public bool showFlyingTimeText = false;
}
