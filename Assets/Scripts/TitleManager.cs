using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルト画面全体を管理・制御する
/// </summary>
public class TitleManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>ゲームプレイのシーンへ推移する</summary>
    public void MoveGamePlay()
    {
        // ゲーム開始時に合計スコアを0にする
        GameManager._instance.UpdateTotalScore(0);

        GameManager._instance.FadeOut("GamePlay");
    }
}
