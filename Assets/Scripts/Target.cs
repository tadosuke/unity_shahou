using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameManager gameManager;

    // ランダムな位置を設定するための変数
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        ResetPosition(); // 初期化時にもランダム位置に設定
    }

    void Update()
    {
    }

    // 位置を再設定する
    public void ResetPosition()
    {
        // MinX から MaxX の範囲でランダムな X 座標を生成
        float randomX = Random.Range(minX, maxX);
        // MinY から MaxY の範囲でランダムな Y 座標を生成
        float randomY = Random.Range(minY, maxY);

        // Z座標は固定で、2DゲームならZはそのまま維持、または調整
        float currentZ = transform.position.z;

        // 新しい位置を設定
        transform.position = new Vector3(randomX, randomY, currentZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.OnBallHitTarget();
        }
    }
}
