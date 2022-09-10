using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ��敪���@��p���ă}�b�v�𐶐�����
/// </summary>
public class AreaDivisionMapGenerator : MapGeneratorBase
{
    /// <summary>�����ƒʘH�������N���X</summary>
    public class Area
    {
        /// <summary>�n�_�ƂȂ���W</summary>
        public Position Start { get; set; }
        /// <summary>�I�_�ɂȂ���W</summary>
        public Position Goal { get; set; }

        /// <summary>�G���A�̕���Ԃ�</summary>
        public int GetWidth() => Goal.X - Start.X + 1;
        /// <summary>�G���A�̍�����Ԃ�</summary>
        public int GetHeight() => Goal.Y - Start.Y + 1;

        public Area(int sX, int sY, int gX, int gY)
        {
            Start = new Position(sX, sY);
            Goal = new Position(gX, gY);
        }

        // ��ƂȂ���W��\�����߂̃N���X
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

    /// <summary>�����̕��ƍ����̍ŏ��l</summary>
    const int OneSideMin = 5;
    /// <summary>�}�b�v�̕�</summary>
    int _mapWidth;
    /// <summary>�}�b�v�̍���</summary>
    int _mapHeight;
    /// <summary>�����̍ő吔</summary>
    int _roomNumMax;
    /// <summary>�}�b�v�𕪊����Ăł��������̃��X�g</summary>
    List<Area> _rooms = new List<Area>();
    /// <summary>�}�b�v�𕪊����Ăł����ʘH�̃��X�g</summary>
    List<Area> _passes = new List<Area>();
    /// <summary>�}�b�v�𕪊�������̋��̃��X�g</summary>
    List<Area> _areas = new List<Area>();

    /// <summary>���ƍ����ɉ������}�b�v�𐶐����A������ɂ��ĕԂ�</summary>
    public override string GenerateRandomMap(int width, int height)
    {
        _mapWidth = width;
        _mapHeight = height;
        // �����̍ő吔��ݒ�(16*16�̃}�b�v�ōő�6��������ɐݒ�)
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
        // ���X�g�̒��g��S��Clear����(R�L�[�Ń}�b�v���Đ�������ƃo�O��)
        _rooms.Clear();
        _passes.Clear();
        _areas.Clear();
        return ArrayToString(map);
    }

    /// <summary>���X�g�̒��g�ɑΉ�����ӏ���C�ӂ̕����ɒu��������</summary>
    void ReplaceTileChar(string[,] map, List<Area> list, string tile)
    {
        foreach (Area area in list)
            for (int x = area.Start.X; x <= area.Goal.X; x++)
                for (int y = area.Start.Y; y <= area.Goal.Y; y++)
                    map[x, y] = tile;
    }

    /// <summary>�}�b�v�𕪊�����</summary>
    void Devide()
    {
        // �}�b�v�S�̂���惊�X�g�ɒǉ�����
        _areas.Add(new Area(0, 0, _mapWidth - 1, _mapHeight - 1));
        // �����ɕ������邩
        bool isVert = true;
        // �������̍ő吔���A���������s����B
        for (int i = 0; i < _roomNumMax; i++)
        {
            // �ŏ���1��͕K����������邪����ȍ~��66���̊m���ŋ��𕪊�����
            if (i == 0 || Random.value <= 0.66f)
            {
                DevideArea(isVert);
                isVert = !isVert;
            }
        }
    }

    /// <summary>�w�肳�ꂽ�����ɋ��Ƃ��ĕ�������</summary>
    /// <param name="isVertical">�����ɕ������邩</param>
    void DevideArea(bool isVert)
    {
        // �������������ꎞ�I�Ɋi�[���Ă������X�g
        List<Area> devideds = new List<Area>();
        // ��惊�X�g�̒��g�𕪊����Ă���
        foreach (var area in _areas)
        {
            // �ӂ̒�������1�ӂ̍ŏ��̒�����2�{���������������]��
            int space = (isVert ? area.GetHeight() : area.GetWidth()) - OneSideMin * 2;
            // ���̒[����1�ӂ̍ŏ��̒����Ƀ����_���ŗ]���𑫂����ӏ��ŕ�������
            int devide = (isVert ? area.Start.Y : area.Start.X) + OneSideMin + Random.Range(0, space);

            // �����ɕ�������(�������㉺�ɕ������)�ꍇ�͋��̍������������Ă�2���ɕ�������Ε�������
            if (isVert && area.GetHeight() >= OneSideMin * 2 + 1)
            {
                // �����������E����ʘH�Ƃ��ĕۑ����Ă���
                _passes.Add(new Area(area.Start.X, devide, area.Goal.X, devide));
                // �������ďo�������̋����ꎞ�I�Ɋi�[���Ă���
                devideds.Add(new Area(area.Start.X, devide + 1, area.Goal.X, area.Goal.Y));
                // �����������̋��̍��������E����1�}�X��܂ŏk�߂�
                area.Goal.Y = devide - 1;
            }
            // �����ɕ�������(���������E�ɕ������)�ꍇ�͋��̕����������Ă�2���ɕ�������Ε�������
            else if(!isVert && area.GetWidth() >= OneSideMin * 2 + 1)
            {
                // �����������E����ʘH�Ƃ��ĕۑ����Ă���
                _passes.Add(new Area(devide, area.Start.Y, devide, area.Goal.Y));
                // �������ďo�������̋����ꎞ�I�Ɋi�[���Ă���
                devideds.Add(new Area(devide + 1, area.Start.Y, area.Goal.X, area.Goal.Y));
                // �����������̋��̍��������E����1�}�X���܂ŏk�߂�
                area.Goal.X = devide - 1;
            }
        }

        // ���̃��X�g�ɕ������Ăł�������ǉ�����
        // �����Œǉ����Ȃ���foreach�̒��g���ς���Ă��܂��̂Œ���
        _areas.AddRange(devideds);
    }

    /// <summary>���ɕ����ƒʘH�𐶐�����</summary>
    void GenerateRoomAndPass()
    {
        //���X�g���V���b�t������
        _areas = _areas.OrderBy(a => System.Guid.NewGuid()).ToList();

        foreach (var area in _areas)
        {
            // �����̐����ő吔�̔����ȏ�̏ꍇ�A30���̊m���ŕ����𐶐����Ȃ�
            if (_rooms.Count > _roomNumMax / 2 && Random.value > 0.3f)
                continue;
            // �����𐶐��\�ȃX�y�[�X���v�Z
            // ���̕� - �����Ƃ��Đ��藧�ŏ��̕�
            int widthSpace = area.GetWidth() - OneSideMin;
            int heightSpace = area.GetHeight() - OneSideMin;
            // �����𐶐������_�ƂȂ���W���Z�o
            // ���̒[(�ǂ�����ׂ��ӏ�)����1�}�X �` �����\�ȃX�y�[�X ���烉���_��
            int startX = area.Start.X + Random.Range(1,widthSpace);
            int startY = area.Start.Y + Random.Range(1, heightSpace);
            // �����𐶐�����I�_�ƂȂ���W���Z�o
            // ���̒[(�ǂ�����ӏ�)����-1�}�X - �]�� ���烉���_��  
            int goalX = area.Goal.X - 1 - Random.Range(0, Random.Range(1,widthSpace));
            int goalY = area.Goal.Y - 1 - Random.Range(0, Random.Range(1,heightSpace));
            // �����̃��X�g�֒ǉ�
            Area room = new Area(startX, startY, goalX, goalY);
            _rooms.Add(room);
            // �ʘH����邽�߂�4�����̃��X�g�𐶐����ăV���b�t������
            List<string> dirs = new List<string>() { "Right", "Left", "Up", "Down" };
            dirs = dirs.OrderBy(d => System.Guid.NewGuid()).ToList();
            // �ʘH��1�{�ȏ㑶�݂��邩�B�ʘH��1�{����������true�ɂȂ�
            bool isExist = false;

            foreach (var d in dirs)
            {
                // �ŏ���1�{�ȊO�̒ʘH��66���̊m���Ő������Ȃ�
                if (isExist && Random.value < 0.66f) continue;

                int r;
                switch (d)
                {
                    // �����̉E������E���̕ǂɌ������ĒʘH�����
                    case "Right" when area.Goal.X < _mapWidth - 1:
                        r = room.Start.Y + Random.Range(0, room.GetHeight());
                        _passes.Add(new Area(room.Goal.X + 1, r, area.Goal.X, r));
                        isExist = true;
                        break;
                    // �����̕ǂ��畔���̍����ɂɌ������ĒʘH�����
                    case "Left" when area.Start.X > 0:
                        r = room.Start.Y + Random.Range(0, room.GetHeight());
                        _passes.Add(new Area(area.Start.X, r, room.Start.X - 1, r));
                        isExist = true;
                        break;
                    // �㑤�̕ǂ��畔���̏㑤�Ɍ������ĒʘH�����
                    case "Up" when area.Start.Y > 0:
                        r = room.Start.X + Random.Range(0, room.GetWidth());
                        _passes.Add(new Area(r, area.Start.Y, r, room.Start.Y - 1));
                        isExist = true;
                        break;
                    // �����̉������牺���̕ǂɌ������ĒʘH�����
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
    /// �ʘH���폜����
    /// �}�b�v�ɔ��f�����Ă���폜���Ȃ��ƁA���X�g����1�{�Ƃ��Ă��āA�ʘH1�}�X�ɑ΂���
    /// �S���̒ʘH�̊e�}�X���ׂ荇���Ă邩���ׂȂ��Ƃ����Ȃ��Ȃ�(�ʘH�̖{��*1�{�̃}�X��*�S���̒ʘH�̊e�}�X�̐�)
    /// </summary>
    void CutPass(string[,] map)
    {
        // �㉺����Ƃ��ă}�b�v�[�̕ǂ܂ŐL�тĂ���ʘH��ʂ̒ʘH�Ƃ̐ڑ��_�܂ō폜����
        // ���E�ɓ��̃}�X������������q�����Ă���Ƃ݂Ȃ��Ă��̒n�_�ō폜����߂�
        for (int x = 0; x < _mapWidth - 1; x++)
        {
            // �}�b�v�̏㑤�𒲂ׂ�
            if (map[x, 0] == "O")
                for (int y = 0; y < _mapHeight; y++)
                    if (Cut(x, y, isVert: true)) break;
            // �}�b�v�̉����𒲂ׂ�
            if (map[x, _mapHeight - 1] == "O")
                for (int y = _mapHeight - 1; y >= 0; y--)
                    if (Cut(x, y, isVert: true)) break;
        }
        // ���E����Ƃ��ă}�b�v�̒[�̕ǂ܂ŐL�тĂ���ʘH��ʂ̒ʘH�Ƃ̐ڑ��_�܂ō폜����
        // �㉺�ɓ��̃}�X������������q�����Ă���Ƃ݂Ȃ��Ă��̒n�_�ō폜����߂�
        for (int y = 0; y < _mapHeight - 1; y++)
        {
            // �}�b�v�̍����𒲂ׂ�
            if (map[0, y] == "O")
                for (int x = 0; x < _mapWidth; x++)
                    if (Cut(x, y, isVert: false)) break;
            // �}�b�v�̉E���𒲂ׂ�
            if (map[_mapWidth - 1, y] == "O")
                for (int x = _mapWidth - 1; x >= 0; x--)
                    if (Cut(x, y, isVert: false)) break;
        }

        // �w�肳�ꂽ�}�X�̍��E�܂��͏㉺�𒲂ׂ�
        // ���̒ʘH�ƌq�����Ă��Ȃ���΃}�X��ǂɂ��A�q�����Ă��邩�ǂ����Ԃ�
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

    /// <summary>�����̒��̕��ʂ̏��̃}�X�������_����1�}�X�擾����</summary>
    List<Area> GetRandomRoomMass(string[,] map)
    {
        // �����̃��X�g�������_���ŕ��ёւ���
        _rooms = _rooms.OrderBy(r => System.Guid.NewGuid()).ToList();
        // ���ꂼ��̕����ɑ΂��ĕ��ʂ̏������݂���̂����ׂ�
        foreach (Area room in _rooms)
        {
            // �����̒��̃����_���ȃ}�X���ő�100�񒲂ׂ�
            for (int i = 0; i < 100; i++)
            {
                int x = Random.Range(room.Start.X, room.Goal.X);
                int y = Random.Range(room.Start.Y, room.Goal.Y);
                // ���̃}�X�������炻�̃}�X��Ԃ�
                if (map[x, y] == "O")
                    return new List<Area>() { new Area(x, y, x, y) };
            }
        }

        Debug.LogError("���̃^�C�����擾�ł��܂���ł����B���\�b�h���C������K�v������܂��B");
        return null;

        // �d���Ȃ��Œ��ׂ��邪���������G
        //foreach (Area room in _rooms)
        //{
        //    // �����̉��̒��������������X�g
        //    List<int> widthList = new List<int>();
        //    for (int i = room.Start.X; i < room.Goal.X; i++)
        //    {
        //        widthList.Add(i);
        //    }
        //    // �����̏c�̒��������������X�g
        //    List<int> heightList = new List<int>();
        //    for (int i = room.Start.Y; i < room.Goal.Y; i++)
        //    {
        //        heightList.Add(i);
        //    }
        //    // ���̃��X�g����1�I��
        //    int wr = Random.Range(0, widthList.Count);
        //    // �c�̃��X�g���Ȃ��Ȃ�܂ő���
        //    while (heightList.Count > 0)
        //    {
        //        // �c�̃��X�g���珉���ʒu��I��
        //        int hr = Random.Range(0, heightList.Count);
        //        // �w�肳�ꂽ�}�X�����ʂ̏����ǂ������ׂ�
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
