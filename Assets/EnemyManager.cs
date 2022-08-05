using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を制御する
/// </summary>
public class EnemyManager : ActorBase
{

    void OnEnable()
    {
        FindObjectOfType<PlaySceneManager>().AddEnemy(this);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>ターンの最初に呼ばれる処理</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " ターンの初めに初期化します");
    }

    /// <summary>キー入力待ち中に呼ばれる処理</summary>
    public override void StandBy()
    {
        Debug.Log(gameObject.name + " キーの入力待ちです");
    }

    /// <summary>キャラクターが移動を開始するときに呼ばれる処理</summary>
    public override void MoveStart()
    {
        Debug.Log(gameObject.name + " 移動開始します");
    }

    /// <summary>キャラクターが移動中に呼ばれる処理</summary>
    public override void Move()
    {
        Debug.Log(gameObject.name + " 移動中です");
    }

    /// <summary>キャラクターが移動を終えたときに呼ばれる処理</summary>
    public override void MoveEnd()
    {
        Debug.Log(gameObject.name + " 移動を終えました");
    }

    /// <summary>キャラクターが行動を開始するときに呼ばれる処理</summary>
    public override void ActionStart()
    {
        Debug.Log(gameObject.name + " 行動を開始します");
    }

    /// <summary>キャラクターが行動中に呼ばれる処理</summary>
    public override void Action()
    {
        Debug.Log(gameObject.name + " 行動中です");
    }

    /// <summary>キャラクターが行動を終えるときに呼ばれる処理</summary>
    public override void ActionEnd()
    {
        Debug.Log(gameObject.name + " 行動を終えました");
    }
}
