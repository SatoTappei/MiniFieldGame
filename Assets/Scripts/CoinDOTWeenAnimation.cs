using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// コインをDotWeenでアニメーションさせる
/// </summary>
public class CoinDOTWeenAnimation : MonoBehaviour
{
    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DORotate(new Vector3(0, 720, 0), 1.5f, RotateMode.FastBeyond360));
        sequence.Join(transform.DOJump(transform.position, 0.5f, 1, 1.5f));
        sequence.SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Restart)
            .SetDelay(1)
            .SetLink(gameObject);
    }
}
