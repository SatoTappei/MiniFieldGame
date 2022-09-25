using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�𐧌䂷��
/// </summary>
public class PlayerManager : CharacterBase
{
    /// <summary>�_���[�W���󂯂��ۂɕ\������郁�b�Z�[�W</summary>
    [SerializeField] string _damagedMessage;
    /// <summary>�̗͂�1�ɂȂ����Ƃ��ɕ\������鉉�o</summary>
    //[SerializeField] GameObject _dyingEffect;
    /// <summary>
    /// �v���C���[�̗̑́A
    /// 3�ȏ�ɂ���ɂ�UI�̃A�C�R�������₳�Ȃ��Ƃ����Ȃ��̂ŃC���X�y�N�^�[�ɂ͕\�������Ȃ�
    /// </summary>
    int _lifePoint = 3;

    /// <summary>�A�C�e�����l������2�}�X��ɍU���ł����Ԃ�</summary>
    bool _powerUp;

    /// <summary>�p���[�A�b�v��ԂɂȂ�</summary>
    public void SetPowerUp() => _powerUp = true;

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
            _targetPosXZ = ActorUtility.GetTargetTile(_currentPosXZ, _inputDir);
            // �ړ��̉\�s�\�Ɍ��炸���͂��ꂽ�����ɃL�����N�^�[�̌���������ς���
            transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);

            // �ړ����\�Ȃ�
            if (canMove)
            {
                // ���̎��_�ňړ������̍��W�͌��܂��Ă���̂ŁA�\�񂵂Ă����Ȃ���
                // �G��AI�̃^�[���Ńv���C���[���ړ������̍��W�܂őI�����ɓ����Ă��܂�

                // ���݂̃^�C����̍��W���玩�g�̏����폜���Ă���
                mm.CurrentMap.SetMapTileCharacter(_currentPosXZ.x, _currentPosXZ.z, null);
                // �ړ���̍��W�Ɏ��g�̏���o�^���Ă���
                mm.CurrentMap.SetMapTileCharacter(_targetPosXZ.x, _targetPosXZ.z, this);
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
        StartCoroutine(Move(_targetPosXZ));
        // �ړ���̍��W�ɃA�C�e�������邩�m�F���āA����ꍇ�͊l�����̏��������s
        ItemManager im = FindObjectOfType<MapManager>().CurrentMap.GetMapTileItem(_targetPosXZ.x, _targetPosXZ.z);
        im?.GetThisItem();
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
        // �v���C���[���S�[���̏�ɗ����Ă�����t���O�𗧂Ă�
        if (FindObjectOfType<MapManager>().CurrentMap.GetMapTile(_currentPosXZ.x, _currentPosXZ.z).Char == 'E')
            FindObjectOfType<PlaySceneManager>().StandOnGoalTile();
    }

    /// <summary>�L�����N�^�[���s�����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public override void ActionStart()
    {
        _anim.Play("Slash");
        MapManager mm = FindObjectOfType<MapManager>();
        // �U������}�X�̏����擾
        PosXZ target = ActorUtility.GetTargetTile(_currentPosXZ, _inputDir);
        CharacterBase cb = mm.CurrentMap.GetMapTileActor(target.x, target.z);
        // �U������}�X�ɓG������΃_���[�W�̏���
        cb?.Damaged(_inputDir);
        // �p���[�A�b�v��ԂȂ����1�}�X��̓G�ɂ��U������
        CharacterBase bcb = null;
        if (_powerUp)
        {
            PosXZ backTarget = ActorUtility.GetTargetTile(target, _inputDir);
            bcb = mm.CurrentMap.GetMapTileActor(backTarget.x, backTarget.z);
            bcb?.Damaged(_inputDir);
        }
        SoundManager._instance.Play(cb || bcb ? "SE_�a��" : "SE_�~�X");
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

    /// <summary>�^�[���I�����ɌĂ΂�鏈��</summary>
    public override void TurnEnd()
    {

    }

    /// <summary>���̃L�����N�^�[���_���[�W���󂯂��Ƃ��ɌĂ΂�鏈��</summary>
    public override void Damaged(ActorDir attackedDir)
    {
        // �U�����ꂽ�ۂɊ��ɑ̗͂�0�Ȃ牽�����Ȃ�
        if (_lifePoint <= 0)
        {
            FindObjectOfType<PlaySceneManager>().SendEndAction();
            return;
        }

        _anim.Play("Damage");
        // ��_���[�W�̃G�t�F�N�g�𐶐�����A���������L�����N�^�[�̋��̈ʒu�ɐݒ肷��
        if (_damageEffect != null)
            Instantiate(_damageEffect, new Vector3(transform.position.x, 0.9f, transform.position.z), Quaternion.identity);
        // �̗͂����炵��UI�ɔ��f������AHP��0��菬�����Ȃ��Ă��܂����A�Ăяo�������\�b�h���Œe���̂ō��̂Ƃ���͖��Ȃ�
        _lifePoint--;
        FindObjectOfType<PlayerUIManager>().DecreaseLifePoint(_lifePoint);

        // �̗͂�1�ɂȂ�����m���̉��o���s��
        //if (_lifePoint == 1)
        //{
        //    Instantiate(_dyingEffect, Vector3.zero, Quaternion.identity);
        //}
        // �̗͂�0�ɂȂ�����
        if (_lifePoint == 0) 
        {
            FindObjectOfType<ActionLogManager>().DispLog(_defeatedMessage);
            // �v���C���[�����񂾂��Ƃ�ʒm����
            PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
            psm.PlayerIsDead();
            psm.AddDeadCharacter(gameObject);
            // ���S�̉��o���s��
            Death(attackedDir);
            //// ���S�̃A�j���[�V�������Đ�
            //_anim.Play("Dead");
            //// ���O�h�[���𐶐����čU�����ꂽ�����Ƃ͋t�ɐ�����΂�
            //var obj = Instantiate(_ragDoll, transform.position, Quaternion.identity);
            //Vector3 vec = DirectionToVec3(attackedDir);
            //obj.GetComponent<RagDollController>().Dir = new Vector3(vec.x, 0.5f, vec.z).normalized;
        }
        else
        {
            FindObjectOfType<ActionLogManager>().DispLog(_damagedMessage);
        }
    }
}
