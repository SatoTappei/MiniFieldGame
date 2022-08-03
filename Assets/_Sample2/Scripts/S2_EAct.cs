using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAct
{
    KeyInput, // キーの入力待ち

    // アクション
    ActBegin,   // 開始
    Act,        // 実行中
    ActEnd,     // 終了
                // 移動
    MoveBegin,  // 開始
    Move,       // 移動中
    MoveEnd,    // 完了

    TurnEnd,    // ターン終了
}

//public class S2_EAct : MonoBehaviour
//{
   
//}
