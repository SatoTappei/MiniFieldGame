using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// コインをDotWeenでアニメーションさせる
/// </summary>
public class CoinDOTWeenAnimation : MonoBehaviour
{
    //IEnumerator Start()
    //{
    //    float defaultPosY = transform.position.y;

    //    while (true)
    //    {
    //        transform.DORotate(new Vector3(0, 720, 0), 1.0f, RotateMode.FastBeyond360)
    //            .SetEase(Ease.InOutQuad).SetLink(gameObject);
    //        Sequence sequence = DOTween.Sequence();
    //        sequence.Join(transform.DOMoveY(defaultPosY + 0.5f, 0.5f).SetEase(Ease.OutQuad));
    //        sequence.Append(transform.DOMoveY(defaultPosY, 0.5f).SetEase(Ease.InQuad));
    //        sequence.Play().SetLink(gameObject);

    //        yield return new WaitForSeconds(2.0f);
    //    }
    //}

    void Start()
    {
        //Vector3 defaultPos = transform.position;

        //transform.DORotate(new Vector3(0, 720, 0), 1.0f, RotateMode.FastBeyond360)
        //    .SetEase(Ease.InOutQuad)
        //    .SetLoops(-1, LoopType.Restart)
        //    .SetDelay(1)
        //    .SetLink(gameObject);
        //transform.DOJump(transform.position, 1, 1, 1)
        //    .SetEase(Ease.InOutQuad)
        //    .SetLoops(-1, LoopType.Restart)
        //    .SetDelay(1)
        //    .SetLink(gameObject);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DORotate(new Vector3(0, 720, 0), 1.5f, RotateMode.FastBeyond360));
        sequence.Join(transform.DOJump(transform.position, 0.5f, 1, 1.5f));
        sequence.SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Restart)
            .SetDelay(1)
            .SetLink(gameObject);
    }
}
