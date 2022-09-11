using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    /// <summary>����{�^��</summary>
    [SerializeField] GameObject _closeButton;
    /// <summary>���݃w���v���J���Ă��邩</summary>
    bool _isHelpping;

    /// <summary>�w���v���J���Ă��邩�`�F�b�N</summary>
    public bool CheckOpenHelp() => _isHelpping;


    void Awake()
    {
        _background.SetActive(false);
        _frame.localScale = Vector3.zero;
        // �O������enabled��true�ɂ����܂Ńw���v���J�����Ȃ�
        enabled = false;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isHelpping)
        {
            OpenPanel();
        }
    }

    /// <summary>�w���v���J��</summary>
    public void OpenPanel()
    {
        EventSystem.current.SetSelectedGameObject(_closeButton);
        _isHelpping = true;
        _background.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_frame.DOScale(new Vector3(1, 0.01f, 1), 0.25f));
        sequence.Append(_frame.DOScale(Vector3.one, 0.15f));
        sequence.Append(_frame.DOShakeScale(0.15f, 0.25f));
        sequence.AppendCallback(() => _closeButton.GetComponent<Button>().interactable = true);
    }

    /// <summary>�w���v�����</summary>
    public void ClosePanel()
    {
        _closeButton.GetComponent<Button>().interactable = false;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_frame.DOScale(new Vector3(1.25f, 1.25f, 1), 0.15f));
        sequence.Append(_frame.DOScale(Vector3.zero, 0.25f));
        sequence.AppendCallback(() =>
        {
            _background.SetActive(false);
            _isHelpping = false;
        });
    }

    /// <summary>
    /// �w���v���A�j���[�V���������Ȃ��ŕ���
    /// ���̃V�[���ł͓�x�ƊJ���Ȃ��悤�ɂ���ꍇ�Ɏg��
    /// </summary>
    public void ForcedClosePanel()
    {
        _background.SetActive(false);
        _frame.gameObject.SetActive(false);
        enabled = false;
    }
}
