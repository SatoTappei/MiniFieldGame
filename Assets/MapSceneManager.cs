using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapSceneManager : MonoBehaviour
{
    [TextArea(3, 15)]
    public string _mapData =
        "000P\n" +
        "0+++\n" +
        "0000\n" +
        "+++0\n" +
        "G000\n";

    void Awake()
    {
        var map = GetComponent<Map>();
        var lines = _mapData.Split('\n').ToList();
        map.BuildMap(lines);
    }
}
