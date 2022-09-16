using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MapSceneManager : MonoBehaviour
{
    // ゲームオーバーを表示するテキスト
    public GameObject _gameOver;
    public bool IsAutoGenerate = true;
    [SerializeField] Map.GenerateParam Generateparam;

    [TextArea(3, 15)]
    public string _mapData =
        "000P\n" +
        "0+++\n" +
        "0000\n" +
        "+++0\n" +
        "G000\n";

    void Awake()
    {
        _gameOver.SetActive(false);
        var map = GetComponent<Map>();
        var saveData = dd.Recover();
        if(saveData != null)
        {
            try
            {
                map.BuildMap(saveData._mapData);

                _mapData = saveData._mapData.Aggregate("", (_s, _c) => _s + _c + '\n');
                var player = FindObjectOfType<Player>();
                player.Recover(saveData);
                return;
            }
            catch(Exception e)
            {
                // 復元に失敗したら普通のマップ生成を行う
                Debug.LogWarning($"Fail to Recover SaveData..." + e.Message);
            }
        }

        if (IsAutoGenerate)
        {
            map.GenerateMap(Generateparam);
        }
        else
        {
            var lines = _mapData.Split('\n').ToList();
            map.BuildMap(lines);
        }
    }

    void Update()
    {
        // デバッグ用:Spaceを押したらマップが生成されるように
        if (Input.GetKeyDown(KeyCode.Space)) GenerateMap();
    }

    public void GenerateMap()
    {
        var map = GetComponent<Map>();
        map.DestroyMap();
        map.GenerateMap(Generateparam);
    }
}
