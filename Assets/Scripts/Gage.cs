using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gage : MonoBehaviour
{
    public float gageSpeed = 50.0f;  // �Q�[�W�̑����X�s�[�h

    [SerializeField] private float _power = 0.0f;  // ���݂̃p���[
    private const float POWER_MAX = 100.0f;  // �p���[�̍ő�l

    public float Power => _power;

    void Start()
    {        
    }

    void Update()
    {
        _power += gageSpeed * Time.deltaTime;
        if (POWER_MAX < _power)
        {
            _power = 0f;
        }
    }

    public void Reset()
    { 
        _power = 0.0f;
    }
}
