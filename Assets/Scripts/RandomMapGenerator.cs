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
        public int GetWidth() => Mathf.Abs(Goal.X - Start.X + 1);
        /// <summary>�G���A�̍�����Ԃ�</summary>
        public int GetHeight() => Mathf.Abs(Goal.Y - Start.Y + 1);

        public Area(int sX, int sY, int gX, int gY)
        {
            Start.X = sX;
            Start.Y = sY;
            Goal.X = gX;
            Goal.Y = gY;
        }

        // ��ƂȂ���W��\�����߂̃N���X
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
        // �����̍ő吔��ݒ�(16*16�̃}�b�v�ōő�6��������ɐݒ�)
        _roomNumMax = width * height / 42;

        string[,] map = new string[height, width];
        GenerateRoomAndPass(map);

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

    /// <summary>�����ƒʘH�𐶐�����</summary>
    void GenerateRoomAndPass(string[,] map)
    {
        // �}�b�v�S�̂���惊�X�g�ɒǉ�����
        _areas.Add(new Area(0, 0, _mapWidth - 1, _mapHeight - 1));
        // �����ɕ������邩
        bool isVert = true;
        // �������̍ő吔���A���������s����
        for (int i = 0; i < _roomNumMax; i++)
        {
            DevideMap(isVert);
            isVert = !isVert;
        }
    }

    /// <summary>�}�b�v�����ɕ�������</summary>
    /// <param name="isVertical">�����ɕ������邩</param>
    void DevideMap(bool isVert)
    {
        // �������������ꎞ�I�Ɋi�[���Ă������X�g
        List<Area> devideds = new List<Area>();
        // ��惊�X�g�̒��g�𕪊����Ă���
        foreach (var area in _areas)
        {
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
}
