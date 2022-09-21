using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 穴掘り法を用いてマップを生成する
/// </summary>
public class DiggingMapGenerator : MapGeneratorBase
{
    /// <summary>穴を掘る開始地点となる座標のリスト</summary>
    List<(int, int)> _startMasses = new List<(int, int)>();
    /// <summary>ゴールを設置する候補となるマスのリスト</summary>
    List<(int, int)> _goalMasses = new List<(int, int)>();

    /// <summary>幅と高さに応じたマップを生成し、文字列にして返す</summary>
    public override string GenerateRandomMap(int width, int height)
    {
        // 渡された数が偶数なら-1して奇数に直す
        int w = width % 2 != 0 ? width : width - 1;
        int h = height % 2 != 0 ? height : height - 1;
        // 外周を通路、それ以外を壁にする
        string[,] map = new string[h, w];
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                map[i, j] = i * j == 0 || i == h - 1 || j == w - 1 ? "O" : "W";
        // 穴を掘る
        _startMasses.Add((1, 1));
        // 一本道だった際にゴールまたはスタートが生成されないのを防ぐためにゴールマスとしても追加
        _goalMasses.Add((1, 1));
        DiggingPass(map, _startMasses);
        // 外周を壁に戻す
        for (int i = 0; i < map.GetLength(0); i++)
        {
            map[i, 0] = "W";
            map[i, map.GetLength(1) - 1] = "W";
        }
        for (int i = 0; i < map.GetLength(1); i++)
        {
            map[0, i] = "W";
            map[map.GetLength(0) - 1, i] = "W";
        }
        // 水路を設置する
        SetWaterPath(map, width, height, 10);
        // ゴールとスタートを設置する
        SetSpotRandom(map, "E");
        SetSpotRandom(map, "P");

        return ArrayToString(map);
    }

    /// <summary>通路を掘る</summary>
    void DiggingPass(string[,] map, List<(int,int)> startMasses)
    {
        // 開始座標のリストの中からランダムに決定
        int startIndex = Random.Range(0, startMasses.Count);
        // 開始座標をセット
        int x = startMasses[startIndex].Item1;
        int y = startMasses[startIndex].Item2;
        // 開始座標のリストから削除する
        startMasses.RemoveAt(startIndex);

        while (true)
        {
            // 掘り進める方向を文字列で格納するリスト
            List<string> dirs = new List<string>();
            // 上下左右、2マス先まで壁かどうか調べる
            if (map[x, y - 1] == "W" && map[x, y - 2] == "W")
                dirs.Add("Up");
            if (map[x, y + 1] == "W" && map[x, y + 2] == "W")
                dirs.Add("Down");
            if (map[x - 1, y] == "W" && map[x - 2, y] == "W")
                dirs.Add("Left");
            if (map[x + 1, y] == "W" && map[x + 2, y] == "W")
                dirs.Add("Right");
            // 掘れる方向がなければループを抜ける
            if (dirs.Count == 0) break;
            // 開始座標を掘る
            map[x, y] = "O";
            // 掘る方向をランダムに決める
            int dirIndex = Random.Range(0, dirs.Count);
            switch (dirs[dirIndex])
            {
                case "Up":
                    DiggingMass(map, x, --y);
                    DiggingMass(map, x, --y);
                    break;
                case "Down":
                    DiggingMass(map, x, ++y);
                    DiggingMass(map, x, ++y);
                    break;
                case "Left":
                    DiggingMass(map, --x, y);
                    DiggingMass(map, --x, y);
                    break;
                case "Right":
                    DiggingMass(map, ++x, y);
                    DiggingMass(map, ++x, y);
                    break;
            }
        }

        // 現在の座標をゴールの候補マスのリストに追加する
        _goalMasses.Add((x, y));

        // もし開始座標のリストの中身があるならそこから通路を掘る
        if (startMasses.Count > 0)
            DiggingPass(map, startMasses);
    }

    /// <summary>指定されたマスを掘る</summary>
    void DiggingMass(string[,] map, int x, int y)
    {
        map[x, y] = "O";
        // もしその座標がx,y共に奇数なら開始座標のリストに追加する
        if (x * y % 2 != 0)
            _startMasses.Add((x, y));
    }

    /// <summary>
    /// 水路を引く
    /// </summary>
    /// <param name="tri">試行回数(大きければ水路が増える可能性が上がる)</param>
    void SetWaterPath(string[,] map, int width, int height,int tri)
    {
        for (int i = 0; i < tri; i++)
        {
            int wr = Random.Range(1, width / 2) * 2;
            int hr = Random.Range(1, height / 2) * 2;
            for (int j = 0; j < 2; j++)
            {
                (int, int)[] pair = { (1, 0), (-1, 0), (0, 1), (0, -1) };
                int dr = Random.Range(0, 4);

                int addX = pair[dr].Item1;
                int addY = pair[dr].Item2;

                while (wr + addX >= 1 && wr + addX < width - 1 &&
                    hr + addY >= 1 && hr + addY < height - 1)
                {
                    if (map[wr + addX, hr + addY] == "O")
                    {
                        break;
                    }
                    map[wr + addX, hr + addY] = "S";
                    addX += pair[dr].Item1;
                    addY += pair[dr].Item2;
                }
            }
        }
    }

    /// <summary>ランダムな行き止まりの位置に任意の文字を設置する</summary>
    void SetSpotRandom(string[,] map, string Char)
    {
        // ゴール候補のマスのリストの中から床のマスを探す
        foreach ((int, int) mass in _goalMasses
            .OrderBy(r => System.Guid.NewGuid())
            .Where(i => map[i.Item1,i.Item2] == "O"))
        {
            // 3方向が壁になっているマスを探す
            int count = 0;
            if (map[mass.Item1 - 1, mass.Item2] == "W" ||
                map[mass.Item1 - 1, mass.Item2] == "S") count++;
            if (map[mass.Item1 + 1, mass.Item2] == "W" ||
                map[mass.Item1 + 1, mass.Item2] == "S") count++;
            if (map[mass.Item1, mass.Item2 - 1] == "W" ||
                map[mass.Item1, mass.Item2 - 1] == "S") count++;
            if (map[mass.Item1, mass.Item2 + 1] == "W" ||
                map[mass.Item1, mass.Item2 + 1] == "S") count++;

            if (count == 3)
            {
                map[mass.Item1, mass.Item2] = Char;
                Debug.Log(Char + "を生成しました");
                break;
            }
        }
    }
}