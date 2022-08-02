using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲームの進行を管理・制御する
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    /// <summary>プレイヤーのターンになったら毎ターン勝手に呼ばれる</summary>
    public event UnityAction _playerAction;
    /// <summary>敵のターンになったら毎ターン勝手に呼ばれる</summary>
    public event UnityAction _enemyAction;
    /// <summary>プレイヤーの行動が終わったらtrueになる/summary>
    bool _endPlayerTurn;
    /// <summary>敵全員の行動が終わったらtrueになる</summary>
    bool _endEnemyTurn;

    public bool EndPlayerTurn { set => _endPlayerTurn = value; }
    public bool EndEnemyTurn { set => _endEnemyTurn = value; }

    void Start()
    {
        StartCoroutine(TurnSystem());
    }

    void Update()
    {
        
    }

    /// <summary>ローグライクのようなターンシステムを行う</summary>
    IEnumerator TurnSystem()
    {
        while (true)
        {
            _playerAction.Invoke();
            yield return new WaitUntil(() => _endPlayerTurn);
            _enemyAction.Invoke();
            yield return new WaitUntil(() => _endEnemyTurn);
        }
    }
}
