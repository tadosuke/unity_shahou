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

    // 参照
    public Gage gageLogicX;
    public Gage gageLogicY;
    public Image gageX;
    public Image gageY;
    public Ball ball;
    public Target target;
    public TextMeshProUGUI hitText;  // Hit テキスト
    public TextMeshProUGUI scoreText;  // スコアテキスト
    public TextMeshProUGUI windText;  // 風速テキスト
    public TextMeshProUGUI flyingTimeText;  // 滞空時間テキスト
    public TextMeshProUGUI timeText;  // 残り時間テキスト
    public TextMeshProUGUI timeupText;  // 時間切れテキスト

    // 公開パラメータ
    public float gageSpeed = 50.0f;  // ゲージの増加スピード
    public float waitSecGround = 2.0f; // ボールが地面に接地した時のウェイトタイム
    public float waitSecHit = 2.0f; // ボールが的に当たった時のウェイトタイム
    public float waitSecTimeup = 2.0f; // 時間切れ表示のウェイトタイム
    public float time = 30.0f;  // 残り時間

    // 非公開パラメータ
    private Mode _mode = 0;  // ゲームモード
    private int _score;  // スコア
    private int _wind; // 風力

    // 開始
    void Start()
    {
        _mode = Mode.MODE_X;
        _score = 0;
        _wind = Random.Range(-10, 10);

        gageLogicX.enabled = true;
        gageX.transform.localScale = new Vector3(0, 1, 1);

        gageLogicY.enabled = false;
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
        UpdateTimer();

        Vector3 scale = gageX.transform.localScale;
        scale.x = gageLogicX.Power / 100f;  // X方向のスケールを変更
        gageX.transform.localScale = scale;
    }

    // 更新：縦パワー
    private void UpdateY()
    {
        UpdateTimer();

        Vector3 scale = gageY.transform.localScale;
        scale.x = gageLogicY.Power / 100f;  // X方向のスケールを変更
        gageY.transform.localScale = scale;

    }

    // 更新：飛行中
    private void UpdateFlying()
    {
        // ボールに風の影響を与える
        ball.AddWind(_wind);

        // 滞空時間テキストの更新
        flyingTimeText.text = "+" + (int)(ball.FlyingTime * 10f);
    }

    // 時間切れ演出
    private void UpdateTimeup()
    {
        StartCoroutine(GotoResultAfterWait());  // コルーチンの開始
    }

    // コルーチン：waitSecTimeup だけ待ってからリザルトへ移行する
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
        gageLogicX.enabled = false;
        gageLogicY.enabled = true;
        _mode = Mode.MODE_Y;
    }

    // 縦パワーの決定
    private void OnClickY()
    {
        gageLogicY.enabled = false;
        _mode = Mode.MODE_FLYING;

        // ボールを蹴る
        ball.Kick(gageLogicX.Power, gageLogicY.Power);

        // 滞空時間テキストを ON
        flyingTimeText.gameObject.SetActive(true);
    }

    // コルーチン：waitSecGround だけ待ってからリセット
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(waitSecGround);

        Reset();
    }

    // コルーチン：Hit テキストを表示する
    private IEnumerator ShowHitText()
    {
        hitText.gameObject.SetActive(true);  // テキストをアクティブに
        yield return new WaitForSeconds(waitSecHit);

        hitText.gameObject.SetActive(false);  // テキストを非アクティブに

        // 滞空時間をスコアに加算
        _score += (int)(ball.FlyingTime * 10f);
        UpdateScoreText();

        Reset();
    }

    // リセット
    private void Reset()
    {
        var initScale = new Vector3(0, 1, 1);
        gageX.transform.localScale = initScale;
        gageY.transform.localScale = initScale;
        ball.Reset();
        target.ResetPosition();

        _wind = Random.Range(-10, 10);
        UpdateWindText();

        flyingTimeText.gameObject.SetActive(false);

        _mode = Mode.MODE_X;

        gageLogicX.enabled = true;
        gageLogicX.Reset();

        gageLogicY.enabled = false;
        gageLogicY.Reset();
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
