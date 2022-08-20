using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルト画面全体を管理・制御する
/// </summary>
public class ResultManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>タイトルに戻る</summary>
    public void MoveTitle()
    {
        GameManager._instance.FadeOut("Title");
    }
}
