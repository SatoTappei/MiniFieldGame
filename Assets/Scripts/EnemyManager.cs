using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�𐧌䂷��
/// </summary>
public class EnemyManager : CharacterBase
{
    // �G�̍s���^�C�v
    enum ActionType
    {
        ActionAndMove,  // �U���ƈړ����s��
        MoveOnly,       // �ړ������s��Ȃ�
    }

    /// <summary>�v���C���[�𔭌����ɕ\�������A�C�R��</summary>
    [SerializeField] GameObject _noticeIcon;
    /// <summary>���E�Ƃ��Ĕ�΂�Ray�̒���</summary>
    [SerializeField] int _sightRange;
    /// <summary>�G�̍s���^�C�v</summary>
    [SerializeField] ActionType _actionType;
    /// <summary>�U�����鑊�肪������W</summary>
    PosXZ _attackTargetPos;
    /// <summary>���̃^�[���ɍU��������ꍇ��true�A�ړ��̏ꍇ��false�ɂȂ�</summary>
    bool _doActionThisTurn;
    /// <summary>�v���C���[�𔭌����Ă��邩</summary>
    bool _isNotice;

    /// <summary>PlaySceneManager�����̃v���p�e�B�����ă��\�b�h���Ăяo��</summary>
    public bool DoActionThisTurn { get => _doActionThisTurn; }

    void Awake()
    {
        _noticeIcon.SetActive(false);
    }

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
        // �v���C���[�����E�ɓ����Ă��邩
        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 origin = new Vector3(transform.position.x, 1, transform.position.z);
        if (Physics.Raycast(origin, dir, out RaycastHit hit, _sightRange))
        {
            Debug.DrawRay(origin, dir * _sightRange, Color.red, 5);
            // ��΂���Ray���v���C���[�Ƀq�b�g������v���C���[�𔭌����Ă���
            _isNotice = hit.collider.gameObject.tag == "Player" ? true : false;
            _noticeIcon.SetActive(_isNotice);
        }

        // �ړ��������Ȃ��G�Ȃ�K���ړ����s��
        if (_actionType == ActionType.MoveOnly)
        {
            FindObjectOfType<PlaySceneManager>().AddMoveActor();
            return;
        }

        // �v���C���[�������̏㉺���E�}�X�ɂ���ꍇ�͍U������
        // ����ȊO�̏ꍇ�͈ړ�����
        MapManager mm = FindObjectOfType<MapManager>();
        for (int i = 0; i < 4; i++)
        {
            // ��Direction�̒l�Ɋ��蓖�Ă�������ς���Ƃ��������Ȃ�̂Œ���
            ActorDir checkDir = (ActorDir)(i * 90);
            PosXZ pos = ActorUtility.GetTargetTile(_currentPosXZ, checkDir);
            CharacterBase cb = mm.CurrentMap.GetMapTileActor(pos.x, pos.z);
            _doActionThisTurn = cb != null && cb.GetCharacterType() == CharacterType.Player ? true : false;
            // �U������ꍇ�͓G�̕��������悤�Ɍ����ƍU������^�C����ێ����Ă���
            if (_doActionThisTurn)
            {
                _inputDir = checkDir;
                _attackTargetPos = pos;
                break;
            }
        }

        if(_doActionThisTurn)
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
        MapManager mm = FindObjectOfType<MapManager>();

        // �v���C���[�𔭌����Ă���ꍇ�̓A���S���Y�����g�p���ē�����
        if (_isNotice)
        {
            PosXZ playerPos = FindObjectOfType<PlayerManager>().CurrentPosXZ;
            _inputDir = GetComponent<MoveAlgorithmBase>().GetMoveDirection(_currentPosXZ, playerPos);
            // �ړ���̍��W���擾
            _targetPosXZ = ActorUtility.GetTargetTile(_currentPosXZ, _inputDir);
            canMove = mm.CheckCanMoveTile(_targetPosXZ.x, _targetPosXZ.z);
        }
        // �v���C���[�𔭌����Ă��Ȃ��ꍇ��4���������ꂩ�Ƀ����_���Ɉړ�����
        else
        {
            List<(int, int)> dirs = new List<(int, int)>() { (1, 0), (0, 1), (-1, 0), (0, -1) };

            // �ǂ��ɂ��ړ��ł��Ȃ��ꍇ�͂��̃^�[���͂��̏�ɂƂǂ܂�
            while (!canMove && dirs.Count > 0)
            {
                int r = Random.Range(0, dirs.Count);
                _inputDir = GetKeyToDir(dirs[r].Item1, dirs[r].Item2);
                // �ړ����悤�Ƃ��Ă���^�C�����ړ��ł��邩�ǂ����𒲂ׂ� <= �ϐ�dirs��x��z������ւ���Ă���̂Œ���
                canMove = mm.CheckCanMoveTile(_currentPosXZ.x + dirs[r].Item2, _currentPosXZ.z + dirs[r].Item1);
                dirs.RemoveAt(r);
            }
        }
        
        if (canMove)
        {
            // �ړ���̍��W���擾
            _targetPosXZ = ActorUtility.GetTargetTile(_currentPosXZ, _inputDir);
            // ���݂̃^�C����̍��W���玩�g�̏����폜���Ă���
            mm.CurrentMap.SetMapTileCharacter(_currentPosXZ.x, _currentPosXZ.z, null);
            // �ړ���̍��W�Ɏ��g�̏���o�^���Ă���
            mm.CurrentMap.SetMapTileCharacter(_targetPosXZ.x, _targetPosXZ.z, this);
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

        // �^�[�Q�b�g�̕����������čU������
        PosXZ target = _attackTargetPos;
        transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);

        CharacterBase cb = FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor(target.x, target.z);
        // �U������}�X�Ƀv���C���[������΃_���[�W�̏���
        if (cb != null && cb.GetCharacterType() == CharacterType.Player)
        {
            cb.Damaged(_inputDir);
            SoundManager._instance.Play("SE_�a��");
        }
        else
        {
            SoundManager._instance.Play("SE_�~�X");
        }
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

    /// <summary>
    /// ���̃L�����N�^�[���_���[�W���󂯂��Ƃ��ɌĂ΂�鏈��
    /// </summary>
    /// <param name="attackedDir">�U�����ꂽ����</param>
    public override void Damaged(ActorDir attackedDir)
    {
        FindObjectOfType<ActionLogManager>().DispLog(_defeatedMessage);
        // ���g�����񂾂��Ƃ�PlaySceneManager�ɓ`����
        PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
        psm.RemoveEnemy(this);
        psm.AddDeadCharacter(gameObject);
        // �^�C����̏����폜����
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileCharacter(_currentPosXZ.x, _currentPosXZ.z, null);
        // ���S�̉��o���s��
        Death(attackedDir);
        _noticeIcon.SetActive(false);
        //// ���S�̃A�j���[�V�������Đ�(�X�P�[����0�ɂ��Č����Ȃ�����)
        //_anim.Play("Dead");
        //// ��_���[�W�̃G�t�F�N�g�A�����o�����̕\���ƌ����܂�̐���
        //_damageEffect.SetActive(true);
        //Instantiate(_decalEffect, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.Euler(90, 0, 0));
        //// ���O�h�[���𐶐����čU�����ꂽ�����Ƃ͋t�ɐ�����΂�
        //var Obj = Instantiate(_ragDoll, transform.position, Quaternion.identity);
        //Vector3 vec = DirectionToVec3(attackedDir);
        //Obj.GetComponent<RagDollController>().Dir = new Vector3(vec.x, 0.5f, vec.z).normalized;
    }
}
