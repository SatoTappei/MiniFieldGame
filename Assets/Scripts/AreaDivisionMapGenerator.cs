using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 区域分割法を用いてマップを生成する
/// </summary>
public class AreaDivisionMapGenerator : MapGeneratorBase
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
    const int OneSideMin = 5;
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
    /// <summary>マップを分割した後の区域のリスト</summary>
    List<Area> _areas = new List<Area>();

    /// <summary>幅と高さに応じたマップを生成し、文字列にして返す</summary>
    public override string GenerateRandomMap(int width, int height)
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
        GenerateRoomAndPass();
        ReplaceTileChar(map, _passes, "O");
        ReplaceTileChar(map, _rooms, "O");
        ReplaceTileChar(map, GetRandomRoomMass(map), "E");
        ReplaceTileChar(map, GetRandomRoomMass(map), "P");
        CutPass(map);
        // リストの中身を全部Clearする(Rキーでマップを再生成するとバグる)
        _rooms.Clear();
        _passes.Clear();
        _areas.Clear();
        return ArrayToString(map);
    }

    /// <summary>リストの中身に対応する箇所を任意の文字に置き換える</summary>
    void ReplaceTileChar(string[,] map, List<Area> list, string tile)
    {
        foreach (Area area in list)
            for (int x = area.Start.X; x <= area.Goal.X; x++)
                for (int y = area.Start.Y; y <= area.Goal.Y; y++)
                    map[x, y] = tile;
    }

    /// <summary>マップを分割する</summary>
    void Devide()
    {
        // マップ全体を区域リストに追加する
        _areas.Add(new Area(0, 0, _mapWidth - 1, _mapHeight - 1));
        // 垂直に分割するか
        bool isVert = true;
        // 部屋数の最大数分、分割を試行する。
        for (int i = 0; i < _roomNumMax; i++)
        {
            // 最初の1回は必ず分割されるがそれ以降は66％の確率で区域を分割する
            if (i == 0 || Random.value <= 0.66f)
            {
                DevideArea(isVert);
                isVert = !isVert;
            }
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
            // 辺の長さから1辺の最小の長さの2倍を引いた長さが余幅
            int space = (isVert ? area.GetHeight() : area.GetWidth()) - OneSideMin * 2;
            // 区域の端から1辺の最小の長さにランダムで余幅を足した箇所で分割する
            int devide = (isVert ? area.Start.Y : area.Start.X) + OneSideMin + Random.Range(0, space);

            // 垂直に分割する(部屋が上下に分かれる)場合は区域の高さが分割しても2区域に分けられれば分割する
            if (isVert && area.GetHeight() >= OneSideMin * 2 + 1)
            {
                // 分割した境界線を通路として保存しておく
                _passes.Add(new Area(area.Start.X, devide, area.Goal.X, devide));
                // 分割して出来た側の区域を一時的に格納しておく
                devideds.Add(new Area(area.Start.X, devide + 1, area.Goal.X, area.Goal.Y));
                // 分割した側の区域の高さを境界線の1マス上まで縮める
                area.Goal.Y = devide - 1;
            }
            // 水平に分割する(部屋が左右に分かれる)場合は区域の幅が分割しても2区域に分けられれば分割する
            else if(!isVert && area.GetWidth() >= OneSideMin * 2 + 1)
            {
                // 分割した境界線を通路として保存しておく
                _passes.Add(new Area(devide, area.Start.Y, devide, area.Goal.Y));
                // 分割して出来た側の区域を一時的に格納しておく
                devideds.Add(new Area(devide + 1, area.Start.Y, area.Goal.X, area.Goal.Y));
                // 分割した側の区域の高さを境界線の1マス左まで縮める
                area.Goal.X = devide - 1;
            }
        }

        // 区域のリストに分割してできた区域を追加する
        // ここで追加しないとforeachの中身が変わってしまうので注意
        _areas.AddRange(devideds);
    }

    /// <summary>区域に部屋と通路を生成する</summary>
    void GenerateRoomAndPass()
    {
        //リストをシャッフルする
        _areas = _areas.OrderBy(a => System.Guid.NewGuid()).ToList();

        foreach (var area in _areas)
        {
            // 部屋の数が最大数の半分以上の場合、30％の確率で部屋を生成しない
            if (_rooms.Count > _roomNumMax / 2 && Random.value > 0.3f)
                continue;
            // 部屋を生成可能なスペースを計算
            // 区域の幅 - 部屋として成り立つ最小の幅
            int widthSpace = area.GetWidth() - OneSideMin;
            int heightSpace = area.GetHeight() - OneSideMin;
            // 部屋を生成する基点となる座標を算出
            // 区域の端(壁があるべき箇所)から1マス ～ 生成可能なスペース からランダム
            int startX = area.Start.X + Random.Range(1,widthSpace);
            int startY = area.Start.Y + Random.Range(1, heightSpace);
            // 部屋を生成する終点となる座標を算出
            // 区域の端(壁がある箇所)から-1マス - 余幅 からランダム  
            int goalX = area.Goal.X - 1 - Random.Range(0, Random.Range(1,widthSpace));
            int goalY = area.Goal.Y - 1 - Random.Range(0, Random.Range(1,heightSpace));
            // 部屋のリストへ追加
            Area room = new Area(startX, startY, goalX, goalY);
            _rooms.Add(room);
            // 通路を作るために4方向のリストを生成してシャッフルする
            List<string> dirs = new List<string>() { "Right", "Left", "Up", "Down" };
            dirs = dirs.OrderBy(d => System.Guid.NewGuid()).ToList();
            // 通路が1本以上存在するか。通路を1本生成したらtrueになる
            bool isExist = false;

            foreach (var d in dirs)
            {
                // 最初の1本以外の通路は66％の確率で生成しない
                if (isExist && Random.value < 0.66f) continue;

                int r;
                switch (d)
                {
                    // 部屋の右側から右側の壁に向かって通路を作る
                    case "Right" when area.Goal.X < _mapWidth - 1:
                        r = room.Start.Y + Random.Range(0, room.GetHeight());
                        _passes.Add(new Area(room.Goal.X + 1, r, area.Goal.X, r));
                        isExist = true;
                        break;
                    // 左側の壁から部屋の左側にに向かって通路を作る
                    case "Left" when area.Start.X > 0:
                        r = room.Start.Y + Random.Range(0, room.GetHeight());
                        _passes.Add(new Area(area.Start.X, r, room.Start.X - 1, r));
                        isExist = true;
                        break;
                    // 上側の壁から部屋の上側に向かって通路を作る
                    case "Up" when area.Start.Y > 0:
                        r = room.Start.X + Random.Range(0, room.GetWidth());
                        _passes.Add(new Area(r, area.Start.Y, r, room.Start.Y - 1));
                        isExist = true;
                        break;
                    // 部屋の下側から下側の壁に向かって通路を作る
                    case "Down" when area.Goal.Y < _mapHeight - 1:
                        r = room.Start.X + Random.Range(0, room.GetWidth());
                        _passes.Add(new Area(r, room.Goal.Y + 1, r, area.Goal.Y));
                        isExist = true;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 通路を削除する
    /// マップに反映させてから削除しないと、リストから1本とってきて、通路1マスに対して
    /// 全部の通路の各マスが隣り合ってるか調べないといけなくなる(通路の本数*1本のマス数*全部の通路の各マスの数)
    /// </summary>
    void CutPass(string[,] map)
    {
        // 上下を基準としてマップ端の壁まで伸びている通路を別の通路との接続点まで削除する
        // 左右に道のマスが見つかったら繋がっているとみなしてその地点で削除をやめる
        for (int x = 0; x < _mapWidth - 1; x++)
        {
            // マップの上側を調べる
            if (map[x, 0] == "O")
                for (int y = 0; y < _mapHeight; y++)
                    if (Cut(x, y, isVert: true)) break;
            // マップの下側を調べる
            if (map[x, _mapHeight - 1] == "O")
                for (int y = _mapHeight - 1; y >= 0; y--)
                    if (Cut(x, y, isVert: true)) break;
        }
        // 左右を基準としてマップの端の壁まで伸びている通路を別の通路との接続点まで削除する
        // 上下に道のマスが見つかったら繋がっているとみなしてその地点で削除をやめる
        for (int y = 0; y < _mapHeight - 1; y++)
        {
            // マップの左側を調べる
            if (map[0, y] == "O")
                for (int x = 0; x < _mapWidth; x++)
                    if (Cut(x, y, isVert: false)) break;
            // マップの右側を調べる
            if (map[_mapWidth - 1, y] == "O")
                for (int x = _mapWidth - 1; x >= 0; x--)
                    if (Cut(x, y, isVert: false)) break;
        }

        // 指定されたマスの左右または上下を調べて
        // 他の通路と繋がっていなければマスを壁にし、繋がっているかどうか返す
        bool Cut(int i, int j, bool isVert)
        {
            (int, int) index1 = isVert ? (i - 1, j) : (i, j - 1);
            (int, int) index2 = isVert ? (i + 1, j) : (i, j + 1);

            bool isConect = false;
            if (map[index1.Item1, index1.Item2] == "O" || map[index2.Item1, index2.Item2] == "O")
                isConect = true;
            else
                map[i, j] = "W";
            return isConect;
        }
    }

    /// <summary>部屋の中の普通の床のマスをランダムで1マス取得する</summary>
    List<Area> GetRandomRoomMass(string[,] map)
    {
        // 部屋のリストをランダムで並び替える
        _rooms = _rooms.OrderBy(r => System.Guid.NewGuid()).ToList();
        // それぞれの部屋に対して普通の床が存在するのか調べる
        foreach (Area room in _rooms)
        {
            // 部屋の中のランダムなマスを最大100回調べる
            for (int i = 0; i < 100; i++)
            {
                int x = Random.Range(room.Start.X, room.Goal.X);
                int y = Random.Range(room.Start.Y, room.Goal.Y);
                // 床のマスだったらそのマスを返す
                if (map[x, y] == "O")
                    return new List<Area>() { new Area(x, y, x, y) };
            }
        }

        Debug.LogError("床のタイルを取得できませんでした。メソッドを修正する必要があります。");
        return null;

        // 重複なしで調べられるが処理が複雑
        //foreach (Area room in _rooms)
        //{
        //    // 部屋の横の長さが入ったリスト
        //    List<int> widthList = new List<int>();
        //    for (int i = room.Start.X; i < room.Goal.X; i++)
        //    {
        //        widthList.Add(i);
        //    }
        //    // 部屋の縦の長さが入ったリスト
        //    List<int> heightList = new List<int>();
        //    for (int i = room.Start.Y; i < room.Goal.Y; i++)
        //    {
        //        heightList.Add(i);
        //    }
        //    // 横のリストから1つ選ぶ
        //    int wr = Random.Range(0, widthList.Count);
        //    // 縦のリストがなくなるまで操作
        //    while (heightList.Count > 0)
        //    {
        //        // 縦のリストから初期位置を選ぶ
        //        int hr = Random.Range(0, heightList.Count);
        //        // 指定されたマスが普通の床かどうか調べる
        //        if (map[widthList[wr], heightList[hr]] == "O")
        //        {
        //            int x = widthList[wr];
        //            int y = heightList[hr];
        //            return new List<Area>() { new Area(x, y, x, y) };
        //        }
        //        else
        //        {
        //            heightList.RemoveAt(hr);
        //        }
        //    }
        //}
    }
}
