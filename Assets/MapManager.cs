using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップの生成を制御する
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary生成するマップの文字列</summary>
    [TextArea(7, 7), SerializeField] string _mapStr;


    void Start()
    {
        MapGenerator mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateMap(_mapStr);
    }

    void Update()
    {
        
    }
}
