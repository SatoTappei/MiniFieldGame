using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�𐧌䂷��
/// </summary>
public class EnemyManager : ActorBase
{
    /// <summary>���̃^�[���ɍU��������ꍇ��true�A�ړ��̏ꍇ��false�ɂȂ�</summary>
    bool _doActionThisTurn;

    /// <summary>PlaySceneManager�����̂Ղ�ς��������ă��\�b�h���Ăяo��</summary>
    public bool DoActionThisTurn { get => _doActionThisTurn; }

    void OnEnable()
    {
        FindObjectOfType<PlaySceneManager>().AddEnemy(this);
    }

    void Update()
    {

    }

    /// <summary>���̃^�[���̓G�̍s�������肷��</summary>
    public void RequestAI()
    {
        // TODO:�G��AI�����(���݂̓����_���ōs�������肷��)
        int r = Random.Range(1, 3);
        _doActionThisTurn = r == 1;

        // �ړ��ƍs���̂ǂ���������̂���PlaySceneManager�ɋ�����
        if (_doActionThisTurn)
            FindObjectOfType<PlaySceneManager>().AddActionActor();
        else
            FindObjectOfType<PlaySceneManager>().AddMoveActor();
    }

    /// <summary>�^�[���̍ŏ��ɌĂ΂�鏈��</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " �^�[���̏��߂ɏ��������܂�");
    }

    /// <summary>�L�[���͑҂����ɌĂ΂�鏈��</summary>
    public override void StandBy()
    {
        //Debug.Log(gameObject.name + " �L�[�̓��͑҂��ł�");
    }

    /// <summary>�L�����N�^�[���ړ����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public override void MoveStart()
    {
        Debug.Log(gameObject.name + " �ړ��J�n���܂�");
        // �v���C���[���ړ�����ꍇ��StandBy�̎��_�ō��W�����܂��Ă��邽��
        // ����͈ړ����J�n����Ƃ��Ɉړ�������W�����߂Ă���B
        // �o�O������ꍇ�́A�ړ�������W�����߂鏈�����s���ꏊ��ς���
        // �ǋL:���󃉃��_���ňړ��ƍU�������߂Ă���̂ňړ��ɂȂ����ۂɒʘH�̍s���~�܂�ɂ��違���Ƀv���C���[������ꍇ�ǂ����悤���Ȃ�
        // �Ώ�:����8�}�X�Ƀv���C���[������ꍇ�͍U������悤�ɂ���̂ł��̃^�[���͂��̏�ɂƂǂ܂�

        MapManager mm = FindObjectOfType<MapManager>();

        // TODO:����̓����_����4�����Ɉړ�����̂ŃA���S���Y�����g�������@�ɒ���
        List<(int, int)> dirs = new List<(int, int)>();
        dirs.Add((1, 0));
        dirs.Add((0, 1));
        dirs.Add((-1, 0));
        dirs.Add((0, -1));

        bool canMove = false;
        // �ǂ��ɂ��ړ��ł��Ȃ��ꍇ�͂��̃^�[���͂��̏�ɂƂǂ܂�
        while (!canMove && dirs.Count > 0)
        {
            int r = Random.Range(0, dirs.Count);
            _inputDir = GetKeyToDir(dirs[r].Item1, dirs[r].Item2);
            // �ړ����悤�Ƃ��Ă���^�C�����ړ��ł��邩�ǂ����𒲂ׂ� <= �ϐ�dirs��x��z������ւ���Ă���̂Œ���
            canMove = mm.CheckCanMoveTile(_currentPosXZ.x + dirs[r].Item2, _currentPosXZ.z + dirs[r].Item1);
            dirs.RemoveAt(r);
        }
        // TODO:�����_���Ō��肱���܂�

        // �ړ���̍��W���擾
        _tartgetPosXZ = GetTargetTile(_inputDir);
        // ���݂̃^�C����̍��W���玩�g�̏����폜���Ă���
        mm.CurrentMap.SetMapTileActor(_currentPosXZ.x, _currentPosXZ.z, null);
        // �ړ���̍��W�Ɏ��g�̏���o�^���Ă���
        mm.CurrentMap.SetMapTileActor(_tartgetPosXZ.x, _tartgetPosXZ.z, this);
        // �v���C���[�͂��̏�Ō���������ς��邱�Ƃ�����̂œ��͂����Ƃ��Ɍ�����ς��邪
        // �G�͈ړ����钼�O�Ɍ�����ς���
        transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);
        StartCoroutine(Move(_tartgetPosXZ));
    }

    /// <summary>�L�����N�^�[���ړ����ɌĂ΂�鏈��</summary>
    public override void Move()
    {
        Debug.Log(gameObject.name + " �ړ����ł�");
    }

    /// <summary>�L�����N�^�[���ړ����I�����Ƃ��ɌĂ΂�鏈��</summary>
    public override void MoveEnd()
    {
        Debug.Log(gameObject.name + " �ړ����I���܂���");
    }

    /// <summary>�L�����N�^�[���s�����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public override void ActionStart()
    {
        _anim.Play("Slash");
        // �������ʂɓG��������_���[�W�A��X�ɍU���͈͍͂L���邩������Ȃ��̂ŗ��ӂ��Ă���

    }

    /// <summary>�L�����N�^�[���s�����ɌĂ΂�鏈��</summary>
    public override void Action()
    {
        Debug.Log(gameObject.name + " �s�����ł�");
    }

    /// <summary>�L�����N�^�[���s�����I����Ƃ��ɌĂ΂�鏈��</summary>
    public override void ActionEnd()
    {
        Debug.Log(gameObject.name + " �s�����I���܂���");
    }

    /// <summary>���̃L�����N�^�[���_���[�W���󂯂��Ƃ��ɌĂ΂�鏈��</summary>
    public override void Damaged()
    {
        _anim.Play("Fall");
    }
}
