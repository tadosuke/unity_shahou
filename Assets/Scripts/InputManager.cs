using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent OnPressed;

    // 押された瞬間に呼ばれる関数
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed?.Invoke();
    }
}
