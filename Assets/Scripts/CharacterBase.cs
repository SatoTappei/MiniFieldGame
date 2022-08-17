using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�ɓo�ꂷ��L�����N�^�[�̊��N���X
/// </summary>
public abstract class CharacterBase : ActorBase
{
    
    /// <summary>�L�����N�^�[�̎��</summary>
    public enum CharacterType
    {
        Player,
        Enemy,
        // Obstacle,
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

    [SerializeField] protected Animator _anim;
    /// <summary>�L�����N�^�[�����̃^�C���Ɉړ�����̂ɂ����鎞��</summary>
    protected const float MoveTileTime = 15.0f;
    /// <summary>���̃L�����N�^�[�̎��</summary>
    [SerializeField] CharacterType _characterType;
    /// <summary>���̃L�����N�^�[���N���ł���^�C��</summary>
    [SerializeField] TileType[] _canMoveTile;
    /// <summary>��_���[�W���̃G�t�F�N�g</summary>
    [SerializeField] protected GameObject _damageEffect;
    /// <summary>��_���[�W����Decal�G�t�F�N�g</summary>
    [SerializeField] protected GameObject _decalEffect;
    /// <summary>�L�����N�^�[�����񂾂Ƃ��ɏo�郉�O�h�[��</summary>
    [SerializeField] protected GameObject _ragDoll;
    /// <summary>���݂̃L�����N�^�[�̌���</summary>
    //Direction _currentDir = Direction.Up;
    /// <summary>���͂��ꂽ�����A�L�����N�^�[�̈ړ��Ɏg�p����B�G�̏ꍇ�͎����Ō��܂�</summary>
    protected Direction _inputDir;
    /// <summary>�ړ�����ۂ̈ړ���̍��W</summary>
    protected PosXZ _targetPosXZ;
    /// <summary>�s�������ǂ���</summary>
    protected bool _inAction;
    
    /// <summary>���̃L�����N�^�[�̎�ނ�Ԃ�</summary>
    public CharacterType GetCharacterType() => _characterType;

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
    public override void InitPosXZ()
    {
        _currentPosXZ.x = (int)transform.position.x;
        _currentPosXZ.z = (int)transform.position.z;

        // �������ꂽ���W�Ɏ��g���Z�b�g���čU����ړ��̔���Ɏg����悤�ɂ���
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileCharacter(_currentPosXZ.x, _currentPosXZ.z, this);
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
        //MapManager mm = FindObjectOfType<MapManager>();
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
        // �ړ������������玩�����Ō�Ɉړ����������L�����N�^�[�����m�F���Ă��炤
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

    /// <summary>�����ɑΉ�����Vector3�^��Ԃ�</summary>
    protected Vector3 DirectionToVec3(Direction dir)
    {
        if (dir == Direction.Up) return Vector3.forward;
        else if (dir == Direction.Down) return Vector3.back;
        else if (dir == Direction.Right) return Vector3.right;
        else if (dir == Direction.Left) return Vector3.left;

        // �ǂ�ɂ��Y�����Ȃ��Ȃ�x/z�����ł͂Ȃ��ď�����̃x�N�g����Ԃ�
        return Vector3.up;
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

    /// <summary>���̃L�����N�^�[���_���[�W���󂯂��Ƃ��ɌĂ΂�鏈��</summary>
    public abstract void Damaged(Direction attackedDir);
}
