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
            MapManager mm = FindObjectOfType<MapManager>();
            // 移動しようとしているタイルが移動できるかどうかを調べる
            bool canMove = mm.CheckCanMoveTile((int)(_currentPosXZ.x + hori), (int)(_currentPosXZ.z + vert));
            _inputDir = GetKeyToDir(vert, hori);
            // 移動先の座標を取得
            _tartgetPosXZ = GetTargetTile(_inputDir);
            // 移動の可能不可能に限らず入力された方向にキャラクターの向きだけを変える
            transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);

            // 移動が可能なら
            if (canMove)
            {
                // この時点で移動する先の座標は決まっているので、予約しておかないと
                // 敵のAIのターンでプレイヤーが移動する先の座標まで選択肢に入ってしまう

                // 現在のタイル上の座標から自身の情報を削除しておく
                mm.CurrentMap.SetMapTileActor(_currentPosXZ.x, _currentPosXZ.z, null);
                // 移動先の座標に自身の情報を登録しておく
                mm.CurrentMap.SetMapTileActor(_tartgetPosXZ.x, _tartgetPosXZ.z, this);
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
        // 攻撃するマスの情報を取得
        PosXZ target = GetTargetTile(_inputDir);
        ActorBase ab = FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor(target.x, target.z);
        // 攻撃するマスに敵がいればダメージの処理
        ab?.Damaged();
        
        // キャラクターの向きを保持しておく
        // キャラクターの前のマスの情報を取得
        // 前のマスがnullなら行動終了
        // 攻撃アニメーション再生
        // 敵を消す
        // 行動終了
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
