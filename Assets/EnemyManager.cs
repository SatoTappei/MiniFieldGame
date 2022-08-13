using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を制御する
/// </summary>
public class EnemyManager : ActorBase
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

        // 移動先の座標を取得
        _tartgetPosXZ = GetTargetTile(_inputDir);
        // 現在のタイル上の座標から自身の情報を削除しておく
        mm.CurrentMap.SetMapTileActor(_currentPosXZ.x, _currentPosXZ.z, null);
        // 移動先の座標に自身の情報を登録しておく
        mm.CurrentMap.SetMapTileActor(_tartgetPosXZ.x, _tartgetPosXZ.z, this);
        // プレイヤーはその場で向きだけを変えることがあるので入力したときに向きを変えるが
        // 敵は移動する直前に向きを変える
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
        // もし正面に敵がいたらダメージ、後々に攻撃範囲は広げるかもしれないので留意しておく

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
