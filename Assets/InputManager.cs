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

    // �N���b�N���ɌĂ΂��֐�
    public void OnPointerClick(PointerEventData eventData)
    {
        // �N���b�N���ꂽ���W��C�x���g�̏����������ɋL�q
        gameManager.OnClick();
    }
}
