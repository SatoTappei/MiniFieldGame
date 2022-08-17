using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成するステージのデータ
/// </summary>
[CreateAssetMenu]
public class StageDataSO : ScriptableObject
{
    //Header("ステージの幅"), SerializeField[SerializeField] int _width;
    //[Header("ステージ高さ"), SerializeFieldSerializeField] int _height;
    /// <summary>ゲームオーバーまでのターン制限</summary>
    [Header("制限時間"), SerializeField] int _turnLimit;
    /// <summary>フロアに生成される敵の最大数</summary>
    [Header("敵の最大数"), SerializeField] int _maxEnemy;
    /// <summary>フロアに生成する敵</summary>
    [Header("生成する敵"), SerializeField] GameObject[] _enemies;
    /// <summary>フロアに生成されるコインの最大数</summary>
    [Header("コインの最大数"), SerializeField] int _maxCoin;
    /// <summary>ステージに生成するコイン</summary>
    [Header("生成するコイン"), SerializeField] GameObject _coin;

    public int TurnLimit { get => _turnLimit; }
    public int MaxEnemy { get => _maxEnemy; }
    public GameObject[] Enemies { get => _enemies; }
    public int MaxCoin { get => _maxCoin; }
    public GameObject Coin { get => _coin; }
}
