using UnityEngine;

[CreateAssetMenu(menuName = "ゲーム設定/Create")]
public class ConfigSO : ScriptableObject
{
    [Header("ゲージの増加スピード")]
    public float gageSpeed = 50.0f;

    [Header("ボールが地面に接地した時のウェイトタイム")]
    public float waitSecGround = 2.0f;


    [Header("ボールが的に当たった時のウェイトタイム")]
    public float waitSecHit = 2.0f;

    [Header("時間切れ表示のウェイトタイム")]
    public float waitSecTimeup = 2.0f;
}
