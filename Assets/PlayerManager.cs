using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを制御する
/// </summary>
public class PlayerManager : ActorBase
{
    /// <summary>移動する際の移動先の座標</summary>
    PosXZ _tartgetPosXZ;
    /// <summary>入力された方向、キャラクターの移動に使用する</summary>
    Direction _inputDir;

    void OnEnable()
    {
        // PlaySceneManagerのStateで制御するために自身を登録しておく
        FindObjectOfType<PlaySceneManager>().SetPlayer(this);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// MapGeneratorでマップ生成時、プレイヤーの配置場所が決まったら
    /// ワールド座標をタイル上の座標に変換してセットする
    /// </summary>
    public void InitPosXZ()
    {
        _currentPosXZ.x = (int)transform.position.x;
        _currentPosXZ.z = (int)transform.position.z;
    }

    /// <summary>入力に対応したキャラクターの向きを返す</summary>
    Direction GetKeyToDir()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        if (vert > 0) return Direction.Up;
        else if (vert < 0) return Direction.Down;
        else if (hori > 0) return Direction.Right;
        else if (hori < 0) return Direction.Left;
        
        return Direction.Neutral;
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

        // いずれかのキーが押されたら
        if (Input.anyKeyDown)
        {
            Direction inputDir = GetKeyToDir();
            // 移動先の座標を取得
            _tartgetPosXZ = GetTargetTile(inputDir);

            PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
            // 移動キーならプレイヤーが移動する処理の流れを行う
            if (inputDir != Direction.Neutral)
                psm.SetTurnState(TurnState.PlayerMoveStart);
            // 攻撃キーならプレイヤーが攻撃する処理の流れを行う
            else if (Input.GetButtonDown("Fire1"))
                psm.SetTurnState(TurnState.PlayerActionStart);
        }
    }

    /// <summary>キャラクターが移動を開始するときに呼ばれる処理</summary>
    public override void MoveStart()
    {
        // 目標の座標に向け移動させる
        transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);
        StartCoroutine(Move(_tartgetPosXZ));
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
