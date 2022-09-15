using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// �^�C�g���̃{�^���𐧌䂷��
/// </summary>
public class TitleSceneMainButton : SelectableUIButton
{
    Image _img;
    /// <summary>�^�C�g���{�^�����N���b�N�����Ƃ��ɌĂ΂��C�x���g</summary>
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

    /// <summary>�{�^���̃A�C�h�����</summary>
    public void Idle()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_img.DOFade(0.5f, 2.0f).SetDelay(0.5f)).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>�{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂��</summary>
    public void PushButton()
    {
        SoundManager._instance.Play("SE_�^�C�g���{�^��");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_img.DOFade(0, 0.33f).SetEase(Ease.Flash, 5));
        sequence.AppendCallback(() => _clickEvent.Invoke());
    }
}
