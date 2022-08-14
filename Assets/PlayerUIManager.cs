using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_dispTurnText.transform.DOScale(_dispTurnText.transform.localScale * 1.2f, 0.15f));
        sequence.Append(_dispTurnText.transform.DOScale(_dispTurnText.transform.localScale, 0.15f));
        sequence.Play();
        // TODO:DotWeenを使用したアニメーションを使いたい
    }
}
