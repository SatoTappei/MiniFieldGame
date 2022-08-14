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
    /// <summary>���C�t�|�C���g��\���A�C�R���̐e</summary>
    [SerializeField] Transform _lifePointItem;

    // DotWeen�̋������m�F���邽�߂����̕ϐ��A�m�F���I����������
    int _testLifePoint = 3;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _testLifePoint--;
            DecreaseLifePoint(_testLifePoint);
        }
    }

    /// <summary>�o�߂����^�[����\������</summary>
    public void SetProgressTurn(int turn)
    {
        _dispTurnText.text = turn.ToString("000");
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_dispTurnText.transform.DOScale(_dispTurnText.transform.localScale * 1.2f, 0.15f));
        sequence.Append(_dispTurnText.transform.DOScale(_dispTurnText.transform.localScale, 0.15f));
        sequence.Play();
    }

    /// <summary>���C�t������������</summary>
    public void DecreaseLifePoint(int life)
    {
        Transform child = _lifePointItem.GetChild(life);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(child.DOMoveY(child.position.y - 100.0f, 1.0f).SetEase(Ease.OutBounce));
        sequence.Join(child.DOMoveX(child.position.x + 25.0f, 1.0f));
        sequence.Join(child.GetComponent<Image>().DOFade(0, 1.0f));
        sequence.Play();
    }
}
