using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>キャラクターの方向</summary>
public enum ActorDir
{
    Neutral = 360, // 何も入力されていない状態
    Up = 0,
    Down = 180,
    Right = 90,
    Left = 270,
};

/// <summary>
/// マップに登場するキャラクターの基底クラス
/// </summary>
public abstract class ActorBase : MonoBehaviour
{
    /// <summary>このキャラクターが侵入できるタイル</summary>
    [SerializeField] TileType[] _canMoveTile;
    /// <summary>現在のキャラクターの向き</summary>
    ActorDir _currentDir = ActorDir.Up;
    /// <summary>行動中かどうか</summary>
    bool _inAction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
