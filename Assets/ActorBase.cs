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
    protected const float MoveTileTime = 60.0f;
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

    /// <summary>�w�肵�����W�ɕ⊮���ړ�������</summary>
    protected IEnumerator Move(PosXZ target)
    {
        Vector3 currentPos = new Vector3(_currentPosXZ.x, 0, _currentPosXZ.z);
        Vector3 targetPos = new Vector3(target.x, 0, target.z);

        int count = 0;
        while (transform.position != targetPos)
        {
            float value = count / MoveTileTime;
            transform.position = Vector3.Lerp(currentPos, targetPos, value);
            yield return null;
            count++;
        }

        // �ړ������������猻�݂̃^�C����̈ʒu���ړ���̍��W�ɕύX����
        _currentPosXZ = target;
    }

    /// <summary>���݂̍��W�ƕ�������ړ���̍��W���擾</summary>
    protected PosXZ GetTargetTile(Direction dir)
    {
        PosXZ target = _currentPosXZ;

        if (dir == Direction.Up) target.z++;
        else if (dir == Direction.Down) target.z--;
        else if (dir == Direction.Right) target.x++;
        else if (dir == Direction.Left) target.x--;

        return target;
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
