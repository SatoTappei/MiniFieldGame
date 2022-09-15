using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成するステージのデータ
/// </summary>
[CreateAssetMenu]
public class StageDataSO : ScriptableObject
{
    /// <summary>生成されるステージの幅</summary>
    [Header("ステージの幅"), SerializeField] int _width;
    /// <summary>生成されるステージの高さ</summary>
    [Header("ステージ高さ"), SerializeField] int _height;
    /// <summary>ゲームオーバーまでのターン制限</summary>
    [Header("制限時間"), SerializeField] int _turnLimit;
    /// <summary>フロアに生成される敵の最大数</summary>
    [Header("敵の最大数"), SerializeField] int _maxEnemy;
    /// <summary>フロアに生成する敵</summary>
    [Header("生成する敵"), SerializeField] GameObject[] _enemies;
    /// <summary>フロアに生成するアイテムの最大数</summary>
    [Header("アイテムの最大数"), SerializeField] int _maxItem;
    /// <summary>フロアに生成するアイテム</summary>
    [Header("生成するアイテム"), SerializeField] GameObject[] _items;
    /// <summary>フロアに生成される障害物の最大数</summary>
    //[Header("障害物の最大数"), SerializeField] int _maxObst;
    /// <summary>フロアに生成する障害物</summary>
    //[Header("生成する障害物"), SerializeField] GameObject[] _obstacles;
    /// <summary>フロアに生成されるコインの最大数</summary>
    [Header("コインの最大数"), SerializeField] int _maxCoin;
    /// <summary>ステージに生成するコイン</summary>
    [Header("生成するコイン"), SerializeField] GameObject[] _coins;
    /// <summary>フロアに雲を生成するか</summary>
    [Header("フロアに雲を生成するか"), SerializeField] bool _isCoudy;
    /// <summary>マップを自動生成するコンポーネント</summary>
    [Header("マップを自動生成するコンポーネント"), SerializeField] MapGeneratorBase _mapGenerator;

    public int Width { get => _width; }
    public int Height { get => _height; }
    public int TurnLimit { get => _turnLimit; }
    public int MaxEnemy { get => _maxEnemy; }
    //public GameObject[] Obstacles { get => _obstacles; }
    //public int MaxObst { get => _maxObst; }
    public GameObject[] Items { get => _items; }
    public int MaxItem { get => _maxItem; }
    public GameObject[] Enemies { get => _enemies; }
    public int MaxCoin { get => _maxCoin; }
    public GameObject[] Coins { get => _coins; }
    public bool IsCoudy { get => _isCoudy; }
    public MapGeneratorBase MapGenerator { get => _mapGenerator; }
}
