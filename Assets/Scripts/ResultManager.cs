using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // �X�R�A�\���p�̃e�L�X�g


    void Start()
    {
        int score = PlayerPrefs.GetInt("Score", 0);  // "Score"�L�[�̒l���擾�i�f�t�H���g��0�j
        scoreText.text = "Score: " + score;
    }

    public void OnOKButtonClicked()
    {
        // �^�C�g����ʂ֑J��
        SceneManager.LoadScene("TitleScene");
    }
}
