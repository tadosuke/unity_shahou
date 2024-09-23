using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GageUI : MonoBehaviour
{
    [SerializeField] private Gage model;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.x = model.Power / 100f;  // X�����̃X�P�[����ύX
        transform.localScale = scale;
    }
}
