using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを制御する
/// </summary>
public class PlayerManager : CharacterBase
{
    /// <summary>ダメージを受けた際に表示されるメッセージ</summary>
    [SerializeField] string _damagedMessage;
    /// <summary>体力が1になったときに表示される演出</summary>
    //[SerializeField] GameObject _dyingEffect;
    /// <summary>
    /// プレイヤーの体力、
    /// 3以上にするにはUIのアイコンも増やさないといけないのでインスペクターには表示させない
    /// </summary>
    int _lifePoint = 3;

    /// <summary>アイテムを獲得して2マス先に攻撃できる状態か</summary>
    bool _powerUp;

    /// <summary>パワーアップ状態になる</summary>
    public void SetPowerUp() => _powerUp = true;

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
            _targetPosXZ = ActorUtility.GetTargetTile(_currentPosXZ, _inputDir);
            // 移動の可能不可能に限らず入力された方向にキャラクターの向きだけを変える
            transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);

            // 移動が可能なら
            if (canMove)
            {
                // この時点で移動する先の座標は決まっているので、予約しておかないと
                // 敵のAIのターンでプレイヤーが移動する先の座標まで選択肢に入ってしまう

                // 現在のタイル上の座標から自身の情報を削除しておく
                mm.CurrentMap.SetMapTileCharacter(_currentPosXZ.x, _currentPosXZ.z, null);
                // 移動先の座標に自身の情報を登録しておく
                mm.CurrentMap.SetMapTileCharacter(_targetPosXZ.x, _targetPosXZ.z, this);
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
        StartCoroutine(Move(_targetPosXZ));
        // 移動先の座標にアイテムがあるか確認して、ある場合は獲得時の処理を実行
        ItemManager im = FindObjectOfType<MapManager>().CurrentMap.GetMapTileItem(_targetPosXZ.x, _targetPosXZ.z);
        im?.GetThisItem();
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
        // プレイヤーがゴールの上に立っていたらフラグを立てる
        if (FindObjectOfType<MapManager>().CurrentMap.GetMapTile(_currentPosXZ.x, _currentPosXZ.z).Char == 'E')
            FindObjectOfType<PlaySceneManager>().StandOnGoalTile();
    }

    /// <summary>キャラクターが行動を開始するときに呼ばれる処理</summary>
    public override void ActionStart()
    {
        _anim.Play("Slash");
        MapManager mm = FindObjectOfType<MapManager>();
        // 攻撃するマスの情報を取得
        PosXZ target = ActorUtility.GetTargetTile(_currentPosXZ, _inputDir);
        CharacterBase cb = mm.CurrentMap.GetMapTileActor(target.x, target.z);
        // 攻撃するマスに敵がいればダメージの処理
        cb?.Damaged(_inputDir);
        // パワーアップ状態ならもう1マス先の敵にも攻撃する
        CharacterBase bcb = null;
        if (_powerUp)
        {
            PosXZ backTarget = ActorUtility.GetTargetTile(target, _inputDir);
            bcb = mm.CurrentMap.GetMapTileActor(backTarget.x, backTarget.z);
            bcb?.Damaged(_inputDir);
        }
        SoundManager._instance.Play(cb || bcb ? "SE_斬撃" : "SE_ミス");
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

    /// <summary>ターン終了時に呼ばれる処理</summary>
    public override void TurnEnd()
    {

    }

    /// <summary>このキャラクターがダメージを受けたときに呼ばれる処理</summary>
    public override void Damaged(ActorDir attackedDir)
    {
        // 攻撃された際に既に体力が0なら何もしない
        if (_lifePoint <= 0)
        {
            FindObjectOfType<PlaySceneManager>().SendEndAction();
            return;
        }

        _anim.Play("Damage");
        // 被ダメージのエフェクトを生成する、高さだけキャラクターの胸の位置に設定する
        if (_damageEffect != null)
            Instantiate(_damageEffect, new Vector3(transform.position.x, 0.9f, transform.position.z), Quaternion.identity);
        // 体力を減らしてUIに反映させる、HPが0より小さくなってしまうが、呼び出したメソッド側で弾くので今のところは問題ない
        _lifePoint--;
        FindObjectOfType<PlayerUIManager>().DecreaseLifePoint(_lifePoint);

        // 体力が1になったら瀕死の演出を行う
        //if (_lifePoint == 1)
        //{
        //    Instantiate(_dyingEffect, Vector3.zero, Quaternion.identity);
        //}
        // 体力が0になったら
        if (_lifePoint == 0) 
        {
            FindObjectOfType<ActionLogManager>().DispLog(_defeatedMessage);
            // プレイヤーが死んだことを通知する
            PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
            psm.PlayerIsDead();
            psm.AddDeadCharacter(gameObject);
            // 死亡の演出を行う
            Death(attackedDir);
            //// 死亡のアニメーションを再生
            //_anim.Play("Dead");
            //// ラグドールを生成して攻撃された方向とは逆に吹っ飛ばす
            //var obj = Instantiate(_ragDoll, transform.position, Quaternion.identity);
            //Vector3 vec = DirectionToVec3(attackedDir);
            //obj.GetComponent<RagDollController>().Dir = new Vector3(vec.x, 0.5f, vec.z).normalized;
        }
        else
        {
            FindObjectOfType<ActionLogManager>().DispLog(_damagedMessage);
        }
    }
}
