using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// タイトルのボタンを制御する
/// </summary>
public class TitleSceneMainButton : SelectableUIButton
{
    Image _img;
    /// <summary>タイトルボタンをクリックしたときに呼ばれるイベント</summary>
    [SerializeField] UnityEvent _clickEvent;

    protected override void Awake()
    {
        base.Awake();
        _img = GetComponent<Image>();
    }

    void Start()
    {
        //Idle();
    }

    void Update()
    {
        
    }

    /// <summary>ボタンのアイドル状態</summary>
    public void Idle()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_img.DOFade(0.5f, 2.0f).SetDelay(0.5f)).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>ボタンがクリックされたときに呼ばれる</summary>
    public void PushButton()
    {
        SoundManager._instance.Play("SE_タイトルボタン");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_img.DOFade(0, 0.33f).SetEase(Ease.Flash, 5));
        sequence.AppendCallback(() => _clickEvent.Invoke());
    }
}
