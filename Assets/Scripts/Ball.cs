using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent OnGround;  // �{�[�����n�ʂɂ������̃C�x���g
    public UnityEvent OnTarget;  // �{�[�����I�ɓ����������̃C�x���g

    public float forceMultiplierX = 10.0f;  // �͂̔{��X
    public float forceMultiplierY = 10.0f;  // �͂̔{��Y
    public float windMultiplier = 0.3f;  // ���̔{��

    private Vector3 _startPosition;  // �J�n���̈ʒu���L�^
    private Rigidbody _rigidbody;    // Rigidbody �R���|�[�l���g
    private TrailRenderer _trail;
    private bool _isGround;  // �ڒn���Ă��邩�H
    private bool _isHit;  // �I�ɓ����������H
    private float _flyingTime;  // �؋󎞊�

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;  // �J�n���̈ʒu���L�^
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
            // �؋󎞊Ԃ̍X�V
            _flyingTime += Time.deltaTime;
        }
    }

    // �؋󎞊�
    public float FlyingTime => _flyingTime;

    public void Kick(float powerX, float powerY)
    {
        // �{�[���ɏc���p���[��^���Ĕ�΂�
        Vector3 force = new Vector3(powerX * forceMultiplierX, powerY * forceMultiplierY, 0);
        _rigidbody.AddForce(force, ForceMode.Impulse);

        // ���R�Ȍ����ڂɂȂ�悤�ɁA�K���ɉ�]��������
        Vector3 torque = new Vector3(0, 0, powerX + powerY);
        _rigidbody.AddTorque(torque, ForceMode.Impulse);

        // �O�Ղ� ON
        _trail.enabled = true;

        _flyingTime = 0.0f;
        _isGround = false;
    }

    // �{�[���ɕ��̉e����^����
    public void AddWind(float wind)
    {
        Vector3 force = new Vector3(wind * windMultiplier, 0, 0);
        _rigidbody.AddForce(force, ForceMode.Force);

    }

    // �{�[�����J�n���̈ʒu�Ƀ��Z�b�g����֐�
    public void Reset()
    {
        // �ʒu���J�n���̈ʒu�ɖ߂�
        transform.position = _startPosition;

        // �{�[���̑��x�����Z�b�g
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        // �O�Ղ� OFF
        _trail.Clear();
        _trail.enabled = false;

        _flyingTime = 0.0f;
        _isGround = true;
        _isHit = false;
    }


    // �R���W�����ɓ��������ɌĂ΂��
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
