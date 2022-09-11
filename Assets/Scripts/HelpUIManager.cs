using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �Q�[���v���C���̃w���v���Ǘ�����
/// </summary>
public class HelpUIManager : MonoBehaviour
{
    /// <summary>�\�������w���v�̔w�i</summary>
    [SerializeField] GameObject _background;
    /// <summary>�\�������w���v�̘g</summary>
    [SerializeField] Transform _frame;
    /// <summary>�w���v�̃{�^��</summary>
    [SerializeField] Button _helpButton;
    /// <summary>����{�^��</summary>
    [SerializeField] Button _closeButton;

    /// <summary>�w���v�̉񗗉\���ǂ�����؂�ւ�</summary>
    public void ActiveHelpUI(bool canOpen) => _helpButton.interactable = canOpen;

    void Start()
    {
        _background.SetActive(false);
        _frame.localScale = Vector3.zero;
    }

    void Update()
    {
        
    }

    /// <summary>�w���v���J��</summary>
    public void OpenPanel()
    {
        _background.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_frame.DOScale(new Vector3(1, 0.01f, 1), 0.25f));
        sequence.Append(_frame.DOScale(Vector3.one, 0.15f));
        sequence.Append(_frame.DOShakeScale(0.15f, 0.25f));
        sequence.AppendCallback(() => _closeButton.interactable = true);
    }

    /// <summary>�w���v�����</summary>
    public void ClosePanel()
    {
        _closeButton.interactable = false;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_frame.DOScale(new Vector3(1.25f, 1.25f, 1), 0.15f));
        sequence.Append(_frame.DOScale(Vector3.zero, 0.25f));
        sequence.AppendCallback(() => _background.SetActive(false));
    }
}
