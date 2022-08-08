using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

/// <summary>�^�[�����̏�Ԃ�\��</summary>
//public enum TurnState
//{
//    Init,
//    Standby,    // �v���C���[�̓��͑҂�
//    MoveStart,
//    Move,
//    MoveEnd,
//    ActionStart,
//    Action,
//    ActionEnd,
//}

/// <summary>�^�[�����̏�Ԃ�\��</summary>
public enum TurnState
{
    Init,               // �^�[���̍ŏ��ɏ�����
    StandBy,            // �v���C���[�̓��͑҂�
    PlayerMoveStart,    // �v���C���[���ړ���I������
    PlayerMove,     
    //PlayerMoveEnd,
    PlayerActionStart,  // �v���C���[���s����I������
    PlayerAction,       
    //PlyaerActionEnd,
    TurnEnd,
}

/// <summary>
/// �Q�[���̐i�s���Ǘ��E���䂷��
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    /// <summary>���݂̃^�[�����ǂ̏�Ԃ���ێ����Ă���</summary>
    TurnState _currentTurnState;
    /// <summary>�v���C���[�𐧌䂷��</summary>
    PlayerManager _player;
    /// <summary>��������̓G����𐧌䂷��</summary>
    List<EnemyManager> _enemies = new List<EnemyManager>();

    /// <summary>
    /// �X�N���v�g�̊O����State��i�߂邱�Ƃ�����
    /// ����:StandBy������͂��󂯎���Ĉړ����s���ɕ���
    /// </summary>
    public void SetTurnState(TurnState state) => _currentTurnState = state;
    /// <summary>���̃X�N���v�g��State�ŊǗ����邽�߂Ƀv���C���[�����玩�g���Z�b�g����</summary>
    public void SetPlayer(PlayerManager player) => _player = player;
    /// <summary>���̃X�N���v�g��State�ŊǗ����邽�߂ɓG�����玩�g���Z�b�g����</summary>
    public void AddEnemy(EnemyManager enemy) => _enemies.Add(enemy);

    void Awake()
    {
        _currentTurnState = TurnState.Init;
    }

    void Start()
    {

    }

    void Update()
    {
        switch (_currentTurnState)
        {
            // �^�[���̍ŏ��ɏ���������
            case TurnState.Init:
                _player.TurnInit();
                _enemies.ForEach(e => e.TurnInit());
                _currentTurnState = TurnState.StandBy;
                break;
            // �v���C���[�̓��͂�҂�
            case TurnState.StandBy:
                _player.StandBy();
                _enemies.ForEach(e => e.StandBy());
                break;
            // �v���C���[���ړ���I�������ꍇ�̏���
            case TurnState.PlayerMoveStart:
                StartCoroutine(ProcPlayerMove());
                _currentTurnState = TurnState.PlayerMove;
                break;
            case TurnState.PlayerMove:
                // �v���C���[���ړ�������^�[�����A���t���[�����s���鏈��
                break;
            // �v���C���[���s����I�������ꍇ�̏���
            case TurnState.PlayerActionStart:
                StartCoroutine(ProcPlayerAction());
                _currentTurnState = TurnState.PlayerAction;
                break;
            case TurnState.PlayerAction:
                // �v���C���[���s��������^�[�����A���t���[�����s���鏈��
                break;
            // �^�[���̏I�����̏���
            case TurnState.TurnEnd:
                _currentTurnState = TurnState.Init;
                break;
        }
    }

    // �v���C���[���ړ�������^�[���̏���
    IEnumerator ProcPlayerMove()
    {
        // �G�S�����s�������肷��
        _enemies.ForEach(e => e.RequestAI());
        // �v���C���[���ړ�����
        _player.MoveStart();
        // �G���ړ�����
        _enemies.Where(e => e.DoActionThisTurn).ToList().ForEach(e => e.ActionStart());
        // TODO:�G���ړ��I���܂Ŏ��̏����ɐi�܂Ȃ��悤�ɂ���
        yield return null;
        // �G���s������
        _enemies.Where(e => !e.DoActionThisTurn).ToList().ForEach(e => e.MoveStart());
        // TODO:�G���s���I���܂Ŏ��̏����ɐi�܂Ȃ��悤�ɂ���
        yield return null;
        _currentTurnState = TurnState.TurnEnd;
    }

    // �v���C���[���s��������^�[���̏���
    IEnumerator ProcPlayerAction()
    {
        // �v���C���[���s������
        _player.ActionStart();
        // �G�S�����s�������肷��
        _enemies.ForEach(e => e.RequestAI());
        // �G���s������
        _enemies.Where(e => e.DoActionThisTurn).ToList().ForEach(e => e.ActionStart());
        // TODO:�G���s���I���܂Ŏ��̏����ɐi�܂Ȃ��悤�ɂ���
        yield return null;
        // �G���ړ�������
        _enemies.Where(e => !e.DoActionThisTurn).ToList().ForEach(e => e.MoveStart());
        // TODO:�G���ړ��I���܂Ŏ��̏����ɐi�܂Ȃ��悤�ɂ���
        yield return null;
        _currentTurnState = TurnState.TurnEnd;
    }
}
