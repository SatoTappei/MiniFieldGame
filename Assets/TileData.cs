using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>キャラクターがそのマスに侵入できるかの判定をするのに使う</summary>
public enum TileType
{
    Floor,
    Wall,
}

/// <summary>
/// タイル自体のデータ
/// </summary>
public class TileData : MonoBehaviour
{
    /// <summary>このタイルの種類</summary>
    [SerializeField] TileType _type;

    public TileType Type { get => _type; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}