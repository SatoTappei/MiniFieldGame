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
    Enemy,
    Treasure,
    FoodTreasure,
    WeaponTreasure,
    Trap,
    FoodTrap,
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

    /// <summary>マップの生成に用いる</summary>
    Dictionary<MassType, MassData> MassDataDict { get; set; }
    /// <summary>マップの生成に用いる、その文字が定義されているかを判定</summary>
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

                // 文字に対応したMassData
                var massData = MapCharDict[ch];
                var mass = new Mass();
                var pos = CalcMapPos(i, Data.Count);
                // 下のif文は後々修正する
                if (massData.isCharacter)
                {
                    mass.existObject = Instantiate(massData.prefab, transform);
                    var mapObject = mass.existObject.GetComponent<MapObjectBase>();
                    mapObject.SetPosAndForward(new Vector2Int(i, Data.Count), Direction.South);

                    // キャラクターの時は道も一緒に作成する
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
    public Vector3 CalcMapPos(Vector2Int pos) => CalcMapPos(pos.x, pos.y);

    public Vector2Int CalcDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Vector2Int.down;
            case Direction.South: return Vector2Int.up;
            case Direction.East: return Vector2Int.right;
            case Direction.West: return Vector2Int.left;
            default: throw new NotImplementedException();
        }
    }

    public (Mass mass, Vector2Int movedPos) GetMovePos(Vector2Int currentPos, Direction moveDir)
    {
        var offset = CalcDirection(moveDir);
        var movedPos = currentPos + offset;
        if (movedPos.x < 0 || movedPos.y < 0) return (null, currentPos);
        if (movedPos.y >= MapSize.y) return (null, currentPos);
        var line = Data[movedPos.y];
        if (movedPos.x >= line.Count) return (null, currentPos);

        var mass = line[movedPos.x];
        return (mass, movedPos);
    }

    /// <summary>右回りの時の次の方向を返す</summary>
    public static Direction TurnRightDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Direction.East;
            case Direction.South: return Direction.West;
            case Direction.East: return Direction.South;
            case Direction.West: return Direction.North;
            default: throw new NotImplementedException();
        }
    }

    /// <summary>左回りの時の次の方向を返す</summary>
    public static Direction TurnLeftDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Direction.West;
            case Direction.South: return Direction.East;
            case Direction.East: return Direction.North;
            case Direction.West: return Direction.South;
            default: throw new NotImplementedException();
        }
    }
}
