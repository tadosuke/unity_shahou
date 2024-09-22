using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    public GameManager gameManager;

    // �����ꂽ�u�ԂɌĂ΂��֐�
    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.OnPressed();
    }
}
