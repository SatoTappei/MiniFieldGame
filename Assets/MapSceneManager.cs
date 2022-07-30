using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapSceneManager : MonoBehaviour
{
    // ゲームオーバーを表示するテキスト
    public GameObject _gameOver;

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
        var lines = _mapData.Split('\n').ToList();
        map.BuildMap(lines);
    }
}
