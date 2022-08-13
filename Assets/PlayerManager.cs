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

    /// <summary>ターンの最初に呼ばれる処理</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " ターンの初めに初期化します");
    }

    /// <summary>キー入力待ち中に毎フレーム呼ばれる処理</summary>
    public override void StandBy()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        // 行動
        if (Input.GetButtonDown("Submit"))
        {
            // 行動するキャラクターだということをPlaySceneManagerに伝えて次のStateに移行する
            PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
            psm.AddActionActor();
            FindObjectOfType<PlaySceneManager>().SetTurnState(TurnState.PlayerActionStart);
        }
        // 移動 縦と横が同時に押されていたら無視(バグが出そう)
        else if (Mathf.Abs(vert + hori) == 1)
        {
            // 移動しようとしているタイルが移動できるかどうかを調べる
            bool canMove = FindObjectOfType<MapManager>().CheckCanMoveTile((int)(_currentPosXZ.x + hori), (int)(_currentPosXZ.z + vert));
            _inputDir = GetKeyToDir(vert, hori);
            // 移動先の座標を取得
            _tartgetPosXZ = GetTargetTile(_inputDir);

            // 移動が可能なら
            if (canMove)
            {
                // 移動するキャラクターだということをPlaySceneManagerに伝えて次のStateに移行する
                PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
                psm.AddMoveActor();
                psm.SetTurnState(TurnState.PlayerMoveStart);
            }
        }
    }

    /// <summary>キャラクターが移動を開始するときに呼ばれる処理</summary>
    public override void MoveStart()
    {
        Debug.Log(gameObject.name + " 移動開始します");
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
        _anim.Play("Slash");
        //FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor();
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

    /// <summary>このキャラクターがダメージを受けたときに呼ばれる処理</summary>
    public override void Damaged()
    {
        _anim.Play("Fall");
    }
}
