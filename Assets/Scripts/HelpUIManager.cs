using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// ゲームプレイ中のヘルプを管理する
/// </summary>
public class HelpUIManager : MonoBehaviour
{
    /// <summary>表示されるヘルプの背景</summary>
    [SerializeField] GameObject _background;
    /// <summary>表示されるヘルプの枠</summary>
    [SerializeField] Transform _frame;
    /// <summary>閉じるボタン</summary>
    [SerializeField] GameObject _closeButton;
    /// <summary>現在ヘルプを開いているか</summary>
    bool _isHelpping;

    /// <summary>ヘルプを開いているかチェック</summary>
    public bool CheckOpenHelp() => _isHelpping;


    void Awake()
    {
        _background.SetActive(false);
        _frame.localScale = Vector3.zero;
        // 外部からenabledをtrueにされるまでヘルプを開かせない
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

    /// <summary>ヘルプを開く</summary>
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

    /// <summary>ヘルプを閉じる</summary>
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
    /// ヘルプをアニメーションさせないで閉じる
    /// このシーンでは二度と開かないようにする場合に使う
    /// </summary>
    public void ForcedClosePanel()
    {
        _background.SetActive(false);
        _frame.gameObject.SetActive(false);
        enabled = false;
    }
}
