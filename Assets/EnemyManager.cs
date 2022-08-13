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

        bool canMove = false;
        while (!canMove)
        {
            // TODO:現状はランダムで4方向に移動する
            (int, int)[] dirs = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            int r = Random.Range(0, 4);
            _inputDir = GetKeyToDir(dirs[r].Item1, dirs[r].Item2);
            // 移動しようとしているタイルが移動できるかどうかを調べる <= xとzが入れ替わっているので注意
            canMove = FindObjectOfType<MapManager>().CheckCanMoveTile(_currentPosXZ.x + dirs[r].Item2, _currentPosXZ.z + dirs[r].Item1);
        }

        // 移動先の座標を取得
        _tartgetPosXZ = GetTargetTile(_inputDir);

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
