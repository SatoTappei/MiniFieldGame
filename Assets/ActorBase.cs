using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�ɓo�ꂷ��L�����ƃA�C�e���̊��N���X
/// </summary>
public abstract class ActorBase : MonoBehaviour
{
    /// <summary>XZ���ʏ�ł̍��W</summary>
    protected struct PosXZ
    {
        public int x;
        public int z;
    }

    /// <summary>���݂�XZ���ʏ�ł̈ʒu</summary>
    protected PosXZ _currentPosXZ;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// MapGenerator�Ń}�b�v�������A�z�u�ꏊ�����܂�����
    /// ���[���h���W���^�C����̍��W�ɕϊ����ăZ�b�g����
    /// �h���N���X�ŃI�[�o�[���C�h����ۂɂ�MapManager�̐������ꂽ���W�Ɏ��g��o�^���鏈����ǉ����邱��
    /// </summary>
    public virtual void InitPosXZ()
    {
        _currentPosXZ.x = (int)transform.position.x;
        _currentPosXZ.z = (int)transform.position.z;

        // FindObjectOfType<MapManager>().CurrentMap.SetMapTile~�̃��\�b�h���Ă�œo�^����
    }
}
