using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// "�}�E�X���N���b�N���Ă�"�̃A�C�R���̃A�j���[�V����
/// </summary>
public class ClickIconUI : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.GetChild(0).DOPunchPosition(Vector3.up * -5, 0.25f)).SetLoops(-1).SetDelay(1.5f);
    }

    void Update()
    {
        
    }
}
