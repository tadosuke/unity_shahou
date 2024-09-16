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
    public float gageSpeed = 1.0f;  // ゲージの増加スピード
    public Ball ball;
    public float forceMultiplierX = 10.0f;  // 力の倍率X
    public float forceMultiplierY = 10.0f;  // 力の倍率Y
    public float waitSecGround = 2.0f; // ボールが地面に接地した時のウェイトタイム

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
        scale.x = _powerX / 100f;  // X方向のスケールを変更
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
        scale.x = _powerY / 100f;  // X方向のスケールを変更
        gageY.transform.localScale = scale;
    }

    // クリックされた時に外部から呼ばれる
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

    // ボールが地面と接地したときに呼ばれる
    public void OnBallHitGround()
    {
        // 飛行中でなければ無視
        if(_mode != Mode.MODE_FLYING)
        {
            return;
        }

        StartCoroutine(ResetAfterWait());  // コルーチンの開始
    }

    // コルーチン：waitSecGround だけ待ってからリセット
    private IEnumerator ResetAfterWait()
    {
        yield return new WaitForSeconds(waitSecGround);  // waitSecGround秒待つ

        // リセット
        _powerX = 0f;
        _powerY = 0f;
        gageX.transform.localScale = new Vector3(0, 1, 1);
        gageY.transform.localScale = new Vector3(0, 1, 1);
        ball.Reset();
        _mode = Mode.MODE_X;
    }

    // ボールが的と衝突したときに呼ばれる
    public void OnBallHitTarget()
    {
        Debug.Log("Hit!");
    }

}
