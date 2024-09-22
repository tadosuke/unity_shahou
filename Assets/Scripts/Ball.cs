using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent onGround;  // ボールが地面についた時のイベント

    private Vector3 _startPosition;  // 開始時の位置を記録
    private Rigidbody _rigidbody;    // Rigidbody コンポーネント

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;  // 開始時の位置を記録
    }

    // Update is called once per frame
    void Update()
    {
    }

    // コリジョンに入った時に呼ばれる
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround?.Invoke();
        }
    }

    // ボールを開始時の位置にリセットする関数
    public void Reset()
    {
        // 位置を開始時の位置に戻す
        transform.position = _startPosition;

        // ボールの速度をリセット
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
