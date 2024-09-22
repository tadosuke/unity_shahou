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
    public float gageSpeed = 1.0f;  // ゲージの増加スピード
    public Ball ball;
    public Target target;
    public float waitSecGround = 2.0f; // ボールが地面に接地した時のウェイトタイム
    public float waitSecHit = 2.0f; // ボールが的に当たった時のウェイトタイム
    public float waitSecTimeup = 2.0f; // 時間切れ表示のウェイトタイム
    public float time;  // 残り時間
    public TextMeshProUGUI hitText;  // Hit テキスト
    public TextMeshProUGUI scoreText;  // スコアテキスト
    public TextMeshProUGUI windText;  // 風速テキスト
    public TextMeshProUGUI flyingTimeText;  // 滞空時間テキスト
    public TextMeshProUGUI timeText;  // 残り時間テキスト
    public TextMeshProUGUI timeupText;  // 時間切れテキスト
    public float windMultiplier = 0.0f;  // 風の倍率

    private Mode _mode = 0;  // ゲームモード
    private float _powerX;  // Xパワー
    private float _powerY;  // Yパワー
    private int _score;  // スコア
    private float _flyingTime;  // 滞空時間
    private int _wind; // 風力
    private Rigidbody _ballRigidbody;

    // 開始
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

    // 更新
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

    // ボタンが押されたときに外部から呼ばれる
    public void OnPressed()
    {
        switch (_mode)
        {
            case Mode.MODE_X: OnClickX(); break;
            case Mode.MODE_Y: OnClickY(); break;
        }
    }

    // ボールが地面と接地したときに呼ばれる
    public void OnBallHitGround()
    {
        // 飛行中でなければ無視
        if(_mode != Mode.MODE_FLYING)
        {
            return;
        }
        _mode = Mode.MODE_OUT;

        StartCoroutine(ResetAfterWait());  // コルーチンの開始
    }

    // ボールが的と衝突したときに呼ばれる
    public void OnBallHitTarget()
    {
        // 飛行中でなければ無視
        if (_mode != Mode.MODE_FLYING)
        {
            return;
        }
        _mode = Mode.MODE_HIT;

        StartCoroutine(ShowHitText());  // テキスト表示コルーチンを呼ぶ
    }

    // 更新：横パワー
    private void UpdateX()
    {
        _powerX += gageSpeed * Time.deltaTime;
        if (100f < _powerX)
        {
            _powerX = 0f;
        }

        UpdateTimer();

        Vector3 scale = gageX.transform.localScale;
        scale.x = _powerX / 100f;  // X方向のスケールを変更
        gageX.transform.localScale = scale;
    }

    // 更新：縦パワー
    private void UpdateY()
    {
        _powerY += gageSpeed * Time.deltaTime;
        if (100f < _powerY)
        {
            _powerY = 0f;
        }

        UpdateTimer();

        Vector3 scale = gageY.transform.localScale;
        scale.x = _powerY / 100f;  // X方向のスケールを変更
        gageY.transform.localScale = scale;

    }

    // 更新：飛行中
    private void UpdateFlying()
    {
        // ボールに風の影響を与える
        Vector3 force = new Vector3(_wind * windMultiplier, 0, 0);
        _ballRigidbody.AddForce(force, ForceMode.Force);

        // 滞空時間の更新
        _flyingTime += Time.deltaTime;
        flyingTimeText.text = "+" + (int)(_flyingTime * 10f);
    }

    // 時間切れ演出
    private void UpdateTimeup()
    {
        StartCoroutine(GotoResultAfterWait());  // コルーチンの開始
    }

    // コルーチン：waitSecTimeup だけ待ってからリセット
    private IEnumerator GotoResultAfterWait()
    {
        yield return new WaitForSeconds(waitSecTimeup);

        // スコアを保存
        PlayerPrefs.SetInt("Score", _score);
        PlayerPrefs.Save();

        // リザルトシーンへ遷移
        SceneManager.LoadScene("ResultScene");
    }

    // タイマーの更新
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

    // 横パワーの決定
    private void OnClickX()
    {
        _mode = Mode.MODE_Y;
    }

    // 縦パワーの決定
    private void OnClickY()
    {
        _mode = Mode.MODE_FLYING;

        // ボールを蹴る
        ball.Kick(_powerX, _powerY);

        // 滞空時間テキストを ON
        flyingTimeText.gameObject.SetActive(true);
    }

    // コルーチン：waitSecGround だけ待ってからリセット
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(waitSecGround);

        Reset();
    }

    // Hit テキストを表示する
    private IEnumerator ShowHitText()
    {
        hitText.gameObject.SetActive(true);  // テキストをアクティブに
        yield return new WaitForSeconds(waitSecHit);

        hitText.gameObject.SetActive(false);  // テキストを非アクティブに

        _score += (int)(_flyingTime * 10f);
        UpdateScoreText();

        Reset();
    }

    // リセット
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

    // スコアテキストの更新
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + _score;
    }

    // 風テキストの更新
    private void UpdateWindText()
    {
        windText.text = "Wind: " + _wind;
    }

}
