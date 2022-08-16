using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を制御する
/// </summary>
public class EnemyManager : CharacterBase
{
    /// <summary>このターンに攻撃をする場合はtrue、移動の場合はfalseになる</summary>
    bool _doActionThisTurn;

    /// <summary>PlaySceneManagerがこのぷろぱちぃを見てメソッドを呼び出す</summary>
    public bool DoActionThisTurn { get => _doActionThisTurn; }

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
        // TODO:敵のAIを作る(現在はランダムで行動を決定する)
        int r = Random.Range(1, 3);
        _doActionThisTurn = r == 1;

        // 移動と行動のどっちをするのかをPlaySceneManagerに教える
        if (_doActionThisTurn)
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
        // 追記:現状ランダムで移動と攻撃を決めているので移動になった際に通路の行き止まりにいる＆後ろにプレイヤーがいる場合どうしようもない
        // 対処:周り8マスにプレイヤーがいる場合は攻撃するようにするのでこのターンはその場にとどまる

        MapManager mm = FindObjectOfType<MapManager>();

        // TODO:現状はランダムで4方向に移動するのでアルゴリズムを使った方法に直す
        List<(int, int)> dirs = new List<(int, int)>();
        dirs.Add((1, 0));
        dirs.Add((0, 1));
        dirs.Add((-1, 0));
        dirs.Add((0, -1));

        bool canMove = false;
        // どこにも移動できない場合はそのターンはその場にとどまる
        while (!canMove && dirs.Count > 0)
        {
            int r = Random.Range(0, dirs.Count);
            _inputDir = GetKeyToDir(dirs[r].Item1, dirs[r].Item2);
            // 移動しようとしているタイルが移動できるかどうかを調べる <= 変数dirsのxとzが入れ替わっているので注意
            canMove = mm.CheckCanMoveTile(_currentPosXZ.x + dirs[r].Item2, _currentPosXZ.z + dirs[r].Item1);
            dirs.RemoveAt(r);
        }
        // TODO:ランダムで決定ここまで
        
        if (canMove)
        {
            // 移動先の座標を取得
            _targetPosXZ = GetTargetTile(_inputDir);
            // 現在のタイル上の座標から自身の情報を削除しておく
            mm.CurrentMap.SetMapTileActor(_currentPosXZ.x, _currentPosXZ.z, null);
            // 移動先の座標に自身の情報を登録しておく
            mm.CurrentMap.SetMapTileActor(_targetPosXZ.x, _targetPosXZ.z, this);
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

        // 現在は攻撃する際にプレイヤーがいるかどうか判定しているので空振りが起こる
        // TODO:空振りさせないために"周囲八マスにプレイヤーがいたらそっちを向いて攻撃する"ようにする

        // 攻撃するマスの情報を取得
        PosXZ target = GetTargetTile(_inputDir);
        CharacterBase ab = FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor(target.x, target.z);
        // 攻撃するマスにプレイヤーがいればダメージの処理
        if (ab != null && ab.GetActorType() == ActorType.Player)
            ab.Damaged(_inputDir);

        // もし正面に敵がいたらダメージ、後々に攻撃範囲は広げるかもしれないので留意しておく
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

    /// <summary>
    /// このキャラクターがダメージを受けたときに呼ばれる処理
    /// </summary>
    /// <param name="attackedDir">攻撃された方向</param>
    public override void Damaged(Direction attackedDir)
    {
        // 敵は全員1撃で死ぬので死亡のアニメーションを再生する
        FindObjectOfType<PlaySceneManager>().RemoveEnemy(this);
        // タイル上の情報を削除する
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileActor(_currentPosXZ.x, _currentPosXZ.z, null);
        // 死亡のアニメーションを再生(スケールを0にして見えなくする)
        _anim.Play("Dead");
        // 被ダメージのエフェクトを生成する
        Instantiate(_damageEffect, new Vector3(transform.position.x, 0.9f, transform.position.z), Quaternion.identity);
        Instantiate(_decalEffect, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.Euler(90, 0, 0));
        // ラグドールを生成して攻撃された方向とは逆に吹っ飛ばす
        var Obj = Instantiate(_ragDoll, transform.position, Quaternion.identity);
        Vector3 vec = DirectionToVec3(attackedDir);
        Obj.GetComponent<RagDollController>().Dir = new Vector3(vec.x, 0.5f, vec.z).normalized;
    }
}
