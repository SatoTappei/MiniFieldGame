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
        mapGenerator.SetPlayer();

        // プレイヤーは壁や移動不可のマスには配置できない <= キャラクター毎に侵入不可能の地形をenumで設定してやる
        // 同じ場所には敵を配置できないので、マス毎に上に敵もしくはプレイヤーがいるかどうか見る必要がある

    }

    void Update()
    {
        
    }
}
