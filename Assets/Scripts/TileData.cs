using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消してよし
/// </summary>
public class TileData : MonoBehaviour
{
	/// <summary>長さのクラス</summary>
	public class Range
	{

		public Position Start { get; set; }
		public Position End { get; set; }

		public int GetWidthX()
		{
			return End.X - Start.X + 1;
		}

		public int GetWidthY()
		{
			return End.Y - Start.Y + 1;
		}

		public Range(Position start, Position end)
		{
			Start = start;
			End = end;
		}

		public Range(int startX, int startY, int endX, int endY) : this(new Position(startX, startY), new Position(endX, endY)) { }

		public Range() : this(0, 0, 0, 0) { }

		public override string ToString()
		{
			return string.Format("{0} => {1}", Start, End);
		}

	}

	/// <summary>平面の座標のクラス</summary>
	public class Position
	{

		public int X { get; set; }
		public int Y { get; set; }

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Position() : this(0, 0) { }

		public override string ToString()
		{
			return string.Format("({0}, {1})", X, Y);
		}
	}

	public string GenerateRandomMap()
	{
		int[,] map = GenerateMap(16, 16, 6);

		string str = "";
		for (int i = 0; i < map.GetLength(0); i++)
		{
			for (int j = 0; j < map.GetLength(1); j++)
			{
				if (map[i, j] == 1) str += "O";
				else str += "W";
			}
			if (i != map.GetLength(0) - 1)
			{
				str += '\n';
			}
		}

		return str;
	}

	public static int GetRandomInt(int min, int max)
	{
		//return min + Mathf.FloorToInt(Random.value * (max - min + 1));
		return Random.Range(min, max + 2);
	}

	public static bool RandomJadge(float rate)
	{
		return Random.value < rate;
	}


	private const int MINIMUM_RANGE_WIDTH = 6;

	private int mapSizeX;
	private int mapSizeY;
	private int maxRoom;

	private List<Range> roomList = new List<Range>();
	private List<Range> rangeList = new List<Range>();
	private List<Range> passList = new List<Range>();
	private List<Range> roomPassList = new List<Range>();

	private bool isGenerated = false;

	public int[,] GenerateMap(int mapSizeX, int mapSizeY, int maxRoom)
	{
		this.mapSizeX = mapSizeX;
		this.mapSizeY = mapSizeY;

		int[,] map = new int[mapSizeX, mapSizeY];

		CreateRange(maxRoom);
		CreateRoom();

		// ここまでの結果を一度配列に反映する
		foreach (Range pass in passList)
		{
			for (int x = pass.Start.X; x <= pass.End.X; x++)
			{
				for (int y = pass.Start.Y; y <= pass.End.Y; y++)
				{
					map[x, y] = 1;
				}
			}
		}
		foreach (Range roomPass in roomPassList)
		{
			for (int x = roomPass.Start.X; x <= roomPass.End.X; x++)
			{
				for (int y = roomPass.Start.Y; y <= roomPass.End.Y; y++)
				{
					map[x, y] = 1;
				}
			}
		}
		foreach (Range room in roomList)
		{
			for (int x = room.Start.X; x <= room.End.X; x++)
			{
				for (int y = room.Start.Y; y <= room.End.Y; y++)
				{
					map[x, y] = 1;
				}
			}
		}

		TrimPassList(ref map);

		return map;
	}

	public void CreateRange(int maxRoom)
	{
		// 区画のリストの初期値としてマップ全体を入れる
		rangeList.Add(new Range(0, 0, mapSizeX - 1, mapSizeY - 1));

		bool isDevided;
		do
		{
			// 縦 → 横 の順番で部屋を区切っていく。一つも区切らなかったら終了
			isDevided = DevideRange(false);
			isDevided = DevideRange(true) || isDevided;

			// もしくは最大区画数を超えたら終了
			if (rangeList.Count >= maxRoom)
			{
				break;
			}
		} while (isDevided);

	}

	public bool DevideRange(bool isVertical)
	{
		bool isDevided = false;

		// 区画ごとに切るかどうか判定する
		List<Range> newRangeList = new List<Range>();
		foreach (Range range in rangeList)
		{
			// これ以上分割できない場合はスキップ
			if (isVertical && range.GetWidthY() < MINIMUM_RANGE_WIDTH * 2 + 1)
			{
				continue;
			}
			else if (!isVertical && range.GetWidthX() < MINIMUM_RANGE_WIDTH * 2 + 1)
			{
				continue;
			}

			System.Threading.Thread.Sleep(1);

			// 40％の確率で分割しない
			// ただし、区画の数が1つの時は必ず分割する
			if (rangeList.Count > 1 && RandomJadge(0.4f))
			{
				continue;
			}

			// 長さから最少の区画サイズ2つ分を引き、残りからランダムで分割位置を決める
			int length = isVertical ? range.GetWidthY() : range.GetWidthX();
			int margin = length - MINIMUM_RANGE_WIDTH * 2;
			int baseIndex = isVertical ? range.Start.Y : range.Start.X;
			int devideIndex = baseIndex + MINIMUM_RANGE_WIDTH + GetRandomInt(1, margin) - 1;

			// 分割された区画の大きさを変更し、新しい区画を追加リストに追加する
			// 同時に、分割した境界を通路として保存しておく
			Range newRange = new Range();
			if (isVertical)
			{
				passList.Add(new Range(range.Start.X, devideIndex, range.End.X, devideIndex));
				newRange = new Range(range.Start.X, devideIndex + 1, range.End.X, range.End.Y);
				range.End.Y = devideIndex - 1;
			}
			else
			{
				passList.Add(new Range(devideIndex, range.Start.Y, devideIndex, range.End.Y));
				newRange = new Range(devideIndex + 1, range.Start.Y, range.End.X, range.End.Y);
				range.End.X = devideIndex - 1;
			}

			// 追加リストに新しい区画を退避する。
			newRangeList.Add(newRange);

			isDevided = true;
		}

		// 追加リストに退避しておいた新しい区画を追加する。
		rangeList.AddRange(newRangeList);

		return isDevided;
	}

	private void CreateRoom()
	{
		// 部屋のない区画が偏らないようにリストをシャッフルする
		rangeList.Sort((a, b) => GetRandomInt(0, 1) - 1);

		// 1区画あたり1部屋を作っていく。作らない区画もあり。
		foreach (Range range in rangeList)
		{
			System.Threading.Thread.Sleep(1);
			// 30％の確率で部屋を作らない
			// ただし、最大部屋数の半分に満たない場合は作る
			if (roomList.Count > maxRoom / 2 && RandomJadge(0.3f))
			{
				continue;
			}

			// 猶予を計算
			int marginX = range.GetWidthX() - MINIMUM_RANGE_WIDTH + 1;
			int marginY = range.GetWidthY() - MINIMUM_RANGE_WIDTH + 1;

			// 開始位置を決定
			int randomX = GetRandomInt(1, marginX);
			int randomY = GetRandomInt(1, marginY);

			// 座標を算出
			int startX = range.Start.X + randomX;
			int endX = range.End.X - GetRandomInt(0, (marginX - randomX)) - 1;
			int startY = range.Start.Y + randomY;
			int endY = range.End.Y - GetRandomInt(0, (marginY - randomY)) - 1;

			// 部屋リストへ追加
			Range room = new Range(startX, startY, endX, endY);
			roomList.Add(room);

			// 通路を作る
			CreatePass(range, room);
		}
	}

	private void CreatePass(Range range, Range room)
	{
		List<int> directionList = new List<int>();
		if (range.Start.X != 0)
		{
			// Xマイナス方向
			directionList.Add(0);
		}
		if (range.End.X != mapSizeX - 1)
		{
			// Xプラス方向
			directionList.Add(1);
		}
		if (range.Start.Y != 0)
		{
			// Yマイナス方向
			directionList.Add(2);
		}
		if (range.End.Y != mapSizeY - 1)
		{
			// Yプラス方向
			directionList.Add(3);
		}

		// 通路の有無が偏らないよう、リストをシャッフルする
		directionList.Sort((a, b) => GetRandomInt(0, 1) - 1);

		bool isFirst = true;
		foreach (int direction in directionList)
		{
			System.Threading.Thread.Sleep(1);
			// 80%の確率で通路を作らない
			// ただし、まだ通路がない場合は必ず作る
			if (!isFirst && RandomJadge(0.8f))
			{
				continue;
			}
			else
			{
				isFirst = false;
			}

			// 向きの判定
			int random;
			switch (direction)
			{
				case 0: // Xマイナス方向
					random = room.Start.Y + GetRandomInt(1, room.GetWidthY()) - 1;
					roomPassList.Add(new Range(range.Start.X, random, room.Start.X - 1, random));
					break;

				case 1: // Xプラス方向
					random = room.Start.Y + GetRandomInt(1, room.GetWidthY()) - 1;
					roomPassList.Add(new Range(room.End.X + 1, random, range.End.X, random));
					break;

				case 2: // Yマイナス方向
					random = room.Start.X + GetRandomInt(1, room.GetWidthX()) - 1;
					roomPassList.Add(new Range(random, range.Start.Y, random, room.Start.Y - 1));
					break;

				case 3: // Yプラス方向
					random = room.Start.X + GetRandomInt(1, room.GetWidthX()) - 1;
					roomPassList.Add(new Range(random, room.End.Y + 1, random, range.End.Y));
					break;
			}
		}

	}

	private void TrimPassList(ref int[,] map)
	{
		// どの部屋通路からも接続されなかった通路を削除する
		for (int i = passList.Count - 1; i >= 0; i--)
		{
			Range pass = passList[i];

			bool isVertical = pass.GetWidthY() > 1;

			// 通路が部屋通路から接続されているかチェック
			bool isTrimTarget = true;
			if (isVertical)
			{
				int x = pass.Start.X;
				for (int y = pass.Start.Y; y <= pass.End.Y; y++)
				{
					if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
					{
						isTrimTarget = false;
						break;
					}
				}
			}
			else
			{
				int y = pass.Start.Y;
				for (int x = pass.Start.X; x <= pass.End.X; x++)
				{
					if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
					{
						isTrimTarget = false;
						break;
					}
				}
			}

			// 削除対象となった通路を削除する
			if (isTrimTarget)
			{
				passList.Remove(pass);

				// マップ配列からも削除
				if (isVertical)
				{
					int x = pass.Start.X;
					for (int y = pass.Start.Y; y <= pass.End.Y; y++)
					{
						map[x, y] = 0;
					}
				}
				else
				{
					int y = pass.Start.Y;
					for (int x = pass.Start.X; x <= pass.End.X; x++)
					{
						map[x, y] = 0;
					}
				}
			}
		}

		// 外周に接している通路を別の通路との接続点まで削除する
		// 上下基準
		for (int x = 0; x < mapSizeX - 1; x++)
		{
			if (map[x, 0] == 1)
			{
				for (int y = 0; y < mapSizeY; y++)
				{
					if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
					{
						break;
					}
					map[x, y] = 0;
				}
			}
			if (map[x, mapSizeY - 1] == 1)
			{
				for (int y = mapSizeY - 1; y >= 0; y--)
				{
					if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
					{
						break;
					}
					map[x, y] = 0;
				}
			}
		}
		// 左右基準
		for (int y = 0; y < mapSizeY - 1; y++)
		{
			if (map[0, y] == 1)
			{
				for (int x = 0; x < mapSizeY; x++)
				{
					if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
					{
						break;
					}
					map[x, y] = 0;
				}
			}
			if (map[mapSizeX - 1, y] == 1)
			{
				for (int x = mapSizeX - 1; x >= 0; x--)
				{
					if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
					{
						break;
					}
					map[x, y] = 0;
				}
			}
		}
	}

	//void Start()
	//{

	//}

	//void Update()
	//{

	//}

	///// <summary>ランダムでマップを生成して文字列にして返す</summary>
	//public string GenerateRandomMap(int width, int height)
	//{
	//    // 指定された幅と高さでマップの二次元配列を作成する
	//    string[,] map = new string[width, height];
	//    for (int i = 0; i < height; i++)
	//        for (int j = 0; j < width; j++)
	//            map[i, j] = "W";

	//    // TODO:マップを区域分割する処理
	//    map = SplitFloor(map);

	//    // 二次元配列をstring型にして返す
	//    string str = "";
	//    for (int i = 0; i < height; i++)
	//    {
	//        for (int j = 0; j < width; j++)
	//            str += map[i, j];
	//        if (i != height - 1)
	//            str += '\n';
	//    }
	//    return str;
	//}

	///// <summary>区域</summary>
	//struct Rect
	//{
	//    public int x; // 左上のX座標
	//    public int y; // 左上のY座標
	//    public int width; // 幅
	//    public int height; // 高さ
	//}

	//// マップを区域に分割して文字列で返す
	//public string[,] SplitFloor(string[,] map)
	//{
	//    // 縦、もしくは横に1~3等分する
	//    // 何等分するか
	//    //int splitNum = Random.Range(1, 4);
	//    // 縦か横か
	//    //bool isVert = Random.Range(0, 2) == 0 ? true : false;
	//    // 端が壁、部屋の定義は2*2以上

	//    // 分割した区域を格納するリスト
	//    List<Rect> rects = new List<Rect>();
	//    // マップ全体を1つの区域とする
	//    Rect rect;
	//    rect.x = 0;
	//    rect.y = 0;
	//    rect.width = map.GetLength(1);
	//    rect.height = map.GetLength(0);

	//    // 垂直方向に分割するかどうか
	//    bool isVert = true;

	//    while (rect.width * rect.height > 16)
	//    {
	//        Debug.Log($"左上の座標{rect.x}:{rect.y} 幅{rect.width} 高さ{rect.height}");
	//        // 決められたラインで2分割する
	//        int splitLine = Random.Range(4, (isVert ? rect.width : rect.height) - 4);
	//        // 2分割した矩形を格納しておく
	//        Rect rect1;
	//        Rect rect2;

	//        // 縦に分割する場合
	//        if (isVert)
	//        {
	//            // 分割した2つをそれぞれRectに格納する
	//            rect1.x = rect.x;
	//            rect1.y = rect.y;
	//            rect1.width = Mathf.Abs(rect.x - splitLine);
	//            rect1.height = rect.height;
	//            rect2.x = splitLine + 1;
	//            rect2.y = rect.y;
	//            rect2.width = Mathf.Abs(splitLine - rect.width - 1);
	//            rect2.height = rect.height;
	//            Debug.Log("縦分割");
	//        }
	//        // 横に分割する場合
	//        else
	//        {
	//            rect1.x = rect.x;
	//            rect1.y = rect.y;
	//            rect1.width = rect.width;
	//            rect1.height = Mathf.Abs(rect.y - splitLine);
	//            rect2.x = rect.x;
	//            rect2.y = splitLine + 1;
	//            rect2.width = rect.width;
	//            rect2.height = Mathf.Abs(splitLine - rect.height - 1);
	//            Debug.Log("横分割");
	//        }

	//        // 小さいほうをリストに格納
	//        int area1 = rect1.width * rect1.height;
	//        int area2 = rect2.width * rect2.height;
	//        rects.Add(area1 < area2 ? rect1 : rect2);
	//        // 大きいほうを次は分割する
	//        rect = area1 < area2 ? rect2 : rect1;

	//        // 縦と横の分割を切り替える
	//        isVert = !isVert;

	//        // 分割した区画を表示するテスト、いらなくなったら消す
	//        if (area1 < area2)
	//        {
	//            for (int i = rect1.y; i < rect1.height; i++)
	//                for (int j = rect1.x; j < rect1.width; j++)
	//                    map[i, j] = "O";
	//        }
	//        else
	//        {
	//            for (int i = rect2.y; i < rect2.height; i++)
	//                for (int j = rect2.x; j < rect2.x + (rect2.width - 1); j++)
	//                    map[i, j] = "O";
	//        }
	//        // テストここまで
	//    }
	//    return map;
	//}
}