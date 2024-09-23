using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public UnityEvent OnTimeup;
    public float timeMax = 30.0f;  // ��������

    [SerializeField] private float _time;  // �c�莞��

    public float CurrentTime => _time;

    void Start()
    {
        _time = timeMax;
    }

    void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            _time = 0;
            OnTimeup.Invoke();
        }
    }
}
