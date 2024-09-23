using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeupText : MonoBehaviour
{
    public VariablesSO variables;
    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.enabled = variables.showTimeupText;
    }
}
