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

    public Image gageX;
    public Image gageY;
    public float gageSpeed = 1.0f;  // �Q�[�W�̑����X�s�[�h
    public Ball ball;
    public Target target;
    public float waitSecGround = 2.0f; // �{�[�����n�ʂɐڒn�������̃E�F�C�g�^�C��
    public float waitSecHit = 2.0f; // �{�[�����I�ɓ����������̃E�F�C�g�^�C��
    public float waitSecTimeup = 2.0f; // ���Ԑ؂�\���̃E�F�C�g�^�C��
    public float time;  // �c�莞��
    public TextMeshProUGUI hitText;  // Hit �e�L�X�g
    public TextMeshProUGUI scoreText;  // �X�R�A�e�L�X�g
    public TextMeshProUGUI windText;  // �����e�L�X�g
    public TextMeshProUGUI flyingTimeText;  // �؋󎞊ԃe�L�X�g
    public TextMeshProUGUI timeText;  // �c�莞�ԃe�L�X�g
    public TextMeshProUGUI timeupText;  // ���Ԑ؂�e�L�X�g
    public float windMultiplier = 0.0f;  // ���̔{��

    private Mode _mode = 0;  // �Q�[�����[�h
    private float _powerX;  // X�p���[
    private float _powerY;  // Y�p���[
    private int _score;  // �X�R�A
    private float _flyingTime;  // �؋󎞊�
    private int _wind; // ����
    private Rigidbody _ballRigidbody;

    // �J�n
    void Start()
    {
        _mode = 0;
        _score = 0;
        _powerX = 0f;
        _powerY = 0f;
        _flyingTime = 0;
        _wind = Random.Range(-10, 10);
        _ballRigidbody = ball.GetComponent<Rigidbody>();

        gageX.transform.localScale = new Vector3(0, 1, 1);
        gageY.transform.localScale = new Vector3(0, 1, 1);
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
        _powerX += gageSpeed * Time.deltaTime;
        if (100f < _powerX)
        {
            _powerX = 0f;
        }

        UpdateTimer();

        Vector3 scale = gageX.transform.localScale;
        scale.x = _powerX / 100f;  // X�����̃X�P�[����ύX
        gageX.transform.localScale = scale;
    }

    // �X�V�F�c�p���[
    private void UpdateY()
    {
        _powerY += gageSpeed * Time.deltaTime;
        if (100f < _powerY)
        {
            _powerY = 0f;
        }

        UpdateTimer();

        Vector3 scale = gageY.transform.localScale;
        scale.x = _powerY / 100f;  // X�����̃X�P�[����ύX
        gageY.transform.localScale = scale;

    }

    // �X�V�F��s��
    private void UpdateFlying()
    {
        // �{�[���ɕ��̉e����^����
        Vector3 force = new Vector3(_wind * windMultiplier, 0, 0);
        _ballRigidbody.AddForce(force, ForceMode.Force);

        // �؋󎞊Ԃ̍X�V
        _flyingTime += Time.deltaTime;
        flyingTimeText.text = "+" + (int)(_flyingTime * 10f);
    }

    // ���Ԑ؂ꉉ�o
    private void UpdateTimeup()
    {
        StartCoroutine(GotoResultAfterWait());  // �R���[�`���̊J�n
    }

    // �R���[�`���FwaitSecTimeup �����҂��Ă��烊�Z�b�g
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
        _mode = Mode.MODE_Y;
    }

    // �c�p���[�̌���
    private void OnClickY()
    {
        _mode = Mode.MODE_FLYING;

        // �{�[�����R��
        ball.Kick(_powerX, _powerY);

        // �؋󎞊ԃe�L�X�g�� ON
        flyingTimeText.gameObject.SetActive(true);
    }

    // �R���[�`���FwaitSecGround �����҂��Ă��烊�Z�b�g
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(waitSecGround);

        Reset();
    }

    // Hit �e�L�X�g��\������
    private IEnumerator ShowHitText()
    {
        hitText.gameObject.SetActive(true);  // �e�L�X�g���A�N�e�B�u��
        yield return new WaitForSeconds(waitSecHit);

        hitText.gameObject.SetActive(false);  // �e�L�X�g���A�N�e�B�u��

        _score += (int)(_flyingTime * 10f);
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
        target.ResetPosition();

        _wind = Random.Range(-10, 10);
        UpdateWindText();

        _flyingTime = 0f;
        flyingTimeText.gameObject.SetActive(false);

        _mode = Mode.MODE_X;
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
