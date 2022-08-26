using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ă悵
/// </summary>
public class TileData : MonoBehaviour
{
	/// <summary>�����̃N���X</summary>
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

	/// <summary>���ʂ̍��W�̃N���X</summary>
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

		// �����܂ł̌��ʂ���x�z��ɔ��f����
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
		// ���̃��X�g�̏����l�Ƃ��ă}�b�v�S�̂�����
		rangeList.Add(new Range(0, 0, mapSizeX - 1, mapSizeY - 1));

		bool isDevided;
		do
		{
			// �c �� �� �̏��Ԃŕ�������؂��Ă����B�����؂�Ȃ�������I��
			isDevided = DevideRange(false);
			isDevided = DevideRange(true) || isDevided;

			// �������͍ő��搔�𒴂�����I��
			if (rangeList.Count >= maxRoom)
			{
				break;
			}
		} while (isDevided);

	}

	public bool DevideRange(bool isVertical)
	{
		bool isDevided = false;

		// ��悲�Ƃɐ؂邩�ǂ������肷��
		List<Range> newRangeList = new List<Range>();
		foreach (Range range in rangeList)
		{
			// ����ȏ㕪���ł��Ȃ��ꍇ�̓X�L�b�v
			if (isVertical && range.GetWidthY() < MINIMUM_RANGE_WIDTH * 2 + 1)
			{
				continue;
			}
			else if (!isVertical && range.GetWidthX() < MINIMUM_RANGE_WIDTH * 2 + 1)
			{
				continue;
			}

			System.Threading.Thread.Sleep(1);

			// 40���̊m���ŕ������Ȃ�
			// �������A���̐���1�̎��͕K����������
			if (rangeList.Count > 1 && RandomJadge(0.4f))
			{
				continue;
			}

			// ��������ŏ��̋��T�C�Y2���������A�c�肩�烉���_���ŕ����ʒu�����߂�
			int length = isVertical ? range.GetWidthY() : range.GetWidthX();
			int margin = length - MINIMUM_RANGE_WIDTH * 2;
			int baseIndex = isVertical ? range.Start.Y : range.Start.X;
			int devideIndex = baseIndex + MINIMUM_RANGE_WIDTH + GetRandomInt(1, margin) - 1;

			// �������ꂽ���̑傫����ύX���A�V��������ǉ����X�g�ɒǉ�����
			// �����ɁA�����������E��ʘH�Ƃ��ĕۑ����Ă���
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

			// �ǉ����X�g�ɐV��������ޔ�����B
			newRangeList.Add(newRange);

			isDevided = true;
		}

		// �ǉ����X�g�ɑޔ����Ă������V��������ǉ�����B
		rangeList.AddRange(newRangeList);

		return isDevided;
	}

	private void CreateRoom()
	{
		// �����̂Ȃ���悪�΂�Ȃ��悤�Ƀ��X�g���V���b�t������
		rangeList.Sort((a, b) => GetRandomInt(0, 1) - 1);

		// 1��悠����1����������Ă����B���Ȃ���������B
		foreach (Range range in rangeList)
		{
			System.Threading.Thread.Sleep(1);
			// 30���̊m���ŕ��������Ȃ�
			// �������A�ő啔�����̔����ɖ����Ȃ��ꍇ�͍��
			if (roomList.Count > maxRoom / 2 && RandomJadge(0.3f))
			{
				continue;
			}

			// �P�\���v�Z
			int marginX = range.GetWidthX() - MINIMUM_RANGE_WIDTH + 1;
			int marginY = range.GetWidthY() - MINIMUM_RANGE_WIDTH + 1;

			// �J�n�ʒu������
			int randomX = GetRandomInt(1, marginX);
			int randomY = GetRandomInt(1, marginY);

			// ���W���Z�o
			int startX = range.Start.X + randomX;
			int endX = range.End.X - GetRandomInt(0, (marginX - randomX)) - 1;
			int startY = range.Start.Y + randomY;
			int endY = range.End.Y - GetRandomInt(0, (marginY - randomY)) - 1;

			// �������X�g�֒ǉ�
			Range room = new Range(startX, startY, endX, endY);
			roomList.Add(room);

			// �ʘH�����
			CreatePass(range, room);
		}
	}

	private void CreatePass(Range range, Range room)
	{
		List<int> directionList = new List<int>();
		if (range.Start.X != 0)
		{
			// X�}�C�i�X����
			directionList.Add(0);
		}
		if (range.End.X != mapSizeX - 1)
		{
			// X�v���X����
			directionList.Add(1);
		}
		if (range.Start.Y != 0)
		{
			// Y�}�C�i�X����
			directionList.Add(2);
		}
		if (range.End.Y != mapSizeY - 1)
		{
			// Y�v���X����
			directionList.Add(3);
		}

		// �ʘH�̗L�����΂�Ȃ��悤�A���X�g���V���b�t������
		directionList.Sort((a, b) => GetRandomInt(0, 1) - 1);

		bool isFirst = true;
		foreach (int direction in directionList)
		{
			System.Threading.Thread.Sleep(1);
			// 80%�̊m���ŒʘH�����Ȃ�
			// �������A�܂��ʘH���Ȃ��ꍇ�͕K�����
			if (!isFirst && RandomJadge(0.8f))
			{
				continue;
			}
			else
			{
				isFirst = false;
			}

			// �����̔���
			int random;
			switch (direction)
			{
				case 0: // X�}�C�i�X����
					random = room.Start.Y + GetRandomInt(1, room.GetWidthY()) - 1;
					roomPassList.Add(new Range(range.Start.X, random, room.Start.X - 1, random));
					break;

				case 1: // X�v���X����
					random = room.Start.Y + GetRandomInt(1, room.GetWidthY()) - 1;
					roomPassList.Add(new Range(room.End.X + 1, random, range.End.X, random));
					break;

				case 2: // Y�}�C�i�X����
					random = room.Start.X + GetRandomInt(1, room.GetWidthX()) - 1;
					roomPassList.Add(new Range(random, range.Start.Y, random, room.Start.Y - 1));
					break;

				case 3: // Y�v���X����
					random = room.Start.X + GetRandomInt(1, room.GetWidthX()) - 1;
					roomPassList.Add(new Range(random, room.End.Y + 1, random, range.End.Y));
					break;
			}
		}

	}

	private void TrimPassList(ref int[,] map)
	{
		// �ǂ̕����ʘH������ڑ�����Ȃ������ʘH���폜����
		for (int i = passList.Count - 1; i >= 0; i--)
		{
			Range pass = passList[i];

			bool isVertical = pass.GetWidthY() > 1;

			// �ʘH�������ʘH����ڑ�����Ă��邩�`�F�b�N
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

			// �폜�ΏۂƂȂ����ʘH���폜����
			if (isTrimTarget)
			{
				passList.Remove(pass);

				// �}�b�v�z�񂩂���폜
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

		// �O���ɐڂ��Ă���ʘH��ʂ̒ʘH�Ƃ̐ڑ��_�܂ō폜����
		// �㉺�
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
		// ���E�
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

	///// <summary>�����_���Ń}�b�v�𐶐����ĕ�����ɂ��ĕԂ�</summary>
	//public string GenerateRandomMap(int width, int height)
	//{
	//    // �w�肳�ꂽ���ƍ����Ń}�b�v�̓񎟌��z����쐬����
	//    string[,] map = new string[width, height];
	//    for (int i = 0; i < height; i++)
	//        for (int j = 0; j < width; j++)
	//            map[i, j] = "W";

	//    // TODO:�}�b�v����敪�����鏈��
	//    map = SplitFloor(map);

	//    // �񎟌��z���string�^�ɂ��ĕԂ�
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

	///// <summary>���</summary>
	//struct Rect
	//{
	//    public int x; // �����X���W
	//    public int y; // �����Y���W
	//    public int width; // ��
	//    public int height; // ����
	//}

	//// �}�b�v�����ɕ������ĕ�����ŕԂ�
	//public string[,] SplitFloor(string[,] map)
	//{
	//    // �c�A�������͉���1~3��������
	//    // ���������邩
	//    //int splitNum = Random.Range(1, 4);
	//    // �c������
	//    //bool isVert = Random.Range(0, 2) == 0 ? true : false;
	//    // �[���ǁA�����̒�`��2*2�ȏ�

	//    // �������������i�[���郊�X�g
	//    List<Rect> rects = new List<Rect>();
	//    // �}�b�v�S�̂�1�̋��Ƃ���
	//    Rect rect;
	//    rect.x = 0;
	//    rect.y = 0;
	//    rect.width = map.GetLength(1);
	//    rect.height = map.GetLength(0);

	//    // ���������ɕ������邩�ǂ���
	//    bool isVert = true;

	//    while (rect.width * rect.height > 16)
	//    {
	//        Debug.Log($"����̍��W{rect.x}:{rect.y} ��{rect.width} ����{rect.height}");
	//        // ���߂�ꂽ���C����2��������
	//        int splitLine = Random.Range(4, (isVert ? rect.width : rect.height) - 4);
	//        // 2����������`���i�[���Ă���
	//        Rect rect1;
	//        Rect rect2;

	//        // �c�ɕ�������ꍇ
	//        if (isVert)
	//        {
	//            // ��������2�����ꂼ��Rect�Ɋi�[����
	//            rect1.x = rect.x;
	//            rect1.y = rect.y;
	//            rect1.width = Mathf.Abs(rect.x - splitLine);
	//            rect1.height = rect.height;
	//            rect2.x = splitLine + 1;
	//            rect2.y = rect.y;
	//            rect2.width = Mathf.Abs(splitLine - rect.width - 1);
	//            rect2.height = rect.height;
	//            Debug.Log("�c����");
	//        }
	//        // ���ɕ�������ꍇ
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
	//            Debug.Log("������");
	//        }

	//        // �������ق������X�g�Ɋi�[
	//        int area1 = rect1.width * rect1.height;
	//        int area2 = rect2.width * rect2.height;
	//        rects.Add(area1 < area2 ? rect1 : rect2);
	//        // �傫���ق������͕�������
	//        rect = area1 < area2 ? rect2 : rect1;

	//        // �c�Ɖ��̕�����؂�ւ���
	//        isVert = !isVert;

	//        // ������������\������e�X�g�A����Ȃ��Ȃ��������
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
	//        // �e�X�g�����܂�
	//    }
	//    return map;
	//}
}