using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// ïméÄÇ…Ç»Ç¡ÇΩç€ÇÃââèo
/// </summary>
public class DyingEffect : MonoBehaviour
{
    Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
    }

    void Start()
    {
        DOTween.Sequence().Append(_img.DOFade(0.5f, 2.0f).SetDelay(0.5f)).SetLoops(-1, LoopType.Yoyo);
        SoundManager._instance.Play("SE_ïméÄ");
    }

    void Update()
    {
        
    }
}
