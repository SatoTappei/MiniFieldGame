using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�𐧌䂷��
/// </summary>
public class PlayerManager : ActorBase
{
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

    /// <summary>�^�[���̍ŏ��ɌĂ΂�鏈��</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " �^�[���̏��߂ɏ��������܂�");
    }

    /// <summary>�L�[���͑҂����ɖ��t���[���Ă΂�鏈��</summary>
    public override void StandBy()
    {
        Debug.Log(gameObject.name + " �L�[�̓��͑҂��ł�");

        // �����ꂩ�̃L�[�������ꂽ��
        if (Input.anyKeyDown)
        {
            PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();

            // �U���L�[�Ȃ�U���̏���������
            if (Input.GetButtonDown("Submit"))
            {
                psm.SetTurnState(TurnState.PlayerActionStart);
            }
            // �ړ��L�[�Ȃ�ړ��̏���������
            else if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal"))
            {
                float vert = Input.GetAxisRaw("Vertical");
                float hori = Input.GetAxisRaw("Horizontal");
                // �ړ����悤�Ƃ��Ă���^�C�����ړ��ł��邩�ǂ����𒲂ׂ�
                bool canMove = FindObjectOfType<MapManager>().CheckCanMoveTile((int)(_currentPosXZ.x + hori), (int)(_currentPosXZ.z + vert));
                Debug.Log("�ړ��\�� " + canMove);
                _inputDir = GetKeyToDir(vert, hori);
                // �ړ���̍��W���擾
                _tartgetPosXZ = GetTargetTile(_inputDir);

                // �ړ����\�Ȃ玟��State�Ɉڍs����
                if (canMove)
                    psm.SetTurnState(TurnState.PlayerMoveStart);
            }
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
        // �e�X�g:�U���p�̃G�t�F�N�g�𐶐�����A��X�ɃL�����N�^�[�̃A�j���[�V�����ɐ؂�ւ���
        Instantiate(_attackEffect, transform.position, Quaternion.identity);
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
