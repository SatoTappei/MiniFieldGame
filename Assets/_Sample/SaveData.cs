using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// セーブデータの処理
/// </summary>
[Serializable]
public class SaveData
{
    public int _level;
    public int _life;
    public int _attack;
    public int _exp;
    public string _weaponName;
    public int _weaponAttack;
    public List<string> _mapData;

    public void Save()
    {
        var json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("save", json);
    }

    public static SaveData Recover()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            var json = PlayerPrefs.GetString("save");
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            return null;
        }
    }

    public static void Destroy()
    {
        PlayerPrefs.DeleteAll();
    }
}
