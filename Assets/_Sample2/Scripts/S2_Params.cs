using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのパラメーター
/// </summary>
[System.Serializable]
public class S2_Params
{
    public int id; // ユニークID
    public int lv; // レベル
    public int hp; // HP
    public int hpmax; // 最大HP
    public int str; // 力
    public int exp; // 獲得した経験値
    public int xp; // 倒した時に得られる経験値
    public int def; // 防御
}
