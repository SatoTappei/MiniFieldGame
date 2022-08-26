using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区域分割法を用いてマップを生成する
/// </summary>
public class RandomMapGenerator : MonoBehaviour
{
    /// <summary>部屋と通路を現すクラス</summary>
    public class Area
    {
        /// <summary>始点となる座標</summary>
        public Position Start { get; set; }
        /// <summary>終点になる座標</summary>
        public Position Goal { get; set; }

        /// <summary>エリアの幅を返す</summary>
        public int GetWidth() => Mathf.Abs(Goal.X - Start.X + 1);
        /// <summary>エリアの高さを返す</summary>
        public int GetHeight() => Mathf.Abs(Goal.Y - Start.Y + 1);

        public Area(int sX, int sY, int gX, int gY)
        {
            Start.X = sX;
            Start.Y = sY;
            Goal.X = gX;
            Goal.Y = gY;
        }

        // 基準となる座標を表すためのクラス
        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Position(int sX, int sY)
            {
                X = sX;
                Y = sY;
            }
        }
    }

    /// <summary>部屋の幅と高さの最小値</summary>
    const int OneSideMin = 6;
    /// <summary>マップの幅</summary>
    int _mapWidth;
    /// <summary>マップの高さ</summary>
    int _mapHeight;
    /// <summary>部屋の最大数</summary>
    int _roomNumMax;
    /// <summary>マップを分割してできた部屋のリスト</summary>
    List<Area> _rooms = new List<Area>();
    /// <summary>マップを分割してできた通路のリスト</summary>
    List<Area> _passes = new List<Area>();
    /// <summary>部屋から延びる通路のリスト</summary>
    List<Area> _roomPasses = new List<Area>();
    /// <summary>マップを分割した後の区域のリスト</summary>
    List<Area> _areas = new List<Area>();

    /// <summary>TODO:要検証、ランダムな値を返す</summary>
    int GetRandomValue(int min, int max) => min + Mathf.FloorToInt(Random.value * (max - min + 1));

    /// <summary>幅と高さに応じたマップを生成し、文字列にして返す</summary>
    public string GenerateRandomMap(int width, int height)
    {
        // 部屋の最大数を設定(16*16のマップで最大6部屋を基準に設定)
        _roomNumMax = width * height / 42;

        string[,] map = new string[height, width];
        GenerateRoomAndPass(map);

        return ArrayToString(map);
    }

    /// <summary>二次元配列を文字列にして返す</summary>
    string ArrayToString(string[,] array)
    {
        string str = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
                str += array[i, j];
            if (i < array.GetLength(0) - 1)
                str += '\n';
        }
        return str;
    }

    /// <summary>部屋と通路を生成する</summary>
    void GenerateRoomAndPass(string[,] map)
    {
        // マップ全体を区域リストに追加する
        _areas.Add(new Area(0, 0, _mapWidth - 1, _mapHeight - 1));
        // 垂直に分割するか
        bool isVert = true;
        // 部屋数の最大数分、分割を試行する
        for (int i = 0; i < _roomNumMax; i++)
        {
            DevideMap(isVert);
            isVert = !isVert;
        }
    }

    /// <summary>マップを区域に分割する</summary>
    /// <param name="isVertical">垂直に分割するか</param>
    void DevideMap(bool isVert)
    {
        // 分割した区域を一時的に格納しておくリスト
        List<Area> devideds = new List<Area>();
        // 区域リストの中身を分割していく
        foreach (var area in _areas)
        {
            // 垂直に分割する(部屋が上下に分かれる)場合は区域の高さが分割しても2区域に分けられれば分割する
            if (isVert && area.GetHeight() >= OneSideMin * 2 + 1)
            {
                // 高さから1辺の最小の長さの2倍を引いた値が余裕
                int space = area.GetHeight() - OneSideMin * 2;
                // 分割する位置をランダムで決める
                int devidePos = area.Start.Y + OneSideMin + GetRandomValue(1, space) - 1;
                // 分割した境界線を通路として保存しておく
                _passes.Add(new Area(area.Start.X, devidePos, area.Goal.X, devidePos));
                // 分割して出来た側の区域を一時的に格納しておく
                devideds.Add(new Area(area.Start.X, devidePos + 1, area.Goal.X, area.Goal.Y));
                // 分割した側の区域の高さを境界線の1マス上まで縮める
                area.Goal.Y = devidePos - 1;
            }
            // 水平に分割する(部屋が左右に分かれる)場合は区域の幅が分割しても2区域に分けられれば分割する
            else if(!isVert && area.GetWidth() >= OneSideMin * 2 + 1)
            {
                // 幅から1辺の最小の長さの2倍を引いた値が余裕
                int space = area.GetWidth() - OneSideMin * 2;
                // 分割する位置をランダムで決める
                int devidePos = area.Start.X + OneSideMin + GetRandomValue(1, space) - 1;
                // 分割した境界線を通路として保存しておく
                _passes.Add(new Area(devidePos, area.Start.Y, devidePos, area.Goal.Y));
                // 分割して出来た側の区域を一時的に格納しておく
                devideds.Add(new Area(devidePos + 1, area.Start.Y, area.Goal.X, area.Goal.Y));
                // 分割した側の区域の高さを境界線の1マス左まで縮める
                area.Goal.X = devidePos - 1;
            }
        }

        // 区域のリストに分割してできた区域を追加する
        // ここで追加しないとforeachの中身が変わってしまうので注意
        _areas.AddRange(devideds);
    }
}
