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

        bool canMove = false;
        while (!canMove)
        {
            // TODO:����̓����_����4�����Ɉړ�����
            (int, int)[] dirs = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            int r = Random.Range(0, 4);
            _inputDir = GetKeyToDir(dirs[r].Item1, dirs[r].Item2);
            // �ړ����悤�Ƃ��Ă���^�C�����ړ��ł��邩�ǂ����𒲂ׂ� <= x��z������ւ���Ă���̂Œ���
            canMove = FindObjectOfType<MapManager>().CheckCanMoveTile(_currentPosXZ.x + dirs[r].Item2, _currentPosXZ.z + dirs[r].Item1);
        }

        // �ړ���̍��W���擾
        _tartgetPosXZ = GetTargetTile(_inputDir);

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
