using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�ɓo�ꂷ��L�����N�^�[�̊��N���X
/// </summary>
public abstract class ActorBase : MonoBehaviour
{
    /// <summary>XZ���ʏ�ł̍��W</summary>
    protected struct PosXZ
    {
        public int x;
        public int z;
    }

    /// <summary>�L�����N�^�[�̕���</summary>
    public enum Direction
    {
        Neutral = 360, // �������͂���Ă��Ȃ����
        Up = 0,
        Down = 180,
        Right = 90,
        Left = 270,
    };

    /// <summary>�L�����N�^�[�����̃^�C���Ɉړ�����̂ɂ����鎞��</summary>
    protected const float MoveTileTime = 30.0f;
    /// <summary>���̃L�����N�^�[���N���ł���^�C��</summary>
    [SerializeField] TileType[] _canMoveTile;
    /// <summary>���݂̃L�����N�^�[�̌���</summary>
    Direction _currentDir = Direction.Up;
    /// <summary>���݂�XZ���ʏ�ł̈ʒu</summary>
    protected PosXZ _currentPosXZ;
    /// <summary>�s�������ǂ���</summary>
    protected bool _inAction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�^�[���̍ŏ��ɌĂ΂�鏈��</summary>
    public abstract void TurnInit();

    /// <summary>�L�[���͑҂����ɌĂ΂�鏈��</summary>
    public abstract void StandBy();

    /// <summary>�L�����N�^�[���ړ����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public abstract void MoveStart();

    /// <summary>�L�����N�^�[���ړ����ɌĂ΂�鏈��</summary>
    public abstract void Move();

    /// <summary>�L�����N�^�[���ړ����I�����Ƃ��ɌĂ΂�鏈��</summary>
    public abstract void MoveEnd();

    /// <summary>�L�����N�^�[���s�����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public abstract void ActionStart();

    /// <summary>�L�����N�^�[���s�����ɌĂ΂�鏈��</summary>
    public abstract void Action();

    /// <summary>�L�����N�^�[���s�����I����Ƃ��ɌĂ΂�鏈��</summary>
    public abstract void ActionEnd();
}
