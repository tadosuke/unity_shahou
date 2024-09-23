using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    // �Q��
    public Gage gageX;
    public Gage gageY;
    public Timer timer;
    public Ball ball;
    public Target target;
    public ConfigSO config;  // �Q�[���ݒ�
    public VariablesSO variables;  // �Q�[���ϐ�

    // ����J�p�����[�^
    private Mode _mode;  // �Q�[�����[�h

    // �J�n
    void Start()
    {
        _mode = Mode.MODE_X;
        variables.score = 0;
        variables.wind = Random.Range(-10, 10);

        gageX.enabled = true;
        gageY.enabled = false;
        timer.enabled = true;

        variables.showTimeupText = false;
    }

    // �X�V
    void Update()
    {
        switch(_mode)
        {
            case Mode.MODE_FLYING: UpdateFlying(); break;
        }
    }

    // �{�^���������ꂽ�Ƃ��ɊO������Ă΂��
    public void OnPressed()
    {
        switch (_mode)
        {
            case Mode.MODE_X: OnClickX(); break;
            case Mode.MODE_Y: OnClickY(); break;
        }
    }

    // �{�[�����n�ʂƐڒn�����Ƃ��ɌĂ΂��
    public void OnBallHitGround()
    {
        // ��s���łȂ���Ζ���
        if(_mode != Mode.MODE_FLYING)
        {
            return;
        }
        timer.enabled = false;
        _mode = Mode.MODE_OUT;

        StartCoroutine(ResetAfterWait());  // �R���[�`���̊J�n
    }

    // �{�[�����I�ƏՓ˂����Ƃ��ɌĂ΂��
    public void OnBallHitTarget()
    {
        // ��s���łȂ���Ζ���
        if (_mode != Mode.MODE_FLYING)
        {
            return;
        }
        timer.enabled = false;
        _mode = Mode.MODE_HIT;

        StartCoroutine(ShowHitText());  // �e�L�X�g�\���R���[�`�����Ă�
    }

    // ���Ԑ؂ꎞ�ɌĂ΂��
    public void OnTimeup()
    {
        gageX.enabled = false;
        gageY.enabled = false;
        timer.enabled = false;

        // ���Ԑ؂�e�L�X�g�̕\��
        variables.showTimeupText = true;
        _mode = Mode.MODE_TIMEUP;

        StartCoroutine(GotoResultAfterWait());  // �R���[�`���̊J�n
    }

    // �X�V�F��s��
    private void UpdateFlying()
    {
        // �{�[���ɕ��̉e����^����
        ball.AddWind(variables.wind);
    }

    // �R���[�`���FwaitSecTimeup �����҂��Ă��烊�U���g�ֈڍs����
    private IEnumerator GotoResultAfterWait()
    {
        yield return new WaitForSeconds(config.waitSecTimeup);

        // �X�R�A��ۑ�
        PlayerPrefs.SetInt("Score", variables.score);
        PlayerPrefs.Save();

        // ���U���g�V�[���֑J��
        SceneManager.LoadScene("ResultScene");
    }

    // ���p���[�̌���
    private void OnClickX()
    {
        gageX.enabled = false;
        gageY.enabled = true;
        _mode = Mode.MODE_Y;
    }

    // �c�p���[�̌���
    private void OnClickY()
    {
        gageY.enabled = false;
        timer.enabled = false;
        _mode = Mode.MODE_FLYING;

        // �{�[�����R��
        ball.Kick(gageX.Power, gageY.Power);

        // �؋󎞊ԃe�L�X�g�� ON
        variables.showFlyingTimeText = true;
    }

    // �R���[�`���FwaitSecGround �����҂��Ă��烊�Z�b�g
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(config.waitSecGround);

        Reset();
    }

    // �R���[�`���FHit �e�L�X�g��\������
    private IEnumerator ShowHitText()
    {
        variables.showHitText = true;
        yield return new WaitForSeconds(config.waitSecHit);

        variables.showHitText = false;

        // �؋󎞊Ԃ��X�R�A�ɉ��Z
        variables.score += (int)(ball.FlyingTime * 10f);

        Reset();
    }

    // ���Z�b�g
    private void Reset()
    {
        ball.Reset();
        target.ResetPosition();

        variables.wind = Random.Range(-10, 10);

        variables.showFlyingTimeText = false;

        _mode = Mode.MODE_X;

        gageX.enabled = true;
        gageX.Reset();

        gageY.enabled = false;
        gageY.Reset();

        timer.enabled = true;
    }
}
