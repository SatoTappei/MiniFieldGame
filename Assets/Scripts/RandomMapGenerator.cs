using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��敪���@��p���ă}�b�v�𐶐�����
/// </summary>
public class RandomMapGenerator : MonoBehaviour
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
    const int OneSideMin = 6;
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
    /// <summary>�������牄�т�ʘH�̃��X�g</summary>
    List<Area> _roomPasses = new List<Area>();
    /// <summary>�}�b�v�𕪊�������̋��̃��X�g</summary>
    List<Area> _areas = new List<Area>();

    /// <summary>TODO:�v���؁A�����_���Ȓl��Ԃ�</summary>
    int GetRandomValue(int min, int max) => min + Mathf.FloorToInt(Random.value * (max - min + 1));

    /// <summary>���ƍ����ɉ������}�b�v�𐶐����A������ɂ��ĕԂ�</summary>
    public string GenerateRandomMap(int width, int height)
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

    /// <summary>�񎟌��z��𕶎���ɂ��ĕԂ�</summary>
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

    /// <summary>�}�b�v�𕪊�����</summary>
    void Devide()
    {
        // �}�b�v�S�̂���惊�X�g�ɒǉ�����
        _areas.Add(new Area(0, 0, _mapWidth - 1, _mapHeight - 1));
        // �����ɕ������邩
        bool isVert = true;
        // �������̍ő吔���A���������s����
        for (int i = 0; i < _roomNumMax; i++)
        {
            DevideArea(isVert);
            isVert = !isVert;
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
            // ���̐���2�ȏ�̏ꍇ��40���̊m���ŕ������Ȃ�
            if (_areas.Count > 1 && Random.value > 0.4f)
                continue;
            Debug.Log(area.GetWidth() + " " + area.GetHeight());
            // �����ɕ�������(�������㉺�ɕ������)�ꍇ�͋��̍������������Ă�2���ɕ�������Ε�������
            if (isVert && area.GetHeight() >= OneSideMin * 2 + 1)
            {
                // ��������1�ӂ̍ŏ��̒�����2�{���������l���]�T
                int space = area.GetHeight() - OneSideMin * 2;
                // ��������ʒu�������_���Ō��߂�
                int devidePos = area.Start.Y + OneSideMin + GetRandomValue(1, space) - 1;
                // �����������E����ʘH�Ƃ��ĕۑ����Ă���
                _passes.Add(new Area(area.Start.X, devidePos, area.Goal.X, devidePos));
                // �������ďo�������̋����ꎞ�I�Ɋi�[���Ă���
                devideds.Add(new Area(area.Start.X, devidePos + 1, area.Goal.X, area.Goal.Y));
                // �����������̋��̍��������E����1�}�X��܂ŏk�߂�
                area.Goal.Y = devidePos - 1;
            }
            // �����ɕ�������(���������E�ɕ������)�ꍇ�͋��̕����������Ă�2���ɕ�������Ε�������
            else if(!isVert && area.GetWidth() >= OneSideMin * 2 + 1)
            {
                // ������1�ӂ̍ŏ��̒�����2�{���������l���]�T
                int space = area.GetWidth() - OneSideMin * 2;
                // ��������ʒu�������_���Ō��߂�
                int devidePos = area.Start.X + OneSideMin + GetRandomValue(1, space) - 1;
                // �����������E����ʘH�Ƃ��ĕۑ����Ă���
                _passes.Add(new Area(devidePos, area.Start.Y, devidePos, area.Goal.Y));
                // �������ďo�������̋����ꎞ�I�Ɋi�[���Ă���
                devideds.Add(new Area(devidePos + 1, area.Start.Y, area.Goal.X, area.Goal.Y));
                // �����������̋��̍��������E����1�}�X���܂ŏk�߂�
                area.Goal.X = devidePos - 1;
            }
        }

        // ���̃��X�g�ɕ������Ăł�������ǉ�����
        // �����Œǉ����Ȃ���foreach�̒��g���ς���Ă��܂��̂Œ���
        _areas.AddRange(devideds);
    }

    /// <summary>���ɕ����𐶐�����</summary>
    void GenerateRoomInArea()
    {
        // ����H:�����̂Ȃ���悪�΂�Ȃ��悤�Ƀ��X�g���V���b�t������
        _areas.Sort((a, b) => GetRandomValue(0, 1) - 1);

        foreach (var area in _areas)
        {
            // �����̐����ő吔�̔����ȏ�̏ꍇ�A30���̊m���ŕ����𐶐����Ȃ�
            if (_rooms.Count > _roomNumMax / 2 && Random.value > 0.3f)
                continue;
            // �����𐶐����邽�߂̃X�y�[�X���v�Z
            int spaceX = area.GetWidth() - OneSideMin + 1;
            int spaceY = area.GetHeight() - OneSideMin + 1;
            // �����𐶐�������W�ɕ�����������
            int randomX = GetRandomValue(1, spaceX);
            int randomY = GetRandomValue(1, spaceY);
            // ���W���Z�o
            int startX = area.Start.X + randomX;
            int startY = area.Start.Y + randomY;
            int goalX = area.Goal.X - GetRandomValue(0, spaceX - randomX) - 1;
            int goalY = area.Goal.Y - GetRandomValue(0, spaceY - randomY) - 1;
            // �����̃��X�g�֒ǉ�
            Area room = new Area(startX, startY, goalX, goalY);
            _rooms.Add(room);
            // �ʘH�����
            GeneratePassFromRoom(area, room);
        }
    }

    /// <summary>��������ʘH�����</summary>
    void GeneratePassFromRoom(Area area, Area room)
    {
        // �e�[��������1�}�X�ł�����Ă���ΒʘH��L�΂����Ƃ��Ēǉ�
        List<string> dirs = new List<string>();
        if (area.Start.X > 0) 
            dirs.Add("Left");
        if (area.Goal.X < _mapWidth - 1)
            dirs.Add("Right");
        if (area.Start.Y > 0)
            dirs.Add("Up");
        if (area.Goal.Y < _mapHeight - 1)
            dirs.Add("Down");
        // ����H:�ʘH�̗L�����΂�Ȃ��悤�A���X�g���V���b�t������
        dirs.Sort((a, b) => GetRandomValue(0, 1) - 1);
        // ���̕�������n�߂ɐL�т��{���ǂ���
        bool isFirst = true;

        foreach (var dir in dirs)
        {
            // �ŏ���1�{�łȂ����80���̊m���ŒʘH������Ȃ�
            if (!isFirst && Random.value < 0.8f)
                continue;
            isFirst = false;

            // ���̒[���畔���܂ŐL�т�ʘH�����
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

    /// <summary>�ʘH���폜����</summary>
    void CutPass(ref string[,] map)
    {
        // �ǂ̕����̒ʘH������ڑ�����Ȃ������ʘH���폜����
        for (int i = _passes.Count - 1; i >= 0; i--)
        {
            // Area�^�̃C���X�^���X��ʘH�̃��X�g��i�Ԗڂŏ�����
            Area pass = _passes[i];
            // �ʘH�̍�����1���傫����Ώc�����̒ʘH�Ƃ݂Ȃ�
            bool isVert = pass.GetHeight() > 1;
            // �폜�̑ΏۂƂȂ邩�ǂ���
            bool isCut = true;

            if (isVert)
            {
                for (int y = pass.Start.Y; y <= pass.Goal.Y; y++)
                {
                    // �ʘH�̍��E�ɑ��̒ʘH������ΐڑ�����Ă���Ƃ݂Ȃ��A�폜�Ώۂɂ��Ȃ�
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
                    // �ʘH�̏㉺�ɑ��̒ʘH������ΐڑ�����Ă���Ƃ݂Ȃ��A�폜�Ώۂɂ��Ȃ�
                    if (map[x, pass.Start.Y - 1] == "O" || map[x, pass.Start.Y + 1] == "O")
                    {
                        isCut = false;
                        break;
                    }
                }
            }

            // �폜�ΏۂƂȂ����ʘH���폜����
            if (isCut)
            {
                _passes.Remove(pass);

                // �}�b�v�ォ��폜
                if (isVert)
                    for (int y = pass.Start.Y; y <= pass.Goal.Y; y++)
                        map[pass.Start.X, y] = "W";
                else
                    for (int x = pass.Start.X; x <= pass.Goal.X; x++)
                        map[x, pass.Start.Y] = "W";
            }
        }

        // �㉺����Ƃ��ă}�b�v�[�̕ǂ܂ŐL�тĂ���ʘH��ʂ̒ʘH�Ƃ̐ڑ��_�܂ō폜����
        for (int x = 0; x < _mapWidth - 1; x++)
        {
            // �}�b�v�̏㑤�𒲂ׂ�
            if (map[x, 0] == "O")
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    // ���E�ɓ��̃}�X������������q�����Ă���Ƃ݂Ȃ��Ă��̒n�_�ō폜����߂�
                    if (map[x - 1, y] == "O" || map[x + 1, y] == "O")
                        break;
                    map[x, y] = "W";
                }
            }
            // �}�b�v�̉����𒲂ׂ�
            if (map[x, _mapHeight - 1] == "O")
            {
                for (int y = _mapHeight - 1; y >= 0; y--)
                {
                    // ���E�ɓ��̃}�X������������q�����Ă���Ƃ݂Ȃ��Ă��̒n�_�ō폜����߂�
                    if (map[x - 1, y] == "O" || map[x + 1, y] == "O")
                        break;
                    map[x, y] = "W";
                }
            }
        }
        // ���E����Ƃ��ă}�b�v�̒[�̕ǂ܂ŐL�тĂ���ʘH��ʂ̒ʘH�Ƃ̐ڑ��_�܂ō폜����
        for (int y = 0; y < _mapHeight - 1; y++)
        {
            // �}�b�v�̍����𒲂ׂ�
            if (map[0, y] == "O")
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    // �㉺�ɓ��̃}�X������������q�����Ă���Ƃ݂Ȃ��Ă��̒n�_�ō폜����߂�
                    if (map[x, y - 1] == "O" || map[x, y + 1] == "O")
                        break;
                    map[x, y] = "W";
                }
            }
            // �}�b�v�̉E���𒲂ׂ�
            if (map[_mapWidth - 1, y] == "O")
            {
                for (int x = _mapWidth - 1; x >= 0; x--)
                {
                    // �㉺�ɓ��̃}�X������������q�����Ă���Ƃ݂Ȃ��Ă��̒n�_�ō폜����߂�
                    if (map[x, y - 1] == "O" || map[x, y + 1] == "O")
                        break;
                    map[x, y] = "W";
                }
            }
        }
    }
}
