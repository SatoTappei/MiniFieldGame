using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップに登場するキャラクターの基底クラス
/// </summary>
public abstract class ActorBase : MonoBehaviour
{
    /// <summary>XZ平面上での座標</summary>
    protected struct PosXZ
    {
        public int x;
        public int z;
    }

    /// <summary>キャラクターの方向</summary>
    public enum Direction
    {
        Neutral = 360, // 何も入力されていない状態
        Up = 0,
        Down = 180,
        Right = 90,
        Left = 270,
    };

    /// <summary>キャラクターが次のタイルに移動するのにかかる時間</summary>
    protected const float MoveTileTime = 30.0f;
    /// <summary>このキャラクターが侵入できるタイル</summary>
    [SerializeField] TileType[] _canMoveTile;
    /// <summary>現在のキャラクターの向き</summary>
    Direction _currentDir = Direction.Up;
    /// <summary>現在のXZ平面上での位置</summary>
    protected PosXZ _currentPosXZ;
    /// <summary>行動中かどうか</summary>
    protected bool _inAction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>ターンの最初に呼ばれる処理</summary>
    public abstract void TurnInit();

    /// <summary>キー入力待ち中に呼ばれる処理</summary>
    public abstract void StandBy();

    /// <summary>キャラクターが移動を開始するときに呼ばれる処理</summary>
    public abstract void MoveStart();

    /// <summary>キャラクターが移動中に呼ばれる処理</summary>
    public abstract void Move();

    /// <summary>キャラクターが移動を終えたときに呼ばれる処理</summary>
    public abstract void MoveEnd();

    /// <summary>キャラクターが行動を開始するときに呼ばれる処理</summary>
    public abstract void ActionStart();

    /// <summary>キャラクターが行動中に呼ばれる処理</summary>
    public abstract void Action();

    /// <summary>キャラクターが行動を終えるときに呼ばれる処理</summary>
    public abstract void ActionEnd();
}
