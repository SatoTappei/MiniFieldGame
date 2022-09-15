using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトル画面全体を管理・制御する
/// </summary>
public class TitleManager : MonoBehaviour
{
    // ゲームを始めるのボタン
    [SerializeField] Button _gameStartButton;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>ゲームプレイのシーンへ推移する</summary>
    public void MoveGamePlay()
    {
        // フェード中ならボタンが反応しない
        if (GameManager._instance.IsFading) return;
        _gameStartButton.interactable = false;

        // ゲーム開始時に合計スコアを0にする
        GameManager._instance.UpdateTotalScore(0);
        // ゲーム開始時に現在のステージ番号を初期値に戻す
        GameManager._instance.ResetStageNum();
        
        GameManager._instance.FadeOut("GamePlay");
    }

    /// <summary>ゲーム終了の処理</summary>
    public void ExitGame()
    {
        // 先にDoTweenのコンポーネントが破棄されるとエラーが出るので
        // DotWeenを使用したオブジェクトをすべて破棄する
        Destroy(GameObject.Find("BackgroundObjects"));
        Destroy(GameObject.Find("Canvas"));


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
