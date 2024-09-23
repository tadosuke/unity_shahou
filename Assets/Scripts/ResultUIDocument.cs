using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ResultUIDocument : MonoBehaviour
{
    private UIDocument _uiDocument;

    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();

        // スコアテキスト
        int score = PlayerPrefs.GetInt("Score", 0);
        var labelElement = _uiDocument.rootVisualElement.Q<Label>("Label");
        labelElement.text = $"Score: {score}";

        // ボタン設定
        var buttonElement = _uiDocument.rootVisualElement.Q<Button>("Button");
        buttonElement.clicked += () => { SceneManager.LoadScene("TitleScene"); };
    }
}
