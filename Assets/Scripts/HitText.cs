using UnityEngine;
using TMPro;

public class HitText : MonoBehaviour
{
    public VariablesSO variables;
    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _text.enabled = variables.showHitText;
    }
}
