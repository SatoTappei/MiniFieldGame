using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの行動を制御する
/// </summary>
public class PlayerController : ActorBase
{
    /// <summary>XZ平面上での座標</summary>
    public struct PosXZ
    {
        public int x;
        public int z;
    }

    /// <summary>現在のXZ平面上での位置</summary>
    PosXZ _currentPosXZ;

    void OnEnable()
    {
        FindObjectOfType<PlaySceneManager>()._playerAction += TurnStart; 
    }

    void Start()
    {

    }

    void Update()
    {
        // TODO:現状1回しか移動できない、移動するとカメラが回転してしまう

        ActorDir inpugDir = GetKeyToDir();
        // 行動中ではない時に、上下左右の入力があった場合
        if (!_inAction && inpugDir != ActorDir.Neutral)
        {
            // 行動中にする
            _inAction = true;
            // 移動先の座標を取得
            PosXZ tartgetPosXZ = GetTargetTile(_currentPosXZ, inpugDir);
            // 目標の座標に向け移動させる
            StartCoroutine(Move(_currentPosXZ, tartgetPosXZ));
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

    /// <summary>プレイヤーのターンが始まったら呼ばれる処理</summary>
    void TurnStart()
    {
        Debug.Log("プレイヤーターン開始");
        StartCoroutine(Action());
    }

    /// <summary>プレイヤーを行動させる</summary>
    IEnumerator Action()
    {
        // TODO:行動の処理を書く、"移動"もしくは"攻撃"をしたらプレイヤーの行動は終了
        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Space)); // テスト Spaceキーを押すまで待つ
        PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
        psm.EndPlayerTurn = true;
    }

    /// <summary>指定した座標に補完しつつ移動させる</summary>
    IEnumerator Move(PosXZ current, PosXZ target)
    {
        Vector3 currentPos = new Vector3(current.x, 0, current.z);
        Vector3 targetPos = new Vector3(target.x, 0, target.z);

        int count = 0;
        while (transform.position != targetPos)
        {
            float value = count / MoveTileTime;
            transform.position = Vector3.Lerp(currentPos, targetPos, value);
            yield return null;
            count++;
        }
    }

    /// <summary>入力に対応したキャラクターの向きを返す</summary>
    ActorDir GetKeyToDir()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        if (vert > 0) return ActorDir.Up;
        else if (vert < 0) return ActorDir.Down;
        else if (hori > 0) return ActorDir.Right;
        else if (hori < 0) return ActorDir.Left;
        
        return ActorDir.Neutral;
    }

    /// <summary>現在の座標と方向から移動先の座標を取得</summary>
    PosXZ GetTargetTile(PosXZ current,ActorDir dir)
    {
        PosXZ target = current;

        if (dir == ActorDir.Up) target.z++;
        else if (dir == ActorDir.Down) target.z--;
        else if (dir == ActorDir.Right) target.x++;
        else if (dir == ActorDir.Left) target.x--;

        return target;
    }
}
