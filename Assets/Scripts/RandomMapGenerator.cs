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
        public int GetWidth() => Goal.X - Start.X + 1;
        /// <summary>エリアの高さを返す</summary>
        public int GetHeight() => Goal.Y - Start.Y + 1;

        public Area(int sX, int sY, int gX, int gY)
        {
            Start = new Position(sX, sY);
            Goal = new Position(gX, gY);
        }

        // 基準となる座標を表すためのクラス
        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Position(int x, int y)
            {
                X = x;
                Y = y;
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
        _mapWidth = width;
        _mapHeight = height;
        // 部屋の最大数を設定(16*16のマップで最大6部屋を基準に設定)
        _roomNumMax = width * height / 42;

        string[,] map = new string[height, width];
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                map[i, j] = "W";

        Devide();
        GenerateRoomInArea();

        foreach (Area pass in _passes)
            for (int x = pass.Start.X; x <= pass.Goal.X; x++)
                for (int y = pass.Start.Y; y <= pass.Goal.Y; y++)
                    map[x, y] = "O";
        foreach (Area roomPass in _roomPasses)
            for (int x = roomPass.Start.X; x <= roomPass.Goal.X; x++)
                for (int y = roomPass.Start.Y; y <= roomPass.Goal.Y; y++)
                    map[x, y] = "O";
        foreach (Area room in _rooms)
            for (int x = room.Start.X; x <= room.Goal.X; x++)
                for (int y = room.Start.Y; y <= room.Goal.Y; y++)
                    map[x, y] = "O";

        CutPass(ref map);
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

    /// <summary>マップを分割する</summary>
    void Devide()
    {
        // マップ全体を区域リストに追加する
        _areas.Add(new Area(0, 0, _mapWidth - 1, _mapHeight - 1));
        // 垂直に分割するか
        bool isVert = true;
        // 部屋数の最大数分、分割を試行する
        for (int i = 0; i < _roomNumMax; i++)
        {
            DevideArea(isVert);
            isVert = !isVert;
        }
    }

    /// <summary>指定された方向に区域として分割する</summary>
    /// <param name="isVertical">垂直に分割するか</param>
    void DevideArea(bool isVert)
    {
        // 分割した区域を一時的に格納しておくリスト
        List<Area> devideds = new List<Area>();
        // 区域リストの中身を分割していく
        foreach (var area in _areas)
        {
            // 区域の数が2つ以上の場合は40％の確率で分割しない
            if (_areas.Count > 1 && Random.value > 0.4f)
                continue;
            Debug.Log(area.GetWidth() + " " + area.GetHeight());
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

    /// <summary>区域に部屋を生成する</summary>
    void GenerateRoomInArea()
    {
        // いる？:部屋のない区画が偏らないようにリストをシャッフルする
        _areas.Sort((a, b) => GetRandomValue(0, 1) - 1);

        foreach (var area in _areas)
        {
            // 部屋の数が最大数の半分以上の場合、30％の確率で部屋を生成しない
            if (_rooms.Count > _roomNumMax / 2 && Random.value > 0.3f)
                continue;
            // 部屋を生成するためのスペースを計算
            int spaceX = area.GetWidth() - OneSideMin + 1;
            int spaceY = area.GetHeight() - OneSideMin + 1;
            // 部屋を生成する座標に幅を持たせる
            int randomX = GetRandomValue(1, spaceX);
            int randomY = GetRandomValue(1, spaceY);
            // 座標を算出
            int startX = area.Start.X + randomX;
            int startY = area.Start.Y + randomY;
            int goalX = area.Goal.X - GetRandomValue(0, spaceX - randomX) - 1;
            int goalY = area.Goal.Y - GetRandomValue(0, spaceY - randomY) - 1;
            // 部屋のリストへ追加
            Area room = new Area(startX, startY, goalX, goalY);
            _rooms.Add(room);
            // 通路を作る
            GeneratePassFromRoom(area, room);
        }
    }

    /// <summary>部屋から通路を作る</summary>
    void GeneratePassFromRoom(Area area, Area room)
    {
        // 各端っこから1マスでも離れていれば通路を伸ばす候補として追加
        List<string> dirs = new List<string>();
        if (area.Start.X > 0) 
            dirs.Add("Left");
        if (area.Goal.X < _mapWidth - 1)
            dirs.Add("Right");
        if (area.Start.Y > 0)
            dirs.Add("Up");
        if (area.Goal.Y < _mapHeight - 1)
            dirs.Add("Down");
        // いる？:通路の有無が偏らないよう、リストをシャッフルする
        dirs.Sort((a, b) => GetRandomValue(0, 1) - 1);
        // その部屋から始めに伸びる一本かどうか
        bool isFirst = true;

        foreach (var dir in dirs)
        {
            // 最初の1本でなければ80％の確率で通路が作られない
            if (!isFirst && Random.value < 0.8f)
                continue;
            isFirst = false;

            // 区域の端から部屋まで伸びる通路を作る
            int random;
            switch (dir)
            {
                case "Left":
                    random = room.Start.Y + GetRandomValue(1, room.GetHeight()) - 1;
                    _roomPasses.Add(new Area(area.Start.X, random, room.Start.X - 1, random));
                    break;
                case "Right":
                    random = room.Start.Y + GetRandomValue(1, room.GetHeight()) - 1;
                    _roomPasses.Add(new Area(room.Goal.X + 1, random, area.Goal.X, random));
                    break;
                case "Up":
                    random = room.Start.X + GetRandomValue(1, room.GetWidth()) - 1;
                    _roomPasses.Add(new Area(random, area.Start.Y, random, room.Start.Y - 1));
                    break;
                case "Down":
                    random = room.Start.X + GetRandomValue(1, room.GetWidth()) - 1;
                    _roomPasses.Add(new Area(random, room.Goal.Y + 1, random, area.Goal.Y));
                    break;
            }
        }
    }

    /// <summary>通路を削除する</summary>
    void CutPass(ref string[,] map)
    {
        // どの部屋の通路からも接続されなかった通路を削除する
        for (int i = _passes.Count - 1; i >= 0; i--)
        {
            // Area型のインスタンスを通路のリストのi番目で初期化
            Area pass = _passes[i];
            // 通路の高さが1より大きければ縦向きの通路とみなす
            bool isVert = pass.GetHeight() > 1;
            // 削除の対象となるかどうか
            bool isCut = true;

            if (isVert)
            {
                for (int y = pass.Start.Y; y <= pass.Goal.Y; y++)
                {
                    // 通路の左右に他の通路があれば接続されているとみなし、削除対象にしない
                    if (map[pass.Start.X - 1, y] == "O" || map[pass.Start.X + 1, y] == "O")
                    {
                        isCut = false;
                        break;
                    }
                }
            }
            else
            {
                for (int x = pass.Start.X; x <= pass.Goal.X; x++)
                {
                    // 通路の上下に他の通路があれば接続されているとみなし、削除対象にしない
                    if (map[x, pass.Start.Y - 1] == "O" || map[x, pass.Start.Y + 1] == "O")
                    {
                        isCut = false;
                        break;
                    }
                }
            }

            // 削除対象となった通路を削除する
            if (isCut)
            {
                _passes.Remove(pass);

                // マップ上から削除
                if (isVert)
                    for (int y = pass.Start.Y; y <= pass.Goal.Y; y++)
                        map[pass.Start.X, y] = "W";
                else
                    for (int x = pass.Start.X; x <= pass.Goal.X; x++)
                        map[x, pass.Start.Y] = "W";
            }
        }

        // 上下を基準としてマップ端の壁まで伸びている通路を別の通路との接続点まで削除する
        for (int x = 0; x < _mapWidth - 1; x++)
        {
            // マップの上側を調べる
            if (map[x, 0] == "O")
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    // 左右に道のマスが見つかったら繋がっているとみなしてその地点で削除をやめる
                    if (map[x - 1, y] == "O" || map[x + 1, y] == "O")
                        break;
                    map[x, y] = "W";
                }
            }
            // マップの下側を調べる
            if (map[x, _mapHeight - 1] == "O")
            {
                for (int y = _mapHeight - 1; y >= 0; y--)
                {
                    // 左右に道のマスが見つかったら繋がっているとみなしてその地点で削除をやめる
                    if (map[x - 1, y] == "O" || map[x + 1, y] == "O")
                        break;
                    map[x, y] = "W";
                }
            }
        }
        // 左右を基準としてマップの端の壁まで伸びている通路を別の通路との接続点まで削除する
        for (int y = 0; y < _mapHeight - 1; y++)
        {
            // マップの左側を調べる
            if (map[0, y] == "O")
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    // 上下に道のマスが見つかったら繋がっているとみなしてその地点で削除をやめる
                    if (map[x, y - 1] == "O" || map[x, y + 1] == "O")
                        break;
                    map[x, y] = "W";
                }
            }
            // マップの右側を調べる
            if (map[_mapWidth - 1, y] == "O")
            {
                for (int x = _mapWidth - 1; x >= 0; x--)
                {
                    // 上下に道のマスが見つかったら繋がっているとみなしてその地点で削除をやめる
                    if (map[x, y - 1] == "O" || map[x, y + 1] == "O")
                        break;
                    map[x, y] = "W";
                }
            }
        }
    }
}
