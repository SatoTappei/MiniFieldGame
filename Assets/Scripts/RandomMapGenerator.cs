using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区域分割法を用いてマップを生成する
/// </summary>
public class RandomMapGenerator : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>ランダムでマップを生成して文字列にして返す</summary>
    public string GenerateRandomMap(int width, int height)
    {
        // 指定された幅と高さでマップの二次元配列を作成する
        string[,] map = new string[width, height];
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                map[i, j] = "W";

        // TODO:マップを区域分割する処理
        map = SplitFloor(map);

        // 二次元配列をstring型にして返す
        string str = "";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
                str += map[i, j];
            if (i != height - 1)
                str += '\n';
        }
        return str;
    }

    /// <summary>区域</summary>
    struct Rect
    {
        public int x; // 左上のX座標
        public int y; // 左上のY座標
        public int width; // 幅
        public int height; // 高さ
    }

    // マップを区域に分割して文字列で返す
    public string[,] SplitFloor(string[,] map)
    {
        // 縦、もしくは横に1~3等分する
        // 何等分するか
        //int splitNum = Random.Range(1, 4);
        // 縦か横か
        //bool isVert = Random.Range(0, 2) == 0 ? true : false;
        // 端が壁、部屋の定義は2*2以上

        // 分割した区域を格納するリスト
        List<Rect> rects = new List<Rect>();
        // マップ全体を1つの区域とする
        Rect rect;
        rect.x = 0;
        rect.y = 0;
        rect.width = map.GetLength(1);
        rect.height = map.GetLength(0);

        // 垂直方向に分割するかどうか
        bool isVert = true;

        while (rect.width * rect.height > 16)
        {
            Debug.Log($"左上の座標{rect.x}:{rect.y} 幅{rect.width} 高さ{rect.height}");
            // 決められたラインで2分割する
            int splitLine = Random.Range(4, (isVert ? rect.width : rect.height) - 4);
            // 2分割した矩形を格納しておく
            Rect rect1;
            Rect rect2;

            // 縦に分割する場合
            if (isVert)
            {
                // 分割した2つをそれぞれRectに格納する
                rect1.x = rect.x;
                rect1.y = rect.y;
                rect1.width = Mathf.Abs(rect.x - splitLine);
                rect1.height = rect.height;
                rect2.x = splitLine + 1;
                rect2.y = rect.y;
                rect2.width = Mathf.Abs(splitLine - rect.width - 1);
                rect2.height = rect.height;
                Debug.Log("縦分割");
            }
            // 横に分割する場合
            else
            {
                rect1.x = rect.x;
                rect1.y = rect.y;
                rect1.width = rect.width;
                rect1.height = Mathf.Abs(rect.y - splitLine);
                rect2.x = rect.x;
                rect2.y = splitLine + 1;
                rect2.width = rect.width;
                rect2.height = Mathf.Abs(splitLine - rect.height - 1);
                Debug.Log("横分割");
            }

            // 小さいほうをリストに格納
            int area1 = rect1.width * rect1.height;
            int area2 = rect2.width * rect2.height;
            rects.Add(area1 < area2 ? rect1 : rect2);
            // 大きいほうを次は分割する
            rect = area1 < area2 ? rect2 : rect1;

            // 縦と横の分割を切り替える
            isVert = !isVert;

            // 分割した区画を表示するテスト、いらなくなったら消す
            if (area1 < area2)
            {
                for (int i = rect1.y; i < rect1.height; i++)
                    for (int j = rect1.x; j < rect1.width; j++)
                        map[i, j] = "O";
            }
            else
            {
                for (int i = rect2.y; i < rect2.height; i++)
                    for (int j = rect2.x; j < rect2.x + (rect2.width - 1); j++)
                        map[i, j] = "O";
            }
            // テストここまで
        }
        return map;
    }
}
