using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�𐧌䂷��
/// </summary>
public class PlayerManager : ActorBase
{
    /// <summary>�ړ�����ۂ̈ړ���̍��W</summary>
    PosXZ _tartgetPosXZ;
    /// <summary>���͂��ꂽ�����A�L�����N�^�[�̈ړ��Ɏg�p����</summary>
    Direction _inputDir;

    void OnEnable()
    {
        // PlaySceneManager��State�Ő��䂷�邽�߂Ɏ��g��o�^���Ă���
        FindObjectOfType<PlaySceneManager>().SetPlayer(this);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// MapGenerator�Ń}�b�v�������A�v���C���[�̔z�u�ꏊ�����܂�����
    /// ���[���h���W���^�C����̍��W�ɕϊ����ăZ�b�g����
    /// </summary>
    public void InitPosXZ()
    {
        _currentPosXZ.x = (int)transform.position.x;
        _currentPosXZ.z = (int)transform.position.z;
    }

    /// <summary>���͂ɑΉ������L�����N�^�[�̌�����Ԃ�</summary>
    Direction GetKeyToDir()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        if (vert > 0) return Direction.Up;
        else if (vert < 0) return Direction.Down;
        else if (hori > 0) return Direction.Right;
        else if (hori < 0) return Direction.Left;
        
        return Direction.Neutral;
    }

    /// <summary>�^�[���̍ŏ��ɌĂ΂�鏈��</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " �^�[���̏��߂ɏ��������܂�");
    }

    /// <summary>�L�[���͑҂����ɌĂ΂�鏈��</summary>
    public override void StandBy()
    {
        Debug.Log(gameObject.name + " �L�[�̓��͑҂��ł�");

        // �����ꂩ�̃L�[�������ꂽ��
        if (Input.anyKeyDown)
        {
            Direction inputDir = GetKeyToDir();
            // �ړ���̍��W���擾
            _tartgetPosXZ = GetTargetTile(inputDir);

            PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
            // �ړ��L�[�Ȃ�v���C���[���ړ����鏈���̗�����s��
            if (inputDir != Direction.Neutral)
                psm.SetTurnState(TurnState.PlayerMoveStart);
            // �U���L�[�Ȃ�v���C���[���U�����鏈���̗�����s��
            else if (Input.GetButtonDown("Fire1"))
                psm.SetTurnState(TurnState.PlayerActionStart);
        }
    }

    /// <summary>�L�����N�^�[���ړ����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public override void MoveStart()
    {
        // �ڕW�̍��W�Ɍ����ړ�������
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
        Debug.Log(gameObject.name + " �s�����J�n���܂�");
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
}
