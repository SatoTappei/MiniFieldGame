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
    /// <summary>ライフポイントを表すアイコンの親</summary>
    [SerializeField] Transform _lifePointItem;

    // DotWeenの挙動を確認するためだけの変数、確認が終わったら消す
    int _testLifePoint = 3;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _testLifePoint--;
            DecreaseLifePoint(_testLifePoint);
        }
    }

    /// <summary>経過したターンを表示する</summary>
    public void SetProgressTurn(int turn)
    {
        _dispTurnText.text = turn.ToString("000");
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_dispTurnText.transform.DOScale(_dispTurnText.transform.localScale * 1.2f, 0.15f));
        sequence.Append(_dispTurnText.transform.DOScale(_dispTurnText.transform.localScale, 0.15f));
        sequence.Play();
    }

    /// <summary>ライフを減少させる</summary>
    public void DecreaseLifePoint(int life)
    {
        Transform child = _lifePointItem.GetChild(life);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(child.DOMoveY(child.position.y - 100.0f, 1.0f).SetEase(Ease.OutBounce));
        sequence.Join(child.DOMoveX(child.position.x + 25.0f, 1.0f));
        sequence.Join(child.GetComponent<Image>().DOFade(0, 1.0f));
        sequence.Play();
    }
}
