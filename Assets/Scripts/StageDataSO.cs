using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������X�e�[�W�̃f�[�^
/// </summary>
[CreateAssetMenu]
public class StageDataSO : ScriptableObject
{
    /// <summary>���������X�e�[�W�̕�</summary>
    [Header("�X�e�[�W�̕�"), SerializeField] int _width;
    /// <summary>���������X�e�[�W�̍���</summary>
    [Header("�X�e�[�W����"), SerializeField] int _height;
    /// <summary>�Q�[���I�[�o�[�܂ł̃^�[������</summary>
    [Header("��������"), SerializeField] int _turnLimit;
    /// <summary>�t���A�ɐ��������G�̍ő吔</summary>
    [Header("�G�̍ő吔"), SerializeField] int _maxEnemy;
    /// <summary>�t���A�ɐ�������G</summary>
    [Header("��������G"), SerializeField] GameObject[] _enemies;
    /// <summary>�t���A�ɐ�������A�C�e���̍ő吔</summary>
    [Header("�A�C�e���̍ő吔"), SerializeField] int _maxItem;
    /// <summary>�t���A�ɐ�������A�C�e��</summary>
    [Header("��������A�C�e��"), SerializeField] GameObject[] _items;
    /// <summary>�t���A�ɐ���������Q���̍ő吔</summary>
    //[Header("��Q���̍ő吔"), SerializeField] int _maxObst;
    /// <summary>�t���A�ɐ��������Q��</summary>
    //[Header("���������Q��"), SerializeField] GameObject[] _obstacles;
    /// <summary>�t���A�ɐ��������R�C���̍ő吔</summary>
    [Header("�R�C���̍ő吔"), SerializeField] int _maxCoin;
    /// <summary>�X�e�[�W�ɐ�������R�C��</summary>
    [Header("��������R�C��"), SerializeField] GameObject[] _coins;
    /// <summary>�t���A�ɉ_�𐶐����邩</summary>
    [Header("�t���A�ɉ_�𐶐����邩"), SerializeField] bool _isCoudy;
    /// <summary>�}�b�v��������������R���|�[�l���g</summary>
    [Header("�}�b�v��������������R���|�[�l���g"), SerializeField] MapGeneratorBase _mapGenerator;

    public int Width { get => _width; }
    public int Height { get => _height; }
    public int TurnLimit { get => _turnLimit; }
    public int MaxEnemy { get => _maxEnemy; }
    //public GameObject[] Obstacles { get => _obstacles; }
    //public int MaxObst { get => _maxObst; }
    public GameObject[] Items { get => _items; }
    public int MaxItem { get => _maxItem; }
    public GameObject[] Enemies { get => _enemies; }
    public int MaxCoin { get => _maxCoin; }
    public GameObject[] Coins { get => _coins; }
    public bool IsCoudy { get => _isCoudy; }
    public MapGeneratorBase MapGenerator { get => _mapGenerator; }
}
