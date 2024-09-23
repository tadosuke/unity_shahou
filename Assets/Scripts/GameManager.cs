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
    public Ball ball;
    public Target target;
    public TextMeshProUGUI hitText;  // Hit �e�L�X�g
    public TextMeshProUGUI scoreText;  // �X�R�A�e�L�X�g
    public TextMeshProUGUI windText;  // �����e�L�X�g
    public TextMeshProUGUI flyingTimeText;  // �؋󎞊ԃe�L�X�g
    public TextMeshProUGUI timeText;  // �c�莞�ԃe�L�X�g
    public TextMeshProUGUI timeupText;  // ���Ԑ؂�e�L�X�g

    // ���J�p�����[�^
    public float gageSpeed = 50.0f;  // �Q�[�W�̑����X�s�[�h
    public float waitSecGround = 2.0f; // �{�[�����n�ʂɐڒn�������̃E�F�C�g�^�C��
    public float waitSecHit = 2.0f; // �{�[�����I�ɓ����������̃E�F�C�g�^�C��
    public float waitSecTimeup = 2.0f; // ���Ԑ؂�\���̃E�F�C�g�^�C��
    public float time = 30.0f;  // �c�莞��

    // ����J�p�����[�^
    private Mode _mode = 0;  // �Q�[�����[�h
    private int _score;  // �X�R�A
    private int _wind; // ����

    // �J�n
    void Start()
    {
        _mode = Mode.MODE_X;
        _score = 0;
        _wind = Random.Range(-10, 10);

        gageX.enabled = true;
        gageY.enabled = false;

        timeupText.gameObject.SetActive(false);

        UpdateWindText();
        UpdateScoreText();
    }

    // �X�V
    void Update()
    {
        switch(_mode)
        {
            case Mode.MODE_X: UpdateX(); break;
            case Mode.MODE_Y: UpdateY(); break;
            case Mode.MODE_FLYING: UpdateFlying(); break;
            case Mode.MODE_TIMEUP: UpdateTimeup(); break;
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
        _mode = Mode.MODE_HIT;

        StartCoroutine(ShowHitText());  // �e�L�X�g�\���R���[�`�����Ă�
    }

    // �X�V�F���p���[
    private void UpdateX()
    {
        UpdateTimer();
    }

    // �X�V�F�c�p���[
    private void UpdateY()
    {
        UpdateTimer();
    }

    // �X�V�F��s��
    private void UpdateFlying()
    {
        // �{�[���ɕ��̉e����^����
        ball.AddWind(_wind);

        // �؋󎞊ԃe�L�X�g�̍X�V
        flyingTimeText.text = "+" + (int)(ball.FlyingTime * 10f);
    }

    // ���Ԑ؂ꉉ�o
    private void UpdateTimeup()
    {
        StartCoroutine(GotoResultAfterWait());  // �R���[�`���̊J�n
    }

    // �R���[�`���FwaitSecTimeup �����҂��Ă��烊�U���g�ֈڍs����
    private IEnumerator GotoResultAfterWait()
    {
        yield return new WaitForSeconds(waitSecTimeup);

        // �X�R�A��ۑ�
        PlayerPrefs.SetInt("Score", _score);
        PlayerPrefs.Save();

        // ���U���g�V�[���֑J��
        SceneManager.LoadScene("ResultScene");
    }

    // �^�C�}�[�̍X�V
    private void UpdateTimer()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 0;
            timeupText.gameObject.SetActive(true);
            _mode = Mode.MODE_TIMEUP;
        }

        timeText.text = "Time: " + (int)time;
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
        _mode = Mode.MODE_FLYING;

        // �{�[�����R��
        ball.Kick(gageX.Power, gageY.Power);

        // �؋󎞊ԃe�L�X�g�� ON
        flyingTimeText.gameObject.SetActive(true);
    }

    // �R���[�`���FwaitSecGround �����҂��Ă��烊�Z�b�g
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(waitSecGround);

        Reset();
    }

    // �R���[�`���FHit �e�L�X�g��\������
    private IEnumerator ShowHitText()
    {
        hitText.gameObject.SetActive(true);  // �e�L�X�g���A�N�e�B�u��
        yield return new WaitForSeconds(waitSecHit);

        hitText.gameObject.SetActive(false);  // �e�L�X�g���A�N�e�B�u��

        // �؋󎞊Ԃ��X�R�A�ɉ��Z
        _score += (int)(ball.FlyingTime * 10f);
        UpdateScoreText();

        Reset();
    }

    // ���Z�b�g
    private void Reset()
    {
        ball.Reset();
        target.ResetPosition();

        _wind = Random.Range(-10, 10);
        UpdateWindText();

        flyingTimeText.gameObject.SetActive(false);

        _mode = Mode.MODE_X;

        gageX.enabled = true;
        gageX.Reset();

        gageY.enabled = false;
        gageY.Reset();
    }

    // �X�R�A�e�L�X�g�̍X�V
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + _score;
    }

    // ���e�L�X�g�̍X�V
    private void UpdateWindText()
    {
        windText.text = "Wind: " + _wind;
    }

}
