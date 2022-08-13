using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー関係のUIへの表示を行う
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    /// <summary>経過ターンを表示するテキスト</summary>
    [SerializeField] Text _dispTurnText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>経過したターンを表示する</summary>
    public void SetProgressTurn(int turn)
    {
        _dispTurnText.text = turn.ToString("000");
        // TODO:DotWeenを使用したアニメーションを使いたい
    }
}
