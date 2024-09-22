using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    public GameManager gameManager;

    // 押された瞬間に呼ばれる関数
    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.OnPressed();
    }
}
