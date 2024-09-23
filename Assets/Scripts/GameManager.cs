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
    public ConfigSO config;  // ゲーム設定
    public VariablesSO variables;  // ゲーム変数

    // 非公開パラメータ
    private Mode _mode;  // ゲームモード

    // 開始
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
        variables.showTimeupText = true;
        _mode = Mode.MODE_TIMEUP;

        StartCoroutine(GotoResultAfterWait());  // コルーチンの開始
    }

    // コルーチン：waitSecTimeup だけ待ってからリザルトへ移行する
    private IEnumerator GotoResultAfterWait()
    {
        yield return new WaitForSeconds(config.waitSecTimeup);

        // スコアを保存
        PlayerPrefs.SetInt("Score", variables.score);
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
        variables.showFlyingTimeText = true;
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
        variables.showHitText = true;
        yield return new WaitForSeconds(config.waitSecHit);

        variables.showHitText = false;

        // 滞空時間をスコアに加算
        variables.score += (int)(ball.FlyingTime * 10f);

        Reset();
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
