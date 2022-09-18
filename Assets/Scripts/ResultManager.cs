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

    /// <summary>入力されたプレイヤーの名前を保持する</summary>
    string _scoreName;
    /// <summary>名前の入力が終わったらtrueになる</summary>
    bool _finishedNameEntry;

    IEnumerator Start()
    {
        // スコアの表示が終わるまでは入力できないようにする
        _resultUIManager.SetSelectedDummyButton();
        // フェードインが終わるのを待つ
        yield return new WaitWhile(() => GameManager._instance.IsFading);
        // スコアを表示させる
        SoundManager._instance.Play("SE_カタカタ");
        yield return StartCoroutine(_resultUIManager.DispClearScore(GameManager._instance.TotalScore));
        // スコアネームを入力する
        _resultUIManager.SetSelectedDefaultButton();
        // スコアネームを入力し終えるまで待つ
        yield return new WaitUntil(() => _finishedNameEntry);
        // 前回までのリザルトのデータをロードする
        SaveDataManager.Load();
        // 今回のリザルトをセーブする
        SaveDataManager.Save(_scoreName, GameManager._instance.TotalScore);
        // ランキングに反映させる
        SetRanking();
        // ネームエントリーを終えてランキング画面へ移行する
        MoveRanking();
        // ランキングに反映
        SetRanking();
    }

    void Update()
    {
        //// セーブとロードのテスト、終わったら消す
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    string[] ns = { "TPI", "TKS", "HOG", "PIY" };
        //    int r = Random.Range(0, 4);
        //    int score = Random.Range(0, 10000);
        //    SaveDataManager.Save(ns[r], score);
        //    Debug.Log("セーブデータをセーブしました");
        //}
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    SaveDataManager.Load();
        //    Debug.Log("セーブデータをロードしました");
        //}
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
        SoundManager._instance.Play("SE_タイトルボタン");
        // ダミーのボタンを選択状態にして入力を防ぐ
        _resultUIManager.SetSelectedDummyButton();
        // ネームエントリーが終わったフラグを立てる
        _finishedNameEntry = true;
    }

    /// <summary>ランキング画面に移行する</summary>
    void MoveRanking()
    {
        // スコアネーム入力画面を閉じてランキング画面へ移行する
        _resultUIManager.ChangeToRankingPanel();
        // ランキング画面へ移行して少し待ってからボタンを押せるようにする
        DOVirtual.DelayedCall(1.5f, () =>
        {
            _resultUIManager.SetSelectedBackTitleButton();
        });
    }

    /// <summary>ランキングに反映させる</summary>
    void SetRanking()
    {
        int max = SaveDataManager.GetDataCount();
        for (int i = 1; i <= max; i++)
        {
            SaveDataManager.Result result = SaveDataManager.GetRankResult(i);
            _resultUIManager.SetRankingItem(i, result._name, result._score);
        }
    }

    /// <summary>タイトルに戻る</summary>
    public void MoveTitle()
    {
        GameManager._instance.FadeOut("Title");
    }
}
