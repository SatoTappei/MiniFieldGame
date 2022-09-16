using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// リザルト画面全体を管理・制御する
/// </summary>
public class ResultManager : MonoBehaviour
{
    [SerializeField] ResultUIManager _resultUIManager;

    string _scoreName;

    IEnumerator Start()
    {
        // スコアの表示が終わるまでは入力できないようにする
        _resultUIManager.SetSelectedDummyButton();
        // スコアを表示させる
        yield return StartCoroutine(_resultUIManager.DispClearScore(9999/*GameManager._instance.TotalScore*/));
        // スコアネームを入力する
        _resultUIManager.SetSelectedDefaultButton();
        yield return null;
    }

    void Update()
    {
        
    }

    /// <summary>スコアネームに文字を追加する</summary>
    public void AddScoreName(NameInputButton nib)
    {
        _scoreName += nib.Char;
        _resultUIManager.SetScoreName(_scoreName);

        // 入力された名前が3文字になったらスコアネームの入力を終える
        if (_scoreName.Length == 3)
            EndInputScoreName();
        else
            SoundManager._instance.Play("SE_リザルト1");
    }

    /// <summary>スコアネームの入力を終える</summary>
    public void EndInputScoreName()
    {
        // 名前の入力がない場合は既定の名前にする
        if (_scoreName == null)
        {
            _scoreName = "^^;";
            _resultUIManager.SetScoreName(_scoreName);
        }
        // ダミーのボタンを選択状態にして入力を防ぐ
        _resultUIManager.SetSelectedDummyButton();
        // スコアネーム入力画面を閉じてランキング画面へ移行する
        _resultUIManager.ChangeToRankingPanel();
        SoundManager._instance.Play("SE_タイトルボタン");
        // ランキング画面へ移行して少し待ってからボタンを押せるようにする
        DOVirtual.DelayedCall(1.5f, () =>
        {
            _resultUIManager.SetSelectedBackTitleButton();
        });
    }

    /// <summary>タイトルに戻る</summary>
    public void MoveTitle()
    {
        GameManager._instance.FadeOut("Title");
    }
}
