using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 瀕死になった際の演出
/// </summary>
public class DyingEffect : MonoBehaviour
{
    void Awake()
    {
        Image _img = GetComponent<Image>();
        DOTween.Sequence().Append(_img.DOFade(0.5f, 2.0f).SetDelay(0.5f)).SetLoops(-1, LoopType.Yoyo);
        SoundManager._instance.Play("SE_瀕死");
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
