using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gage : MonoBehaviour
{
    public float gageSpeed = 50.0f;  // ゲージの増加スピード

    [SerializeField] private float _power = 0.0f;  // 現在のパワー
    private const float POWER_MAX = 100.0f;  // パワーの最大値

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
