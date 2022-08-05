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
        // TODO:ボタン入力を連続すると移動できなくなるので直す…というかターンの概念を作る

        Direction inpugDir = GetKeyToDir();
        // 行動中ではない時に、上下左右の入力があった場合
        if (!_inAction && inpugDir != Direction.Neutral)
        {
            // 行動中にする
            _inAction = true;
            // 移動先の座標を取得
            PosXZ tartgetPosXZ = GetTargetTile(inpugDir);
            // 目標の座標に向け移動させる
            transform.rotation = Quaternion.Euler(0, (float)inpugDir, 0);
            StartCoroutine(Move(tartgetPosXZ));
        }
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

    ///// <summary>プレイヤーのターンが始まったら呼ばれる処理</summary>
    //void TurnStart()
    //{
    //    Debug.Log("プレイヤーターン開始");
    //    StartCoroutine(Action());
    //}

    ///// <summary>プレイヤーを行動させる</summary>
    //IEnumerator Action()
    //{
    //    // TODO:行動の処理を書く、"移動"もしくは"攻撃"をしたらプレイヤーの行動は終了
    //    yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Space)); // テスト Spaceキーを押すまで待つ
    //    PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
    //   // psm.EndPlayerTurn = true;
    //}

    /// <summary>指定した座標に補完しつつ移動させる</summary>
    IEnumerator Move(PosXZ target)
    {
        Vector3 currentPos = new Vector3(_currentPosXZ.x, 0, _currentPosXZ.z);
        Vector3 targetPos = new Vector3(target.x, 0, target.z);

        int count = 0;
        while (transform.position != targetPos)
        {
            float value = count / MoveTileTime;
            transform.position = Vector3.Lerp(currentPos, targetPos, value);
            yield return null;
            count++;
        }

        // 移動が完了したら現在のタイル上の位置を移動先の座標に変更する
        _currentPosXZ = target;
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

    /// <summary>現在の座標と方向から移動先の座標を取得</summary>
    PosXZ GetTargetTile(Direction dir)
    {
        PosXZ target = _currentPosXZ;

        if (dir == Direction.Up) target.z++;
        else if (dir == Direction.Down) target.z--;
        else if (dir == Direction.Right) target.x++;
        else if (dir == Direction.Left) target.x--;

        return target;
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

        // TODO:直す、移動のキーと攻撃のキーに直す
        if (Input.GetKeyDown(KeyCode.M))
        {
            FindObjectOfType<PlaySceneManager>().PushKey = KeyCode.M;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            FindObjectOfType<PlaySceneManager>().PushKey = KeyCode.A;
        }
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
