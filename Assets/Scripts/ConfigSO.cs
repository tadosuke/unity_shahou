using UnityEngine;

[CreateAssetMenu(menuName = "�Q�[���ݒ�/Create")]
public class ConfigSO : ScriptableObject
{
    [Header("�Q�[�W�̑����X�s�[�h")]
    public float gageSpeed = 50.0f;

    [Header("�{�[�����n�ʂɐڒn�������̃E�F�C�g�^�C��")]
    public float waitSecGround = 2.0f;


    [Header("�{�[�����I�ɓ����������̃E�F�C�g�^�C��")]
    public float waitSecHit = 2.0f;

    [Header("���Ԑ؂�\���̃E�F�C�g�^�C��")]
    public float waitSecTimeup = 2.0f;
}
