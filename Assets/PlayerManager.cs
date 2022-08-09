using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを制御する
/// </summary>
public class PlayerManager : ActorBase
{
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

    /// <summary>ターンの最初に呼ばれる処理</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " ターンの初めに初期化します");
    }

    /// <summary>キー入力待ち中に毎フレーム呼ばれる処理</summary>
    public override void StandBy()
    {
        Debug.Log(gameObject.name + " キーの入力待ちです");

        // いずれかのキーが押されたら
        if (Input.anyKeyDown)
        {
            PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();

            // 攻撃キーなら攻撃の処理をする
            if (Input.GetButtonDown("Submit"))
            {
                psm.SetTurnState(TurnState.PlayerActionStart);
            }
            // 移動キーなら移動の処理をする
            else if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal"))
            {
                float vert = Input.GetAxisRaw("Vertical");
                float hori = Input.GetAxisRaw("Horizontal");
                // 移動しようとしているタイルが移動できるかどうかを調べる
                bool canMove = FindObjectOfType<MapManager>().CheckCanMoveTile((int)(_currentPosXZ.x + hori), (int)(_currentPosXZ.z + vert));
                Debug.Log("移動可能か " + canMove);
                _inputDir = GetKeyToDir(vert, hori);
                // 移動先の座標を取得
                _tartgetPosXZ = GetTargetTile(_inputDir);

                // 移動が可能なら次のStateに移行する
                if (canMove)
                    psm.SetTurnState(TurnState.PlayerMoveStart);
            }
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
        // テスト:攻撃用のエフェクトを生成する、後々にキャラクターのアニメーションに切り替える
        Instantiate(_attackEffect, transform.position, Quaternion.identity);
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
