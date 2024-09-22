using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent OnPressed;

    // ‰Ÿ‚³‚ê‚½uŠÔ‚ÉŒÄ‚Î‚ê‚éŠÖ”
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed?.Invoke();
    }
}
