using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// コインをDotWeenでアニメーションさせる
/// </summary>
public class CoinDOTWeenAnimation : MonoBehaviour
{
    IEnumerator Start()
    {
        float defaultPosY = transform.position.y;

        while (true)
        {
            transform.DORotate(new Vector3(0, 720, 0), 1.0f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutQuad);
            Sequence sequence = DOTween.Sequence();
            sequence.Join(transform.DOMoveY(defaultPosY + 0.5f, 0.5f).SetEase(Ease.OutQuad));
            sequence.Append(transform.DOMoveY(defaultPosY, 0.5f).SetEase(Ease.InQuad));
            sequence.Play();

            yield return new WaitForSeconds(2.0f);
        }
    }
}
