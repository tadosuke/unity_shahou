using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlyingTimeText : MonoBehaviour
{
    public VariablesSO variables;
    public Ball ball;
    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.enabled = variables.showFlyingTimeText;
        if(!_text.enabled){
            return;
        }
        _text.text = "+" + (int)(ball.FlyingTime * 10f);
    }
}
