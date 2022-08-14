using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �v���C���[�֌W��UI�ւ̕\�����s��
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    /// <summary>�o�߃^�[����\������e�L�X�g</summary>
    [SerializeField] Text _dispTurnText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�o�߂����^�[����\������</summary>
    public void SetProgressTurn(int turn)
    {
        _dispTurnText.text = turn.ToString("000");
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_dispTurnText.transform.DOScale(_dispTurnText.transform.localScale * 1.2f, 0.15f));
        sequence.Append(_dispTurnText.transform.DOScale(_dispTurnText.transform.localScale, 0.15f));
        sequence.Play();
        // TODO:DotWeen���g�p�����A�j���[�V�������g������
    }
}
