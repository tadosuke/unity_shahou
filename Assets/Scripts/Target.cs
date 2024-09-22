using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameManager gameManager;

    // �����_���Ȉʒu��ݒ肷�邽�߂̕ϐ�
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        ResetPosition(); // ���������ɂ������_���ʒu�ɐݒ�
    }

    void Update()
    {
    }

    // �ʒu���Đݒ肷��
    public void ResetPosition()
    {
        // MinX ���� MaxX �͈̔͂Ń����_���� X ���W�𐶐�
        float randomX = Random.Range(minX, maxX);
        // MinY ���� MaxY �͈̔͂Ń����_���� Y ���W�𐶐�
        float randomY = Random.Range(minY, maxY);

        // Z���W�͌Œ�ŁA2D�Q�[���Ȃ�Z�͂��̂܂܈ێ��A�܂��͒���
        float currentZ = transform.position.z;

        // �V�����ʒu��ݒ�
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
