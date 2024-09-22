using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    public GameManager gameManager;

    // ‰Ÿ‚³‚ê‚½uŠÔ‚ÉŒÄ‚Î‚ê‚éŠÖ”
    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.OnPressed();
    }
}
