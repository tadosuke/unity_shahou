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
    public Gage gageX;  // 縦パワー
    public Gage gageY;  // 横パワー
    public Timer timer;  // タイマー
    public Ball ball;  // ボール
    public Target target;  // 的
    public ConfigSO config;  // ゲーム設定
    public VariablesSO variables;  // ゲーム変数

    // 非公開パラメータ
    private Mode _mode;  // ゲームモード

    // 開始
    void Start()
    {
        _mode = Mode.MODE_X;

        gageX.enabled = true;
        gageY.enabled = false;
        timer.enabled = true;

        variables.score = 0;
        variables.wind = Random.Range(-10, 10);
        variables.showTimeupText = false;
        variables.showHitText = false;
        variables.showFlyingTimeText = false;
    }

    // 更新
    void Update()
    {
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

        StartCoroutine(GroundCoroutine());  // コルーチンの開始
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

        StartCoroutine(HitCoroutine());  // テキスト表示コルーチンを呼ぶ
    }

    // 時間切れ時に呼ばれる
    public void OnTimeup()
    {
        gageX.enabled = false;
        gageY.enabled = false;
        timer.enabled = false;

        // 時間切れテキストの表示
        variables.showTimeupText = true;
        _mode = Mode.MODE_TIMEUP;

        StartCoroutine(TimeupCoroutine());  // コルーチンの開始
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
        variables.showFlyingTimeText = true;
    }

    // コルーチン：地面に衝突したとき
    private IEnumerator GroundCoroutine()
    {
        yield return new WaitForSeconds(config.waitSecGround);

        Reset();
    }

    // コルーチン：Hit したとき
    private IEnumerator HitCoroutine()
    {
        variables.showHitText = true;
        yield return new WaitForSeconds(config.waitSecHit);

        variables.showHitText = false;

        // 滞空時間をスコアに加算
        variables.score += (int)(ball.FlyingTime * 10f);

        Reset();
    }

    // コルーチン：時間切れになったとき
    private IEnumerator TimeupCoroutine()
    {
        yield return new WaitForSeconds(config.waitSecTimeup);

        // スコアを保存
        PlayerPrefs.SetInt("Score", variables.score);
        PlayerPrefs.Save();

        // リザルトシーンへ遷移
        SceneManager.LoadScene("ResultScene");
    }

    // リセット
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
