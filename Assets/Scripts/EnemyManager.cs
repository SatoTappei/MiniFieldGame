using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�𐧌䂷��
/// </summary>
public class EnemyManager : CharacterBase
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
        
        if (canMove)
        {
            // �ړ���̍��W���擾
            _targetPosXZ = GetTargetTile(_inputDir);
            // ���݂̃^�C����̍��W���玩�g�̏����폜���Ă���
            mm.CurrentMap.SetMapTileActor(_currentPosXZ.x, _currentPosXZ.z, null);
            // �ړ���̍��W�Ɏ��g�̏���o�^���Ă���
            mm.CurrentMap.SetMapTileActor(_targetPosXZ.x, _targetPosXZ.z, this);
            // �v���C���[�͂��̏�Ō���������ς��邱�Ƃ�����̂œ��͂����Ƃ��Ɍ�����ς��邪
            // �G�͈ړ����钼�O�Ɍ�����ς���
            transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);
            StartCoroutine(Move(_targetPosXZ));
        }
        else
        {
            // ���̏�Ɉړ����� = �Ƃǂ܂�
            StartCoroutine(Move(_currentPosXZ));
        }
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

        // ���݂͍U������ۂɃv���C���[�����邩�ǂ������肵�Ă���̂ŋ�U�肪�N����
        // TODO:��U�肳���Ȃ����߂�"���͔��}�X�Ƀv���C���[�������炻�����������čU������"�悤�ɂ���

        // �U������}�X�̏����擾
        PosXZ target = GetTargetTile(_inputDir);
        CharacterBase ab = FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor(target.x, target.z);
        // �U������}�X�Ƀv���C���[������΃_���[�W�̏���
        if (ab != null && ab.GetActorType() == ActorType.Player)
            ab.Damaged(_inputDir);

        // �������ʂɓG��������_���[�W�A��X�ɍU���͈͍͂L���邩������Ȃ��̂ŗ��ӂ��Ă���
        // �L�����N�^�[�̌�����ێ����Ă���
        // �L�����N�^�[�̑O�̃}�X�̏����擾
        // �O�̃}�X��null�Ȃ�s���I��
        // �U���A�j���[�V�����Đ�
        // �G������
        // �s���I��

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

    /// <summary>
    /// ���̃L�����N�^�[���_���[�W���󂯂��Ƃ��ɌĂ΂�鏈��
    /// </summary>
    /// <param name="attackedDir">�U�����ꂽ����</param>
    public override void Damaged(Direction attackedDir)
    {
        // �G�͑S��1���Ŏ��ʂ̂Ŏ��S�̃A�j���[�V�������Đ�����
        FindObjectOfType<PlaySceneManager>().RemoveEnemy(this);
        // �^�C����̏����폜����
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileActor(_currentPosXZ.x, _currentPosXZ.z, null);
        // ���S�̃A�j���[�V�������Đ�(�X�P�[����0�ɂ��Č����Ȃ�����)
        _anim.Play("Dead");
        // ��_���[�W�̃G�t�F�N�g�𐶐�����
        Instantiate(_damageEffect, new Vector3(transform.position.x, 0.9f, transform.position.z), Quaternion.identity);
        Instantiate(_decalEffect, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.Euler(90, 0, 0));
        // ���O�h�[���𐶐����čU�����ꂽ�����Ƃ͋t�ɐ�����΂�
        var Obj = Instantiate(_ragDoll, transform.position, Quaternion.identity);
        Vector3 vec = DirectionToVec3(attackedDir);
        Obj.GetComponent<RagDollController>().Dir = new Vector3(vec.x, 0.5f, vec.z).normalized;
    }
}
