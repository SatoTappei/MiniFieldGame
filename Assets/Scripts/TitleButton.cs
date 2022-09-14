using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// タイトルのボタンを制御する
/// </summary>
public class TitleButton : MonoBehaviour
{
    CanvasGroup _cg;
    /// <summary>タイトルボタンをクリックしたときに呼ばれるイベント</summary>
    [SerializeField] UnityEvent _clickEvent;

    void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Idle();
    }

    void Update()
    {
        
    }

    /// <summary>ボタンのアイドル状態</summary>
    public void Idle()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_cg.DOFade(0.5f, 2.0f).SetDelay(0.5f)).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>ボタンがクリックされたときに呼ばれる</summary>
    public void PushButton()
    {
        SoundManager._instance.Play("SE_タイトルボタン");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_cg.DOFade(0, 0.33f).SetEase(Ease.Flash, 5));
        sequence.AppendCallback(() => _clickEvent.Invoke());
    }
}
