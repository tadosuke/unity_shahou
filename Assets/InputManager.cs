using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerClickHandler
{
    public GameManager gameManager;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // クリック時に呼ばれる関数
    public void OnPointerClick(PointerEventData eventData)
    {
        // クリックされた座標やイベントの処理をここに記述
        gameManager.OnClick();
    }
}
