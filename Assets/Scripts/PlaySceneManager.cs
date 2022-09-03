using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>�^�[�����̏�Ԃ�\��</summary>
public enum TurnState
{
    StandBy,            // ���o���Ȃ̂őҋ@
    Init,               // �^�[���̍ŏ��ɏ�����
    Input,              // �v���C���[�̓��͑҂�
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
    [SerializeField] EffectUIManager _effectUIManager;
    [SerializeField] MapManager _mapManager;
    /// <summary>���݂̃^�[�����ǂ̏�Ԃ���ێ����Ă���</summary>
    TurnState _currentTurnState;
    /// <summary>�v���C���[�𐧌䂷��</summary>
    PlayerManager _player;
    /// <summary>��������̓G�𐧌䂷��</summary>
    List<EnemyManager> _enemies = new List<EnemyManager>();
    /// <summary>
    /// ���̃^�[�����񂾃L�����N�^�[�̃��X�g�A
    /// ��������Ă�����s����o�����Ȃ̂Ń^�[���̍Ō�ɂ܂Ƃ߂ď���
    /// </summary>
    List<GameObject> _deadCharacters = new List<GameObject>();
    /// <summary>�c��^�[��</summary>
    int _remainingTurn;
    /// <summary>���݂̃X�R�A</summary>
    int _currentScore;
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
    /// <summary>�v���C���[������ł��܂�����</summary>
    bool _isPlayerDead;
    /// <summary>�v���C���[���S�[���̏�ɗ����Ă��邩</summary>
    bool _onGoalTile;
    /// <summary>���݂̃X�e�[�W�̃f�[�^��ێ����Ă���SO</summary>
    StageDataSO so;

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
    /// <summary>�X�R�A��ǉ�����</summary>
    public void AddScore(int add) => _playerUIManager.SetScore(_currentScore += add);
    /// <summary>�v���C���[�����S����</summary>
    public void PlayerIsDead() => _isPlayerDead = true;
    /// <summary>�v���C���[������ł��Ȃ������ꍇ�A�v���C���[���S�[���̏�ɗ������t���O�𗧂Ă�</summary>
    public void StandOnGoalTile() => _onGoalTile = !_isPlayerDead ? true : false;
    /// <summary>���̂����񂾃L�����N�^�[�̃��X�g�ɓo�^����</summary>
    public void AddDeadCharacter(GameObject corpse) => _deadCharacters.Add(corpse);

    /// <summary>
    /// �L�����N�^�[���ړ����I���邽�тɌĂ΂�A
    /// �S�����ړ����I������endActorsMove��true�ɂ���
    /// </summary>
    public void CheckRemMoveActor()
    {
        _moveActorCount--;
        _endActorMoveAll = _moveActorCount == 0;
    }

    void Awake()
    {
        _currentTurnState = TurnState.StandBy;
    }

    IEnumerator Start()
    {
        // ���݂̃X�e�[�W�̃f�[�^�𓮓I�ɓǂݍ���
        so = Resources.Load($"Stage_{GameManager._instance.CurrentStageNum}", typeof(StageDataSO)) as StageDataSO;
        // �}�b�v�𐶐�����
        _mapManager.Init(so);
        // �X�e�[�W���Ƀ��Z�b�g�����l��������
        _currentScore = GameManager._instance.TotalScore;
        _playerUIManager.SetScore(_currentScore);
        _playerUIManager.SetStageNum(GameManager._instance.CurrentStageNum);
        _remainingTurn = so.TurnLimit;

        // �t�F�[�h���I���܂ő҂�
        yield return new WaitWhile(() => GameManager._instance.IsFading);
        // �Q�[���X�^�[�g�̉��o
        yield return StartCoroutine(_effectUIManager.GameStartEffect(GameManager._instance.CurrentStageNum, so));
        // ���o���I������珔�X������������
        _currentTurnState = TurnState.Init;
    }

    void Update()
    {
        switch (_currentTurnState)
        {
            // ���o��
            case TurnState.StandBy:
                break;
            // �^�[���̍ŏ��ɏ���������
            case TurnState.Init:
                _player.TurnInit();
                _enemies.ForEach(e => e.TurnInit());
                _endActorMoveAll = false;
                _moveActorCount = 0;
                _actionActorCount = 0;
                _endActorAction = false;
                _playerUIManager.SetProgressTurn(_remainingTurn);
                _currentTurnState = TurnState.Input;
                break;
            // �v���C���[�̓��͂�҂�
            case TurnState.Input:
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
                // �v���C���[�����񂾁A�������͎��Ԑ؂�Ȃ�
                if (_isPlayerDead || _remainingTurn == 0)
                {
                    // �Q�[���I�[�o�[�̉��o���Ăяo���AState�����o���ɐ؂�ւ���
                    StartCoroutine(_effectUIManager.GameOverEffect());
                    _currentTurnState = TurnState.StandBy;
                }
                // �v���C���[���S�[���̏�ɏ���Ă�����
                else if (_onGoalTile)
                {
                    // �X�R�A���v�Z����
                    CalcScore();
                    // �X�e�[�W�N���A�̉��o���Ăяo���AState�����o���ɐ؂�ւ���
                    StartCoroutine(_effectUIManager.StageClearEffect(GameManager._instance.CurrentStageNum, so,
                        _mapManager.RemainingCoin(), _mapManager.RemainingEnemy(), _remainingTurn, _currentScore));
                    // �v�Z��̃X�R�A�����v�X�R�A�ɉ��Z����
                    GameManager._instance.UpdateTotalScore(_currentScore);
                    _currentTurnState = TurnState.StandBy;
                }
                else
                {
                    _currentTurnState = TurnState.Init;
                }
                // ���񂾃L�����N�^�[��S���폜����A�����ȊO�ł�Destroy���Ȃ�����
                _deadCharacters.ForEach(g => Destroy(g));
                _deadCharacters.Clear();
                // �c��^�[�������炷
                _remainingTurn--;
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
        // �v���C���[���ړ����I�������̏���������
        _player.MoveEnd();

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

    /// <summary>�X�R�A���v�Z����</summary>
    void CalcScore()
    {
        // �l�������R�C���̊���
        int coin = Mathf.FloorToInt(1.0f * _mapManager.RemainingCoin() / so.MaxCoin);
        // �|�����G�̊���
        int enemy = Mathf.FloorToInt(1.0f * _mapManager.RemainingEnemy() / so.MaxEnemy);
        // �o�߂����^�[���̊��� 
        int turn = Mathf.FloorToInt(1.0f * (so.TurnLimit - _remainingTurn) / so.TurnLimit);

        // TODO:�X�R�A�̌v�Z��
        _currentScore += coin * 100 + enemy * 100 + turn * 100;
    }

    /// <summary>���̃X�e�[�W�ɐi��</summary>
    public void MoveNextStage()
    {
        // �����Ō�̃X�e�[�W�Ȃ烊�U���g�ɔ��
        string scene = GameManager._instance.CheckAllClear() ? "Result" : "GamePlay";
        // �X�e�[�W�ԍ�����i�߂�
        GameManager._instance.AdvanceStageNum();
        GameManager._instance.FadeOut(scene);
    }

    /// <summary>�Q�[�������g���C����</summary>
    public void RetryGamePlay()
    {
        GameManager._instance.FadeOut("GamePlay");
        // �t�F�[�h������
    }

    /// <summary>�^�C�g���ɖ߂�</summary>
    public void MoveTitle()
    {
        GameManager._instance.FadeOut("Title");
        // �^�C�g���ɂ͂��ł��߂��悤�ɂ���A�~�X�����炷����蒼����悤��
    }
}
