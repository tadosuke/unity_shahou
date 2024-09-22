using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameManager gameManager;

    private Vector3 _startPosition;  // �J�n���̈ʒu���L�^
    private Rigidbody _rigidbody;    // Rigidbody �R���|�[�l���g

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;  // �J�n���̈ʒu���L�^
    }

    // Update is called once per frame
    void Update()
    {
    }

    // �R���W�����ɓ��������ɌĂ΂��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameManager.OnBallHitGround();
        }
    }

    // �{�[�����J�n���̈ʒu�Ƀ��Z�b�g����֐�
    public void Reset()
    {
        // �ʒu���J�n���̈ʒu�ɖ߂�
        transform.position = _startPosition;

        // �{�[���̑��x�����Z�b�g
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
