using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        // �Q�[����ʂ֑J��
        SceneManager.LoadScene("MainScene");
    }
}
