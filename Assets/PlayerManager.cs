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

    /// <summary>�^�[���̍ŏ��ɌĂ΂�鏈��</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " �^�[���̏��߂ɏ��������܂�");
    }

    /// <summary>�L�[���͑҂����ɖ��t���[���Ă΂�鏈��</summary>
    public override void StandBy()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        // �s��
        if (Input.GetButtonDown("Submit"))
        {
            // �s������L�����N�^�[���Ƃ������Ƃ�PlaySceneManager�ɓ`���Ď���State�Ɉڍs����
            PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
            psm.AddActionActor();
            FindObjectOfType<PlaySceneManager>().SetTurnState(TurnState.PlayerActionStart);
        }
        // �ړ� �c�Ɖ��������ɉ�����Ă����疳��(�o�O���o����)
        else if (Mathf.Abs(vert + hori) == 1)
        {
            MapManager mm = FindObjectOfType<MapManager>();
            // �ړ����悤�Ƃ��Ă���^�C�����ړ��ł��邩�ǂ����𒲂ׂ�
            bool canMove = mm.CheckCanMoveTile((int)(_currentPosXZ.x + hori), (int)(_currentPosXZ.z + vert));
            _inputDir = GetKeyToDir(vert, hori);
            // �ړ���̍��W���擾
            _tartgetPosXZ = GetTargetTile(_inputDir);
            // �ړ��̉\�s�\�Ɍ��炸���͂��ꂽ�����ɃL�����N�^�[�̌���������ς���
            transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);

            // �ړ����\�Ȃ�
            if (canMove)
            {
                // ���̎��_�ňړ������̍��W�͌��܂��Ă���̂ŁA�\�񂵂Ă����Ȃ���
                // �G��AI�̃^�[���Ńv���C���[���ړ������̍��W�܂őI�����ɓ����Ă��܂�

                // ���݂̃^�C����̍��W���玩�g�̏����폜���Ă���
                mm.CurrentMap.SetMapTileActor(_currentPosXZ.x, _currentPosXZ.z, null);
                // �ړ���̍��W�Ɏ��g�̏���o�^���Ă���
                mm.CurrentMap.SetMapTileActor(_tartgetPosXZ.x, _tartgetPosXZ.z, this);
                // �ړ�����L�����N�^�[���Ƃ������Ƃ�PlaySceneManager�ɓ`���Ď���State�Ɉڍs����
                PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
                psm.AddMoveActor();
                psm.SetTurnState(TurnState.PlayerMoveStart);
            }
        }
    }

    /// <summary>�L�����N�^�[���ړ����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public override void MoveStart()
    {
        Debug.Log(gameObject.name + " �ړ��J�n���܂�");
        // �ڕW�̍��W�Ɍ����ړ�������
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
        // �U������}�X�̏����擾
        PosXZ target = GetTargetTile(_inputDir);
        ActorBase ab = FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor(target.x, target.z);
        // �U������}�X�ɓG������΃_���[�W�̏���
        ab?.Damaged();
        
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

    /// <summary>���̃L�����N�^�[���_���[�W���󂯂��Ƃ��ɌĂ΂�鏈��</summary>
    public override void Damaged()
    {
        _anim.Play("Fall");
    }
}
