using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
    public float waitSecHit = 2.0f; // �{�[�����I�ɓ����������̃E�F�C�g�^�C��
    public TextMeshProUGUI hitText;  // Hit �e�L�X�g
    public TextMeshProUGUI scoreText;  // �X�R�A�e�L�X�g

    private Mode _mode = 0;
    private float _powerX;
    private float _powerY;
    private int _score;

    void Start()
    {
        _mode = 0;
        _score = 0;
        _powerX = 0f;
        _powerY = 0f;
        gageX.transform.localScale = new Vector3(0, 1, 1);
        gageY.transform.localScale = new Vector3(0, 1, 1);

        UpdateScoreText();
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
        _mode = Mode.MODE_OUT;

        StartCoroutine(ResetAfterWait());  // �R���[�`���̊J�n
    }

    // �R���[�`���FwaitSecGround �����҂��Ă��烊�Z�b�g
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(waitSecGround);

        Reset();
    }

    // �{�[�����I�ƏՓ˂����Ƃ��ɌĂ΂��
    public void OnBallHitTarget()
    {
        // ��s���łȂ���Ζ���
        if (_mode != Mode.MODE_FLYING)
        {
            return;
        }
        _mode = Mode.MODE_HIT;

        StartCoroutine(ShowHitText());  // �e�L�X�g�\���R���[�`�����Ă�
    }

    private IEnumerator ShowHitText()
    {
        hitText.gameObject.SetActive(true);  // �e�L�X�g���A�N�e�B�u��
        yield return new WaitForSeconds(waitSecHit);

        hitText.gameObject.SetActive(false);  // �e�L�X�g���A�N�e�B�u��

        _score += 10;
        UpdateScoreText();

        Reset();
    }

    // ���Z�b�g
    private void Reset()
    {
        _powerX = 0f;
        _powerY = 0f;
        var initScale = new Vector3(0, 1, 1);
        gageX.transform.localScale = initScale;
        gageY.transform.localScale = initScale;
        ball.Reset();

        _mode = Mode.MODE_X;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + _score;
    }

}
