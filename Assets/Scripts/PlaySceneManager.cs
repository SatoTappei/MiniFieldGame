using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>ターン中の状態を表す</summary>
//public enum TurnState
//{
//    Init,
//    Standby,    // プレイヤーの入力待ち
//    MoveStart,
//    Move,
//    MoveEnd,
//    ActionStart,
//    Action,
//    ActionEnd,
//}

/// <summary>ターン中の状態を表す</summary>
public enum TurnState
{
    StandBy,            // 演出中なので待機
    Init,               // ターンの最初に初期化
    Input,              // プレイヤーの入力待ち
    PlayerMoveStart,    // プレイヤーが移動を選択した
    PlayerMove,     
    //PlayerMoveEnd,
    PlayerActionStart,  // プレイヤーが行動を選択した
    PlayerAction,       
    //PlyaerActionEnd,
    TurnEnd,
}

/// <summary>
/// ゲームの進行を管理・制御する
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    [SerializeField] PlayerUIManager _playerUIManager;
    [SerializeField] EffectUIManager _effectUIManager;
    /// <summary>現在のターンがどの状態かを保持しておく</summary>
    TurnState _currentTurnState;
    /// <summary>プレイヤーを制御する</summary>
    PlayerManager _player;
    /// <summary>たくさんの敵さんを制御する</summary>
    List<EnemyManager> _enemies = new List<EnemyManager>();
    /// <summary>ゲーム開始時からの経過ターン</summary>
    int _progressTurn;
    /// <summary>現在のスコア</summary>
    int _currentScore;
    /// <summary>このターン移動するキャラクターが全員移動したらtrueになる</summary>
    bool _endActorMoveAll;
    /// <summary>このターン移動するキャラクターの数</summary>
    int _moveActorCount;
    /// <summary>キャラクターが行動中はfalse、終わったらtrueになって次のキャラの行動に移る</summary>
    bool _endActorAction;
    /// <summary>このターン行動するキャラクターが全員移動したらtrueになる</summary>
    bool _endActorActionAll;
    /// <summary>このターン行動するキャラクターの数</summary>
    int _actionActorCount;

    /// <summary>
    /// スクリプトの外からStateを進めることがある
    /// 現状:StandByから入力を受け取って移動か行動に分岐
    /// </summary>
    public void SetTurnState(TurnState state) => _currentTurnState = state;
    /// <summary>このスクリプトのStateで管理するためにプレイヤー側から自身をセットする</summary>
    public void SetPlayer(PlayerManager player) => _player = player;
    /// <summary>このスクリプトのStateで管理するために敵側から自身をセットする</summary>
    public void AddEnemy(EnemyManager enemy) => _enemies.Add(enemy);
    /// <summary>このスクリプトのStateで管理するために敵側から自身が死んだことを伝える</summary>
    public void RemoveEnemy(EnemyManager em) => _enemies.Remove(em);
    /// <summary>このターン移動するキャラクターとして追加する</summary>
    public void AddMoveActor() => _moveActorCount++;
    /// <summary>このターン行動するキャラクターとして追加する</summary>
    public void AddActionActor() => _actionActorCount++;
    /// <summary>キャラクターのスクリプトからこのターンの行動が終わったことを通知する</summary>
    public void SendEndAction() => _endActorAction = true;
    /// <summary>スコアを追加する</summary>
    public void AddScore(int add) => _playerUIManager.SetScore(_currentScore += add);

    /// <summary>
    /// キャラクターが移動を終えるたびに呼ばれ、
    /// 全員が移動を終えたらendActorsMoveをtrueにする
    /// </summary>
    public void CheckRemMoveActor()
    {
        _moveActorCount--;
        _endActorMoveAll = _moveActorCount == 0;
        Debug.Log(_moveActorCount);
    }

    void Awake()
    {
        _currentTurnState = TurnState.StandBy;
    }

    IEnumerator Start()
    {
        // ゲームスタートの演出
        // GameManagerからステージの情報(マップ情報、敵の数、コインの数、ターン制限)を取得
        // effectUIManagerに渡して使ってもらう
        yield return StartCoroutine(_effectUIManager.GameStartEffect());
        // 演出が終わったら諸々を初期化する
        _currentTurnState = TurnState.Init;
    }

    void Update()
    {
        switch (_currentTurnState)
        {
            // 演出中
            case TurnState.StandBy:
                Debug.Log("演出中です");
                break;
            // ターンの最初に初期化する
            case TurnState.Init:
                _player.TurnInit();
                _enemies.ForEach(e => e.TurnInit());
                _endActorMoveAll = false;
                _moveActorCount = 0;
                _actionActorCount = 0;
                _endActorAction = false;
                _playerUIManager.SetProgressTurn(++_progressTurn);
                _currentTurnState = TurnState.Input;
                break;
            // プレイヤーの入力を待つ
            case TurnState.Input:
                _player.StandBy();
                _enemies.ForEach(e => e.StandBy());
                break;
            // プレイヤーが移動を選択した場合の処理
            case TurnState.PlayerMoveStart:
                StartCoroutine(ProcPlayerMove());
                _currentTurnState = TurnState.PlayerMove;
                break;
            case TurnState.PlayerMove:
                // プレイヤーが移動をするターン中、毎フレーム実行する処理
                break;
            // プレイヤーが行動を選択した場合の処理
            case TurnState.PlayerActionStart:
                StartCoroutine(ProcPlayerAction());
                _currentTurnState = TurnState.PlayerAction;
                break;
            case TurnState.PlayerAction:
                // プレイヤーが行動をするターン中、毎フレーム実行する処理
                break;
            // ターンの終了時の処理
            case TurnState.TurnEnd:
                _currentTurnState = TurnState.Init;
                break;
        }
    }

    /// <summary>プレイヤーが移動をするターンの処理</summary>
    IEnumerator ProcPlayerMove()
    {
        // 敵全員が行動を決定する
        _enemies.ForEach(e => e.RequestAI());
        // プレイヤーが移動する
        _player.MoveStart();
        // 敵が移動する
        _enemies.Where(e => !e.DoActionThisTurn).ToList().ForEach(e => e.MoveStart());
        // 移動するキャラクターが全員終わるまで次の処理に進まないようにする
        yield return new WaitUntil(() => _endActorMoveAll);
        // 敵が順番に行動する
        foreach (EnemyManager e in _enemies.Where(e => e.DoActionThisTurn))
        {
            _endActorAction = false;
            e.ActionStart();
            yield return new WaitUntil(() => _endActorAction);
        }

        _currentTurnState = TurnState.TurnEnd;
    }

    /// <summary>プレイヤーが行動をするターンの処理</summary>
    IEnumerator ProcPlayerAction()
    {
        // プレイヤーが行動する
        _player.ActionStart();
        // プレイヤーが行動終わるまで次の処理に進まないようにする
        yield return new WaitUntil(() => _endActorAction);
        // 敵全員が行動を決定する
        _enemies.ForEach(e => e.RequestAI());
        // 敵が順番に行動する
        foreach (EnemyManager e in _enemies.Where(e => e.DoActionThisTurn))
        {
            _endActorAction = false;
            e.ActionStart();
            yield return new WaitUntil(() => _endActorAction);
        }
        // 移動を選択した敵がいたら
        if (_moveActorCount > 0)
        {
            // 敵を移動させる
            _enemies.Where(e => !e.DoActionThisTurn).ToList().ForEach(e => e.MoveStart());
            // 敵が全員移動を終えるまで次の処理に進まないようにする
            yield return new WaitUntil(() => _endActorMoveAll);
        }

        _currentTurnState = TurnState.TurnEnd;
    }
}
