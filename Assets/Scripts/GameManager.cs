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
    public Gage gageX;
    public Gage gageY;
    public Timer timer;
    public Ball ball;
    public Target target;
    public TextMeshProUGUI hitText;  // Hit テキスト
    public TextMeshProUGUI scoreText;  // スコアテキスト
    public TextMeshProUGUI windText;  // 風速テキスト
    public TextMeshProUGUI flyingTimeText;  // 滞空時間テキスト
    public TextMeshProUGUI timeText;  // 残り時間テキスト
    public TextMeshProUGUI timeupText;  // 時間切れテキスト
    public ConfigSO config;  // ゲーム設定

    // 非公開パラメータ
    private Mode _mode;  // ゲームモード
    private int _score;  // スコア
    private int _wind; // 風力

    // 開始
    void Start()
    {
        _mode = Mode.MODE_X;
        _score = 0;
        _wind = Random.Range(-10, 10);

        gageX.enabled = true;
        gageY.enabled = false;
        timer.enabled = true;

        timeupText.gameObject.SetActive(false);

        UpdateWindText();
        UpdateScoreText();
    }

    // 更新
    void Update()
    {
        switch(_mode)
        {
            case Mode.MODE_FLYING: UpdateFlying(); break;
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
        timer.enabled = false;
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
        timer.enabled = false;
        _mode = Mode.MODE_HIT;

        StartCoroutine(ShowHitText());  // テキスト表示コルーチンを呼ぶ
    }

    // 時間切れ時に呼ばれる
    public void OnTimeup()
    {
        gageX.enabled = false;
        gageY.enabled = false;
        timer.enabled = false;

        // 時間切れテキストの表示
        timeupText.gameObject.SetActive(true);
        _mode = Mode.MODE_TIMEUP;

        StartCoroutine(GotoResultAfterWait());  // コルーチンの開始
    }

    // 更新：飛行中
    private void UpdateFlying()
    {
        // ボールに風の影響を与える
        ball.AddWind(_wind);

        // 滞空時間テキストの更新
        flyingTimeText.text = "+" + (int)(ball.FlyingTime * 10f);
    }

    // コルーチン：waitSecTimeup だけ待ってからリザルトへ移行する
    private IEnumerator GotoResultAfterWait()
    {
        yield return new WaitForSeconds(config.waitSecTimeup);

        // スコアを保存
        PlayerPrefs.SetInt("Score", _score);
        PlayerPrefs.Save();

        // リザルトシーンへ遷移
        SceneManager.LoadScene("ResultScene");
    }

    // 横パワーの決定
    private void OnClickX()
    {
        gageX.enabled = false;
        gageY.enabled = true;
        _mode = Mode.MODE_Y;
    }

    // 縦パワーの決定
    private void OnClickY()
    {
        gageY.enabled = false;
        timer.enabled = false;
        _mode = Mode.MODE_FLYING;

        // ボールを蹴る
        ball.Kick(gageX.Power, gageY.Power);

        // 滞空時間テキストを ON
        flyingTimeText.gameObject.SetActive(true);
    }

    // コルーチン：waitSecGround だけ待ってからリセット
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(config.waitSecGround);

        Reset();
    }

    // コルーチン：Hit テキストを表示する
    private IEnumerator ShowHitText()
    {
        hitText.gameObject.SetActive(true);  // テキストをアクティブに
        yield return new WaitForSeconds(config.waitSecHit);

        hitText.gameObject.SetActive(false);  // テキストを非アクティブに

        // 滞空時間をスコアに加算
        _score += (int)(ball.FlyingTime * 10f);
        UpdateScoreText();

        Reset();
    }

    // リセット
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

        timer.enabled = true;
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
