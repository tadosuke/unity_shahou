using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindText : MonoBehaviour
{
    public VariablesSO variables;  // ゲーム内変数

    private TextMeshProUGUI _text;  // 時間切れテキスト

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.text = $"Wind: {variables.wind}";
    }
}
