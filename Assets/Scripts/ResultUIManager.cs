using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// リザルトシーンのUIを管理する
/// </summary>
public class ResultUIManager : MonoBehaviour
{
    /// <summary>クリア時のスコアを表示するテキスト</summary>
    [SerializeField] Text _clearScoreText;
    /// <summary>スコアネームを表示するテキスト</summary>
    [SerializeField] Text _scoreNameText;
    /// <summary>スコアネーム入力ボタンの親</summary>
    [SerializeField] GameObject _inputButtonParent;
    /// <summary>ボタンを選択していない状態にするためのダミーのボタン</summary>
    [SerializeField] GameObject _dummyButton;
    /// <summary>スコアランキングのパネル</summary>
    [SerializeField] GameObject _rankingPanel;
    /// <summary>デフォルトのスコアネームを入力するボタン</summary>
    [SerializeField] GameObject _defaultInputButton;
    /// <summary>タイトルに戻るのボタン</summary>
    [SerializeField] GameObject _backTitleButton;
    /// <summary>ランキングに表示される各リザルトのオブジェクトの親</summary>
    [SerializeField] Transform _rankingItemParent;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>クリア時のスコアを表示する</summary>
    public IEnumerator DispClearScore(int score)
    {
        bool finish = false;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_clearScoreText.DOCounter(0, score, 2.0f));
        sequence.Append(_clearScoreText.transform.DOScale(1.2f, 0.15f));
        sequence.Append(_clearScoreText.transform.DOScale(1.0f, 0.15f));
        sequence.AppendCallback(() =>
        {
            SoundManager._instance.Play("SE_リザルト2");
            finish = true;
        });
        // TODO:ハイスコア更新時には専用の演出を入れる
        sequence.SetLink(gameObject);

        yield return new WaitUntil(() => finish);
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>スコアネームを表示する</summary>
    public void SetScoreName(string name) => _scoreNameText.text = name;

    /// <summary>スコアネームの入力パネルを閉じる</summary>
    public void ChangeToRankingPanel()
    {
        // スコアネームをフラッシュさせる
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_scoreNameText.DOFade(0, 0.33f).SetEase(Ease.Flash, 5));
        sequence.Append(_scoreNameText.DOFade(255, 0.01f));
        // スコアネーム入力画面を下にスライドさせる
        Transform clearScorePanel = _inputButtonParent.transform.parent;
        sequence.Append(clearScorePanel.DOMoveY(-600, 0.25f).SetDelay(1.0f).SetRelative());
        // ランキングパネルを下にスライドさせる
        sequence.Join(_rankingPanel.transform.DOMoveY(-600, 0.25f).SetRelative());
    }

    /// <summary>ランキングの各リザルトのオブジェクトをセットする</summary>
    public void SetRankingItem(int rank, string name, int score)
    {
        if (rank < 1 || rank > _rankingItemParent.childCount)
        {
            Debug.LogWarning("渡された順位を表示するオブジェクトがありません。:" + rank);
            return;
        }

        GameObject go = _rankingItemParent.GetChild(rank - 1).gameObject;
        go.GetComponent<ResultRankingItem>().SetResult(name, score);
    }

    /// <summary>ダミーのボタンを選択状態にしてキーの入力を防ぐ</summary>
    public void SetSelectedDummyButton() => EventSystem.current.SetSelectedGameObject(_dummyButton);

    /// <summary>デフォルトのボタンを選択状態にしてキーの入力を受け付ける</summary>
    public void SetSelectedDefaultButton() => EventSystem.current.SetSelectedGameObject(_defaultInputButton);

    /// <summary>タイトルに戻るのボタンを選択状態にしてキーの入力を受け付ける</summary>
    public void SetSelectedBackTitleButton() => EventSystem.current.SetSelectedGameObject(_backTitleButton);
}
