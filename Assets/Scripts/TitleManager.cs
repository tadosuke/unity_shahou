using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        // ƒQ[ƒ€‰æ–Ê‚Ö‘JˆÚ
        SceneManager.LoadScene("MainScene");
    }
}
