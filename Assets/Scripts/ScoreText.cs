using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public VariablesSO variables;  // ゲーム内変数

    private TextMeshProUGUI _text;  // 時間切れテキスト

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = $"Score: {variables.score}";
    }
}
