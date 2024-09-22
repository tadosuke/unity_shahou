using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // スコア表示用のテキスト


    void Start()
    {
        int score = PlayerPrefs.GetInt("Score", 0);  // "Score"キーの値を取得（デフォルトは0）
        scoreText.text = "Score: " + score;
    }

    public void OnOKButtonClicked()
    {
        // タイトル画面へ遷移
        SceneManager.LoadScene("TitleScene");
    }
}
