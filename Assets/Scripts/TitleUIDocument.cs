using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleUIDocument : MonoBehaviour
{
    private UIDocument _uiDocument;

    // Start is called before the first frame update
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();

        // ボタン設定
        var buttonElement = _uiDocument.rootVisualElement.Q<Button>("Button");
        buttonElement.clicked += () => { SceneManager.LoadScene("MainScene"); };
    }
}
