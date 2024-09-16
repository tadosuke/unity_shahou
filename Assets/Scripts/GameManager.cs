using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum Mode
    {
        MODE_X,
        MODE_Y,
        MODE_FLYING,
        MODE_HIT,
        MODE_OUT,
        MODE_TIMEUP,
    }

    public Image gageX;
    public Image gageY;
    public float gageSpeed = 1.0f;  // �Q�[�W�̑����X�s�[�h
    public Ball ball;
    public float forceMultiplierX = 10.0f;  // �͂̔{��X
    public float forceMultiplierY = 10.0f;  // �͂̔{��Y
    public float waitSecGround = 2.0f; // �{�[�����n�ʂɐڒn�������̃E�F�C�g�^�C��

    private Mode _mode = 0;
    private float _powerX;
    private float _powerY;

    void Start()
    {
        _mode = 0;
        _powerX = 0f;
        _powerY = 0f;
        gageX.transform.localScale = new Vector3(0, 1, 1);
        gageY.transform.localScale = new Vector3(0, 1, 1);
    }

    void Update()
    {
        switch(_mode)
        {
            case Mode.MODE_X: UpdateX(); break;
            case Mode.MODE_Y: UpdateY(); break;
        }
    }

    private void UpdateX()
    {
        _powerX += gageSpeed * Time.deltaTime;
        if (100f < _powerX)
        {
            _powerX = 0f;
        }

        Vector3 scale = gageX.transform.localScale;
        scale.x = _powerX / 100f;  // X�����̃X�P�[����ύX
        gageX.transform.localScale = scale;
    }

    private void UpdateY()
    {
        _powerY += gageSpeed * Time.deltaTime;
        if (100f < _powerY)
        {
            _powerY = 0f;
        }

        Vector3 scale = gageY.transform.localScale;
        scale.x = _powerY / 100f;  // X�����̃X�P�[����ύX
        gageY.transform.localScale = scale;
    }

    // �N���b�N���ꂽ���ɊO������Ă΂��
    public void OnClick()
    {
        switch (_mode)
        {
            case Mode.MODE_X: OnClickX(); break;
            case Mode.MODE_Y: OnClickY(); break;
        }
    }

    private void OnClickX()
    {
        _mode = Mode.MODE_Y;
    }

    private void OnClickY()
    {
        _mode = Mode.MODE_FLYING;

        // Apply force to the ball
        Vector3 force = new Vector3(_powerX * forceMultiplierX, _powerY * forceMultiplierY, 0);
        var rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.Impulse);
    }

    // �{�[�����n�ʂƐڒn�����Ƃ��ɌĂ΂��
    public void OnBallHitGround()
    {
        // ��s���łȂ���Ζ���
        if(_mode != Mode.MODE_FLYING)
        {
            return;
        }

        StartCoroutine(ResetAfterWait());  // �R���[�`���̊J�n
    }

    // �R���[�`���FwaitSecGround �����҂��Ă��烊�Z�b�g
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(waitSecGround);  // waitSecGround�b�҂�

        // ���Z�b�g
        _powerX = 0f;
        _powerY = 0f;
        gageX.transform.localScale = new Vector3(0, 1, 1);
        gageY.transform.localScale = new Vector3(0, 1, 1);
        ball.Reset();
        _mode = Mode.MODE_X;
    }

    // �{�[�����I�ƏՓ˂����Ƃ��ɌĂ΂��
    public void OnBallHitTarget()
    {
        Debug.Log("Hit!");
    }

}
