using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ���@��@��p���ă}�b�v�𐶐�����
/// </summary>
public class DiggingMapGenerator : MapGeneratorBase
{
    /// <summary>�����@��J�n�n�_�ƂȂ���W�̃��X�g</summary>
    List<(int, int)> _startMasses = new List<(int, int)>();
    /// <summary>�S�[����ݒu������ƂȂ�}�X�̃��X�g</summary>
    List<(int, int)> _goalMasses = new List<(int, int)>();

    /// <summary>���ƍ����ɉ������}�b�v�𐶐����A������ɂ��ĕԂ�</summary>
    public override string GenerateRandomMap(int width, int height)
    {
        // �n���ꂽ���������Ȃ�-1���Ċ�ɒ���
        int w = width % 2 != 0 ? width : width - 1;
        int h = height % 2 != 0 ? height : height - 1;
        // �O����ʘH�A����ȊO��ǂɂ���
        string[,] map = new string[h, w];
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                map[i, j] = i * j == 0 || i == h - 1 || j == w - 1 ? "O" : "W";
        // �����@��
        _startMasses.Add((1, 1));
        DiggingPass(map, _startMasses);
        // �O����ǂɖ߂�
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
        GenerateGoal(map, "E");
        GenerateGoal(map, "P");

        return ArrayToString(map);
    }

    /// <summary>�ʘH���@��</summary>
    void DiggingPass(string[,] map, List<(int,int)> startMasses)
    {
        // �J�n���W�̃��X�g�̒����烉���_���Ɍ���
        int startIndex = Random.Range(0, startMasses.Count);
        // �J�n���W���Z�b�g
        int x = startMasses[startIndex].Item1;
        int y = startMasses[startIndex].Item2;
        // �J�n���W�̃��X�g����폜����
        startMasses.RemoveAt(startIndex);

        while (true)
        {
            // �@��i�߂�����𕶎���Ŋi�[���郊�X�g
            List<string> dirs = new List<string>();
            // �㉺���E�A2�}�X��܂ŕǂ��ǂ������ׂ�
            if (map[x, y - 1] == "W" && map[x, y - 2] == "W")
                dirs.Add("Up");
            if (map[x, y + 1] == "W" && map[x, y + 2] == "W")
                dirs.Add("Down");
            if (map[x - 1, y] == "W" && map[x - 2, y] == "W")
                dirs.Add("Left");
            if (map[x + 1, y] == "W" && map[x + 2, y] == "W")
                dirs.Add("Right");
            // �@���������Ȃ���΃��[�v�𔲂���
            if (dirs.Count == 0) break;
            // �J�n���W���@��
            map[x, y] = "O";
            // �@������������_���Ɍ��߂�
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

        // ���݂̍��W���S�[���̌��}�X�̃��X�g�ɒǉ�����
        _goalMasses.Add((x, y));

        // �����J�n���W�̃��X�g�̒��g������Ȃ炻������ʘH���@��
        if (startMasses.Count > 0)
            DiggingPass(map, startMasses);
    }

    /// <summary>�w�肳�ꂽ�}�X���@��</summary>
    void DiggingMass(string[,] map, int x, int y)
    {
        map[x, y] = "O";
        // �������̍��W��x,y���Ɋ�Ȃ�J�n���W�̃��X�g�ɒǉ�����
        if (x * y % 2 != 0)
            _startMasses.Add((x, y));
    }

    /// <summary>�����̃����_���ȉӏ��ɃS�[����ݒu����</summary>
    void GenerateGoal(string[,] map, string Char)
    {
        // �S�[�����̃}�X�̃��X�g�̒����珰�̃}�X��T��
        foreach ((int, int) mass in _goalMasses.Where(i => map[i.Item1,i.Item2] == "O"))
        {
            // 3�������ǂɂȂ��Ă���}�X��T��
            int count = 0;
            if (map[mass.Item1 - 1, mass.Item2] == "W") count++;
            if (map[mass.Item1 + 1, mass.Item2] == "W") count++;
            if (map[mass.Item1, mass.Item2 - 1] == "W") count++;
            if (map[mass.Item1, mass.Item2 + 1] == "W") count++;

            if (count == 3)
            {
                map[mass.Item1, mass.Item2] = Char;
                break;
            }
        }
    }
}