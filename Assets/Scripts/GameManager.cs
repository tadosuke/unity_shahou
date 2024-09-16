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

    private Mode _mode = 0;
    private float _powerX;
    private float _powerY;

    void Start()
    {
        _mode = 0;
        _powerX = 0f;
        _powerY = 0f;
    }

    void Update()
    {
        switch(_mode)
        {
            case Mode.MODE_X: UpdateX(); break;
            case Mode.MODE_Y: UpdateY(); break;
        }
    }

    // クリックされた時に外部から呼ばれる
    public void OnClick()
    {
        Debug.Log("GameManager:OnClick");
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

}
