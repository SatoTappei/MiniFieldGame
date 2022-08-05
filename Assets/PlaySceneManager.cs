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
    PlayerMoveEnd,
    PlayerActionStart,  // プレイヤーが行動を選択した
    PlayerAction,       
    PlyaerActionEnd,
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
    ActorBase _player;
    /// <summary>敵さんを制御する</summary>
    List<ActorBase> _enemies = new List<ActorBase>();

    /// <summary>
    /// スクリプトの外からStateを進めることがある
    /// 現状:StandByから入力を受け取って移動か行動に分岐
    /// </summary>
    public void SetTurnState(TurnState state) => _currentTurnState = state;
    /// <summary>このスクリプトのStateで管理するためにプレイヤー側から自身をセットする</summary>
    public void SetPlayer(ActorBase player) => _player = player;
    /// <summary>このスクリプトのStateで管理するために敵側から自身をセットする</summary>
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
            // プレイヤーが行動を選択した場合の処理
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
            // ターンの終了時の処理
            case TurnState.TurnEnd:
                break;
        }
    }
}
