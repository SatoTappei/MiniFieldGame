using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を制御する
/// </summary>
public class EnemyManager : CharacterBase
{
    // 敵の行動タイプ
    enum ActionType
    {
        ActionAndMove,  // 攻撃と移動を行う
        MoveOnly,       // 移動しか行わない
    }

    /// <summary>プレイヤーを発見時に表示されるアイコン</summary>
    [SerializeField] GameObject _noticeIcon;
    /// <summary>視界として飛ばすRayの長さ</summary>
    [SerializeField] int _sightRange;
    /// <summary>敵の行動タイプ</summary>
    [SerializeField] ActionType _actionType;
    /// <summary>攻撃する相手がいる座標</summary>
    PosXZ _attackTargetPos;
    /// <summary>このターンに攻撃をする場合はtrue、移動の場合はfalseになる</summary>
    bool _doActionThisTurn;
    /// <summary>プレイヤーを発見しているか</summary>
    bool _isNotice;

    /// <summary>PlaySceneManagerがこのプロパティを見てメソッドを呼び出す</summary>
    public bool DoActionThisTurn { get => _doActionThisTurn; }

    void Awake()
    {
        _noticeIcon.SetActive(false);
    }

    void OnEnable()
    {
        FindObjectOfType<PlaySceneManager>().AddEnemy(this);
    }

    void Update()
    {

    }

    /// <summary>このターンの敵の行動を決定する</summary>
    public void RequestAI()
    {
        // プレイヤーが視界に入っているか
        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 origin = new Vector3(transform.position.x, 1, transform.position.z);
        if (Physics.Raycast(origin, dir, out RaycastHit hit, _sightRange))
        {
            Debug.DrawRay(origin, dir * _sightRange, Color.red, 5);
            // 飛ばしたRayがプレイヤーにヒットしたらプレイヤーを発見している
            _isNotice = hit.collider.gameObject.tag == "Player" ? true : false;
            _noticeIcon.SetActive(_isNotice);
        }

        // 移動しかしない敵なら必ず移動を行う
        if (_actionType == ActionType.MoveOnly)
        {
            FindObjectOfType<PlaySceneManager>().AddMoveActor();
            return;
        }

        // プレイヤーが自分の上下左右マスにいる場合は攻撃する
        // それ以外の場合は移動する
        MapManager mm = FindObjectOfType<MapManager>();
        for (int i = 0; i < 4; i++)
        {
            // ↓Directionの値に割り当てた数字を変えるとおかしくなるので注意
            ActorDir checkDir = (ActorDir)(i * 90);
            PosXZ pos = ActorUtility.GetTargetTile(_currentPosXZ, checkDir);
            CharacterBase cb = mm.CurrentMap.GetMapTileActor(pos.x, pos.z);
            _doActionThisTurn = cb != null && cb.GetCharacterType() == CharacterType.Player ? true : false;
            // 攻撃する場合は敵の方を向くように向きと攻撃するタイルを保持しておく
            if (_doActionThisTurn)
            {
                _inputDir = checkDir;
                _attackTargetPos = pos;
                break;
            }
        }

        if(_doActionThisTurn)
            FindObjectOfType<PlaySceneManager>().AddActionActor();
        else 
            FindObjectOfType<PlaySceneManager>().AddMoveActor();
    }

    /// <summary>ターンの最初に呼ばれる処理</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " ターンの初めに初期化します");
    }

    /// <summary>キー入力待ち中に呼ばれる処理</summary>
    public override void StandBy()
    {
        //Debug.Log(gameObject.name + " キーの入力待ちです");
    }

    /// <summary>キャラクターが移動を開始するときに呼ばれる処理</summary>
    public override void MoveStart()
    {
        Debug.Log(gameObject.name + " 移動開始します");
        // プレイヤーが移動する場合はStandByの時点で座標が決まっているため
        // 現状は移動を開始するときに移動する座標を決めている。
        // バグがある場合は、移動する座標を決める処理を行う場所を変える
        bool canMove = false;
        MapManager mm = FindObjectOfType<MapManager>();

        // プレイヤーを発見している場合はアルゴリズムを使用して動かす
        if (_isNotice)
        {
            PosXZ playerPos = FindObjectOfType<PlayerManager>().CurrentPosXZ;
            _inputDir = GetComponent<MoveAlgorithmBase>().GetMoveDirection(_currentPosXZ, playerPos);
            // 移動先の座標を取得
            _targetPosXZ = ActorUtility.GetTargetTile(_currentPosXZ, _inputDir);
            canMove = mm.CheckCanMoveTile(_targetPosXZ.x, _targetPosXZ.z);
        }
        // プレイヤーを発見していない場合は4方向いずれかにランダムに移動する
        else
        {
            List<(int, int)> dirs = new List<(int, int)>() { (1, 0), (0, 1), (-1, 0), (0, -1) };

            // どこにも移動できない場合はそのターンはその場にとどまる
            while (!canMove && dirs.Count > 0)
            {
                int r = Random.Range(0, dirs.Count);
                _inputDir = GetKeyToDir(dirs[r].Item1, dirs[r].Item2);
                // 移動しようとしているタイルが移動できるかどうかを調べる <= 変数dirsのxとzが入れ替わっているので注意
                canMove = mm.CheckCanMoveTile(_currentPosXZ.x + dirs[r].Item2, _currentPosXZ.z + dirs[r].Item1);
                dirs.RemoveAt(r);
            }
        }
        
        if (canMove)
        {
            // 移動先の座標を取得
            _targetPosXZ = ActorUtility.GetTargetTile(_currentPosXZ, _inputDir);
            // 現在のタイル上の座標から自身の情報を削除しておく
            mm.CurrentMap.SetMapTileCharacter(_currentPosXZ.x, _currentPosXZ.z, null);
            // 移動先の座標に自身の情報を登録しておく
            mm.CurrentMap.SetMapTileCharacter(_targetPosXZ.x, _targetPosXZ.z, this);
            // プレイヤーはその場で向きだけを変えることがあるので入力したときに向きを変えるが
            // 敵は移動する直前に向きを変える
            transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);
            StartCoroutine(Move(_targetPosXZ));
        }
        else
        {
            // その場に移動する = とどまる
            StartCoroutine(Move(_currentPosXZ));
        }
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

        // ターゲットの方向を向いて攻撃する
        PosXZ target = _attackTargetPos;
        transform.rotation = Quaternion.Euler(0, (float)_inputDir, 0);

        CharacterBase cb = FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor(target.x, target.z);
        // 攻撃するマスにプレイヤーがいればダメージの処理
        if (cb != null && cb.GetCharacterType() == CharacterType.Player)
        {
            cb.Damaged(_inputDir);
            SoundManager._instance.Play("SE_斬撃");
        }
        else
        {
            SoundManager._instance.Play("SE_ミス");
        }
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

    /// <summary>
    /// このキャラクターがダメージを受けたときに呼ばれる処理
    /// </summary>
    /// <param name="attackedDir">攻撃された方向</param>
    public override void Damaged(ActorDir attackedDir)
    {
        FindObjectOfType<ActionLogManager>().DispLog(_defeatedMessage);
        // 自身が死んだことをPlaySceneManagerに伝える
        PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
        psm.RemoveEnemy(this);
        psm.AddDeadCharacter(gameObject);
        // タイル上の情報を削除する
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileCharacter(_currentPosXZ.x, _currentPosXZ.z, null);
        // 死亡の演出を行う
        Death(attackedDir);
        _noticeIcon.SetActive(false);
        //// 死亡のアニメーションを再生(スケールを0にして見えなくする)
        //_anim.Play("Dead");
        //// 被ダメージのエフェクト、吹き出た血の表示と血だまりの生成
        //_damageEffect.SetActive(true);
        //Instantiate(_decalEffect, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.Euler(90, 0, 0));
        //// ラグドールを生成して攻撃された方向とは逆に吹っ飛ばす
        //var Obj = Instantiate(_ragDoll, transform.position, Quaternion.identity);
        //Vector3 vec = DirectionToVec3(attackedDir);
        //Obj.GetComponent<RagDollController>().Dir = new Vector3(vec.x, 0.5f, vec.z).normalized;
    }
}
