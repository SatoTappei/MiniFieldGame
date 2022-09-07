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
    /// <summary>�X�e�[�W�ԍ���\������e�L�X�g</summary>
    [SerializeField] Text _stageText;
    /// <summary>�o�߃^�[����\������e�L�X�g</summary>
    [SerializeField] Text _dispTurnText;
    /// <summary>�X�R�A��\������e�L�X�g</summary>
    [SerializeField] Text _scoreText;
    /// <summary>���C�t�|�C���g��\���A�C�R���̐e</summary>
    [SerializeField] Transform _lifePointItem;

    void Start()
    {
        
    }

    void Update()
    {

    }

    /// <summary>�X�e�[�W�ԍ���\������</summary>
    public void SetStageNum(int num)
    {
        _stageText.text = num.ToString();
        PlayTextAnim(_stageText.transform);
    }

    /// <summary>�o�߂����^�[����\������</summary>
    public void SetProgressTurn(int turn)
    {
        _dispTurnText.text = turn.ToString("000");
        PlayTextAnim(_dispTurnText.transform);
    }

    /// <summary>�X�R�A��\������</summary>
    public void SetScore(int score)
    {
        _scoreText.text = score.ToString("000000");
        PlayTextAnim(_scoreText.transform);
    }

    /// <summary>�e�L�X�g���|�b�v������A�j���[�V�������s��</summary>
    void PlayTextAnim(Transform trans)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(trans.DOScale(trans.localScale * 1.2f, 0.15f));
        sequence.Append(trans.DOScale(trans.localScale, 0.15f));
    }

    /// <summary>���C�t������������</summary>
    public void DecreaseLifePoint(int life)
    {
        // ���C�t��0�ȉ��̏ꍇ�͏������Ȃ�
        if (life < 0) return;

        Transform child = _lifePointItem.GetChild(life);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(child.DOMoveY(child.position.y - 100.0f, 1.0f).SetEase(Ease.OutBounce));
        sequence.Join(child.DOMoveX(child.position.x + 25.0f, 1.0f));
        sequence.Join(child.GetComponent<Image>().DOFade(0, 1.0f));
    }
}
