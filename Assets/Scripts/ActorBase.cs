using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�ɓo�ꂷ��L�����ƃA�C�e���̊��N���X
/// </summary>
public abstract class ActorBase : MonoBehaviour
{
    /// <summary>���j���ꂽ���ɏo�郍�O�̃��b�Z�[�W</summary>
    [SerializeField] protected string _defeatedMessage;

    /// <summary>XZ���ʏ�ł̍��W</summary>
    public struct PosXZ
    {
        public int x;
        public int z;
    }

    /// <summary>���݂�XZ���ʏ�ł̈ʒu</summary>
    protected PosXZ _currentPosXZ;

    public PosXZ CurrentPosXZ { get => _currentPosXZ; }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// MapGenerator�Ń}�b�v�������A�z�u�ꏊ�����܂�����
    /// ���[���h���W���^�C����̍��W�ɕϊ����ăZ�b�g����
    /// </summary>
    public abstract void InitPosXZ();
}
