using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������X�e�[�W�̃f�[�^
/// </summary>
[CreateAssetMenu]
public class StageDataSO : ScriptableObject
{
    //Header("�X�e�[�W�̕�"), SerializeField[SerializeField] int _width;
    //[Header("�X�e�[�W����"), SerializeFieldSerializeField] int _height;
    /// <summary>�Q�[���I�[�o�[�܂ł̃^�[������</summary>
    [Header("��������"), SerializeField] int _turnLimit;
    /// <summary>�t���A�ɐ��������G�̍ő吔</summary>
    [Header("�G�̍ő吔"), SerializeField] int _maxEnemy;
    /// <summary>�t���A�ɐ�������G</summary>
    [Header("��������G"), SerializeField] GameObject[] _enemies;
    /// <summary>�t���A�ɐ��������R�C���̍ő吔</summary>
    [Header("�R�C���̍ő吔"), SerializeField] int _maxCoin;
    /// <summary>�X�e�[�W�ɐ�������R�C��</summary>
    [Header("��������R�C��"), SerializeField] GameObject _coin;

    public int TurnLimit { get => _turnLimit; }
    public int MaxEnemy { get => _maxEnemy; }
    public GameObject[] Enemies { get => _enemies; }
    public int MaxCoin { get => _maxCoin; }
    public GameObject Coin { get => _coin; }
}
