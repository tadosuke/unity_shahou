using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent OnPressed;

    // �����ꂽ�u�ԂɌĂ΂��֐�
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed?.Invoke();
    }
}
