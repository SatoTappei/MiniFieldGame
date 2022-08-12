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
    protected const float MoveTileTime = 15.0f;
    /// <summary>���̃L�����N�^�[���N���ł���^�C��</summary>
    [SerializeField] TileType[] _canMoveTile;
    /// <summary>�e�X�g:�U���p���ɏo��G�t�F�N�g�A�K�v�Ȃ��Ȃ����</summary>
    [SerializeField] protected GameObject _attackEffect;
    /// <summary>���݂̃L�����N�^�[�̌���</summary>
    //Direction _currentDir = Direction.Up;
    /// <summary>���͂��ꂽ�����A�L�����N�^�[�̈ړ��Ɏg�p����B�G�̏ꍇ�͎����Ō��܂�</summary>
    protected Direction _inputDir;
    /// <summary>���݂�XZ���ʏ�ł̈ʒu</summary>
    protected PosXZ _currentPosXZ;
    /// <summary>�ړ�����ۂ̈ړ���̍��W</summary>
    protected PosXZ _tartgetPosXZ;
    /// <summary>�s�������ǂ���</summary>
    protected bool _inAction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>���͂ɑΉ������L�����N�^�[�̌�����Ԃ�</summary>
    protected Direction GetKeyToDir(float vert, float hori)
    {
        if (vert == 1) return Direction.Up;
        else if (vert == -1) return Direction.Down;
        else if (hori == 1) return Direction.Right;
        else if (hori == -1) return Direction.Left;

        return Direction.Neutral;
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
        // �ړ�������������PlaySceneManager��CheckRemMoveActor���\�b�h���Ă��
        // �������Ō�Ɉړ����������L�����N�^�[�����m�F���Ă��炤
        FindObjectOfType<PlaySceneManager>().CheckRemMoveActor();
    }

    /// <summary>���݂̍��W�ƕ�������ړ���̍��W���擾</summary>
    protected PosXZ GetTargetTile(Direction dir)
    {
        PosXZ target = _currentPosXZ;

        if (dir == Direction.Up) target.z++;
        else if (dir == Direction.Down) target.z--;
        else if (dir == Direction.Right) target.x++;
        else if (dir == Direction.Left) target.x--;
        
        // �ړ��悪�ǂȂ猻�݂̈ʒu��Ԃ��A���̍ۂ̓^�[����i�߂Ȃ��悤�ɂ���

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
