using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// マップを作成する
/// </summary>

public enum MassType
{
    Road,
    Wall,
    Player,
    Goal,
}

public enum Direction
{
    North,
    South,
    East,
    West,
}

[Serializable]
public class MassData
{
    public GameObject prefab;
    public MassType type;
    public char mapChar;
    public bool isRoad;
    public bool isCharacter;
}

public class Map : MonoBehaviour
{
    public class Mass
    {
        public MassType type;
        public GameObject massGameObject;
        public GameObject existObject;
    }

    public MassData[] _massDataList;
    public Vector2 _massOffset = Vector2.one;
    public bool IsNowBuilding { get; private set; }
    public Vector2Int MapSize { get; private set; }
    /// <summary>マップ上の開始位置</summary>
    public Vector2Int StartPos { get; set; }
    /// <summary>プレイヤーの開始時の向き</summary>
    public Direction StartForward { get; set; }

    Dictionary<MassType, MassData> MassDataDict { get; set; }
    Dictionary<char, MassData> MapCharDict { get; set; }

    List<List<Mass>> Data { get; set; }
    public Mass this[int x, int y]
    {
        get => Data[y][x];
        set => Data[y][x] = value;
    }

    public MassData this[MassType type] 
    {
        get => MassDataDict[type];
    }

    /// <summary>渡された文字列のリストからマップを返す</summary>
    public void BuildMap(List<string> map)
    {
        InitMassData();

        var mapSize = Vector2Int.zero;
        Data = new List<List<Mass>>();
        foreach(var line in map)
        {
            var lineData = new List<Mass>();
            for(int i= 0; i < line.Length; ++i)
            {
                var ch = line[i];
                if (!MapCharDict.ContainsKey(ch))
                {
                    Debug.LogWarning("不明な文字がマップデータに存在しています：" + ch);
                    // 一応、始めのデータで代用する
                    ch = MapCharDict.First().Key;
                }

                var massData = MapCharDict[ch];
                var mass = new Mass();
                var pos = CalcMapPos(i, Data.Count);
                // 下のif文は後々修正する
                if (massData.isCharacter)
                {
                    massData = this[MassType.Road];
                }
                mass.type = massData.type;
                mass.massGameObject = Instantiate(massData.prefab, transform);
                mass.massGameObject.transform.position = pos;
                lineData.Add(mass);
            }
            Data.Add(lineData);

            // マップサイズの設定
            mapSize.x = Mathf.Max(mapSize.x, line.Length);
            mapSize.y++;
        }
        MapSize = mapSize;
    }

    /// <summary>マスを初期化する</summary>
    void InitMassData()
    {
        MassDataDict = new Dictionary<MassType, MassData>();
        MapCharDict = new Dictionary<char, MassData>();
        foreach(var massData in _massDataList)
        {
            MassDataDict.Add(massData.type, massData);
            MapCharDict.Add(massData.mapChar, massData);
        }
    }

    /// <summary>マップ上での位置を計算する</summary>
    public Vector3 CalcMapPos(int x, int y)
    {
        var pos = Vector3.zero;
        pos.x = x * _massOffset.x;
        pos.z = y * _massOffset.y * -1;
        return pos;
    }
}
