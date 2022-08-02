using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの行動を制御する
/// </summary>
public class PlayerController : MonoBehaviour
{

    void OnEnable()
    {
        FindObjectOfType<PlaySceneManager>()._playerAction += TurnStart; 
    }

    void Start()
    {
        
    }

    void Update()
    {

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
}
