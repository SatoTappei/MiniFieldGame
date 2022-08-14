using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] PlayerUIManager _playerUIManager;
    /// <summary>���݂̃^�[�����ǂ̏�Ԃ���ێ����Ă���</summary>
    TurnState _currentTurnState;
    /// <summary>�v���C���[�𐧌䂷��</summary>
    PlayerManager _player;
    /// <summary>��������̓G����𐧌䂷��</summary>
    List<EnemyManager> _enemies = new List<EnemyManager>();
    /// <summary>�Q�[���J�n������̌o�߃^�[��</summary>
    int _progressTurn;
    /// <summary>���̃^�[���ړ�����L�����N�^�[���S���ړ�������true�ɂȂ�</summary>
    bool _endActorMoveAll;
    /// <summary>���̃^�[���ړ�����L�����N�^�[�̐�</summary>
    int _moveActorCount;
    /// <summary>�L�����N�^�[���s������false�A�I�������true�ɂȂ��Ď��̃L�����̍s���Ɉڂ�</summary>
    bool _endActorAction;
    /// <summary>���̃^�[���s������L�����N�^�[���S���ړ�������true�ɂȂ�</summary>
    bool _endActorActionAll;
    /// <summary>���̃^�[���s������L�����N�^�[�̐�</summary>
    int _actionActorCount;

    /// <summary>
    /// �X�N���v�g�̊O����State��i�߂邱�Ƃ�����
    /// ����:StandBy������͂��󂯎���Ĉړ����s���ɕ���
    /// </summary>
    public void SetTurnState(TurnState state) => _currentTurnState = state;
    /// <summary>���̃X�N���v�g��State�ŊǗ����邽�߂Ƀv���C���[�����玩�g���Z�b�g����</summary>
    public void SetPlayer(PlayerManager player) => _player = player;
    /// <summary>���̃X�N���v�g��State�ŊǗ����邽�߂ɓG�����玩�g���Z�b�g����</summary>
    public void AddEnemy(EnemyManager enemy) => _enemies.Add(enemy);
    /// <summary>���̃X�N���v�g��State�ŊǗ����邽�߂ɓG�����玩�g�����񂾂��Ƃ�`����</summary>
    public void RemoveEnemy(EnemyManager em) => _enemies.Remove(em);
    /// <summary>���̃^�[���ړ�����L�����N�^�[�Ƃ��Ēǉ�����</summary>
    public void AddMoveActor() => _moveActorCount++;
    /// <summary>���̃^�[���s������L�����N�^�[�Ƃ��Ēǉ�����</summary>
    public void AddActionActor() => _actionActorCount++;
    /// <summary>�L�����N�^�[�̃X�N���v�g���炱�̃^�[���̍s�����I��������Ƃ�ʒm����</summary>
    public void SendEndAction() => _endActorAction = true;

    /// <summary>
    /// �L�����N�^�[���ړ����I���邽�тɌĂ΂�A
    /// �S�����ړ����I������endActorsMove��true�ɂ���
    /// </summary>
    public void CheckRemMoveActor()
    {
        _moveActorCount--;
        _endActorMoveAll = _moveActorCount == 0;
        Debug.Log(_moveActorCount);
    }

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
                _endActorMoveAll = false;
                _moveActorCount = 0;
                _actionActorCount = 0;
                _endActorAction = false;
                _playerUIManager.SetProgressTurn(++_progressTurn);
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

    /// <summary>�v���C���[���ړ�������^�[���̏���</summary>
    IEnumerator ProcPlayerMove()
    {
        // �G�S�����s�������肷��
        _enemies.ForEach(e => e.RequestAI());
        // �v���C���[���ړ�����
        _player.MoveStart();
        // �G���ړ�����
        _enemies.Where(e => !e.DoActionThisTurn).ToList().ForEach(e => e.MoveStart());
        // �ړ�����L�����N�^�[���S���I���܂Ŏ��̏����ɐi�܂Ȃ��悤�ɂ���
        yield return new WaitUntil(() => _endActorMoveAll);
        // �G�����Ԃɍs������
        foreach (EnemyManager e in _enemies.Where(e => e.DoActionThisTurn))
        {
            _endActorAction = false;
            e.ActionStart();
            yield return new WaitUntil(() => _endActorAction);
        }

        _currentTurnState = TurnState.TurnEnd;
    }

    /// <summary>�v���C���[���s��������^�[���̏���</summary>
    IEnumerator ProcPlayerAction()
    {
        // �v���C���[���s������
        _player.ActionStart();
        // �v���C���[���s���I���܂Ŏ��̏����ɐi�܂Ȃ��悤�ɂ���
        yield return new WaitUntil(() => _endActorAction);
        // �G�S�����s�������肷��
        _enemies.ForEach(e => e.RequestAI());
        // �G�����Ԃɍs������
        foreach (EnemyManager e in _enemies.Where(e => e.DoActionThisTurn))
        {
            _endActorAction = false;
            e.ActionStart();
            yield return new WaitUntil(() => _endActorAction);
        }
        // �ړ���I�������G��������
        if (_moveActorCount > 0)
        {
            // �G���ړ�������
            _enemies.Where(e => !e.DoActionThisTurn).ToList().ForEach(e => e.MoveStart());
            // �G���S���ړ����I����܂Ŏ��̏����ɐi�܂Ȃ��悤�ɂ���
            yield return new WaitUntil(() => _endActorMoveAll);
        }

        _currentTurnState = TurnState.TurnEnd;
    }
}
