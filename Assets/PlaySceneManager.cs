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
    PlayerMoveEnd,
    PlayerActionStart,  // �v���C���[���s����I������
    PlayerAction,       
    PlyaerActionEnd,
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
    ActorBase _player;
    /// <summary>�G����𐧌䂷��</summary>
    List<ActorBase> _enemies = new List<ActorBase>();

    /// <summary>
    /// �X�N���v�g�̊O����State��i�߂邱�Ƃ�����
    /// ����:StandBy������͂��󂯎���Ĉړ����s���ɕ���
    /// </summary>
    public void SetTurnState(TurnState state) => _currentTurnState = state;
    /// <summary>���̃X�N���v�g��State�ŊǗ����邽�߂Ƀv���C���[�����玩�g���Z�b�g����</summary>
    public void SetPlayer(ActorBase player) => _player = player;
    /// <summary>���̃X�N���v�g��State�ŊǗ����邽�߂ɓG�����玩�g���Z�b�g����</summary>
    public void AddEnemy(ActorBase enemy) => _enemies.Add(enemy);

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
                _player.MoveStart();
                _enemies.ForEach(e => e.MoveStart());
                break;
            case TurnState.PlayerMove:
                _player.Move();
                _enemies.ForEach(e => e.Move());
                break;
            case TurnState.PlayerMoveEnd:
                _player.MoveEnd();
                _enemies.ForEach(e => e.MoveEnd());
                break;
            // �v���C���[���s����I�������ꍇ�̏���
            case TurnState.PlayerActionStart:
                _player.ActionStart();
                _enemies.ForEach(e => e.ActionStart());
                break;
            case TurnState.PlayerAction:
                _player.Action();
                _enemies.ForEach(e => e.Action());
                break;
            case TurnState.PlyaerActionEnd:
                _player.ActionEnd();
                _enemies.ForEach(e => e.ActionEnd());
                break;
            // �^�[���̏I�����̏���
            case TurnState.TurnEnd:
                break;
        }
    }
}
