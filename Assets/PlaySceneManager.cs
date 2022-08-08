using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    Init,               // ターンの最初に初期化
    StandBy,            // プレイヤーの入力待ち
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
    /// <summary>現在のターンがどの状態かを保持しておく</summary>
    TurnState _currentTurnState;
    /// <summary>プレイヤーを制御する</summary>
    PlayerManager _player;
    /// <summary>たくさんの敵さんを制御する</summary>
    List<EnemyManager> _enemies = new List<EnemyManager>();

    /// <summary>
    /// スクリプトの外からStateを進めることがある
    /// 現状:StandByから入力を受け取って移動か行動に分岐
    /// </summary>
    public void SetTurnState(TurnState state) => _currentTurnState = state;
    /// <summary>このスクリプトのStateで管理するためにプレイヤー側から自身をセットする</summary>
    public void SetPlayer(PlayerManager player) => _player = player;
    /// <summary>このスクリプトのStateで管理するために敵側から自身をセットする</summary>
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
            // ターンの最初に初期化する
            case TurnState.Init:
                _player.TurnInit();
                _enemies.ForEach(e => e.TurnInit());
                _currentTurnState = TurnState.StandBy;
                break;
            // プレイヤーの入力を待つ
            case TurnState.StandBy:
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

    // プレイヤーが移動をするターンの処理
    IEnumerator ProcPlayerMove()
    {
        // 敵全員が行動を決定する
        _enemies.ForEach(e => e.RequestAI());
        // プレイヤーが移動する
        _player.MoveStart();
        // 敵が移動する
        _enemies.Where(e => e.DoActionThisTurn).ToList().ForEach(e => e.ActionStart());
        // TODO:敵が移動終わるまで次の処理に進まないようにする
        yield return null;
        // 敵が行動する
        _enemies.Where(e => !e.DoActionThisTurn).ToList().ForEach(e => e.MoveStart());
        // TODO:敵が行動終わるまで次の処理に進まないようにする
        yield return null;
        _currentTurnState = TurnState.TurnEnd;
    }

    // プレイヤーが行動をするターンの処理
    IEnumerator ProcPlayerAction()
    {
        // プレイヤーが行動する
        _player.ActionStart();
        // 敵全員が行動を決定する
        _enemies.ForEach(e => e.RequestAI());
        // 敵が行動する
        _enemies.Where(e => e.DoActionThisTurn).ToList().ForEach(e => e.ActionStart());
        // TODO:敵が行動終わるまで次の処理に進まないようにする
        yield return null;
        // 敵を移動させる
        _enemies.Where(e => !e.DoActionThisTurn).ToList().ForEach(e => e.MoveStart());
        // TODO:敵が移動終わるまで次の処理に進まないようにする
        yield return null;
        _currentTurnState = TurnState.TurnEnd;
    }
}
