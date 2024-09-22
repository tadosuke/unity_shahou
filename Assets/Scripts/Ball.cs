using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent OnGround;  // ボールが地面についた時のイベント
    public UnityEvent OnTarget;  // ボールが的に当たった時のイベント

    public float forceMultiplierX = 10.0f;  // 力の倍率X
    public float forceMultiplierY = 10.0f;  // 力の倍率Y
    public float windMultiplier = 0.3f;  // 風の倍率

    private Vector3 _startPosition;  // 開始時の位置を記録
    private Rigidbody _rigidbody;    // Rigidbody コンポーネント
    private TrailRenderer _trail;
    private bool _isGround;  // 接地しているか？
    private bool _isHit;  // 的に当たったか？
    private float _flyingTime;  // 滞空時間

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;  // 開始時の位置を記録
        _trail = GetComponent<TrailRenderer>();
        _flyingTime = 0.0f;
        _isGround = true;
        _isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGround && !_isHit)
        {
            // 滞空時間の更新
            _flyingTime += Time.deltaTime;
        }
    }

    // 滞空時間
    public float FlyingTime => _flyingTime;

    public void Kick(float powerX, float powerY)
    {
        // ボールに縦横パワーを与えて飛ばす
        Vector3 force = new Vector3(powerX * forceMultiplierX, powerY * forceMultiplierY, 0);
        _rigidbody.AddForce(force, ForceMode.Impulse);

        // 自然な見た目になるように、適当に回転を加える
        Vector3 torque = new Vector3(0, 0, powerX + powerY);
        _rigidbody.AddTorque(torque, ForceMode.Impulse);

        // 軌跡を ON
        _trail.enabled = true;

        _flyingTime = 0.0f;
        _isGround = false;
    }

    // ボールに風の影響を与える
    public void AddWind(float wind)
    {
        Vector3 force = new Vector3(wind * windMultiplier, 0, 0);
        _rigidbody.AddForce(force, ForceMode.Force);

    }

    // ボールを開始時の位置にリセットする関数
    public void Reset()
    {
        // 位置を開始時の位置に戻す
        transform.position = _startPosition;

        // ボールの速度をリセット
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        // 軌跡を OFF
        _trail.Clear();
        _trail.enabled = false;

        _flyingTime = 0.0f;
        _isGround = true;
        _isHit = false;
    }


    // コリジョンに入った時に呼ばれる
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            _isHit = true;
            OnTarget?.Invoke();
            return;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
            OnGround?.Invoke();
            return;
        }
    }
}
