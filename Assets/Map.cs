using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// �}�b�v���쐬����
/// </summary>

public enum MassType
{
    Road,
    Wall,
    Player,
    Goal,
}

public enum Direction
{
    North,
    South,
    East,
    West,
}

[Serializable]
public class MassData
{
    public GameObject prefab;
    public MassType type;
    public char mapChar;
    public bool isRoad;
    public bool isCharacter;
}

public class Map : MonoBehaviour
{
    public class Mass
    {
        public MassType type;
        public GameObject massGameObject;
        public GameObject existObject;
    }

    public MassData[] _massDataList;
    public Vector2 _massOffset = Vector2.one;
    public bool IsNowBuilding { get; private set; }
    public Vector2Int MapSize { get; private set; }
    /// <summary>�}�b�v��̊J�n�ʒu</summary>
    public Vector2Int StartPos { get; set; }
    /// <summary>�v���C���[�̊J�n���̌���</summary>
    public Direction StartForward { get; set; }

    Dictionary<MassType, MassData> MassDataDict { get; set; }
    Dictionary<char, MassData> MapCharDict { get; set; }

    List<List<Mass>> Data { get; set; }
    public Mass this[int x, int y]
    {
        get => Data[y][x];
        set => Data[y][x] = value;
    }

    public MassData this[MassType type] 
    {
        get => MassDataDict[type];
    }

    /// <summary>�n���ꂽ������̃��X�g����}�b�v��Ԃ�</summary>
    public void BuildMap(List<string> map)
    {
        InitMassData();

        var mapSize = Vector2Int.zero;
        Data = new List<List<Mass>>();
        foreach(var line in map)
        {
            var lineData = new List<Mass>();
            for(int i= 0; i < line.Length; ++i)
            {
                var ch = line[i];
                if (!MapCharDict.ContainsKey(ch))
                {
                    Debug.LogWarning("�s���ȕ������}�b�v�f�[�^�ɑ��݂��Ă��܂��F" + ch);
                    // �ꉞ�A�n�߂̃f�[�^�ő�p����
                    ch = MapCharDict.First().Key;
                }

                var massData = MapCharDict[ch];
                var mass = new Mass();
                var pos = CalcMapPos(i, Data.Count);
                // ����if���͌�X�C������
                if (massData.isCharacter)
                {
                    massData = this[MassType.Road];
                }
                mass.type = massData.type;
                mass.massGameObject = Instantiate(massData.prefab, transform);
                mass.massGameObject.transform.position = pos;
                lineData.Add(mass);
            }
            Data.Add(lineData);

            // �}�b�v�T�C�Y�̐ݒ�
            mapSize.x = Mathf.Max(mapSize.x, line.Length);
            mapSize.y++;
        }
        MapSize = mapSize;
    }

    /// <summary>�}�X������������</summary>
    void InitMassData()
    {
        MassDataDict = new Dictionary<MassType, MassData>();
        MapCharDict = new Dictionary<char, MassData>();
        foreach(var massData in _massDataList)
        {
            MassDataDict.Add(massData.type, massData);
            MapCharDict.Add(massData.mapChar, massData);
        }
    }

    /// <summary>�}�b�v��ł̈ʒu���v�Z����</summary>
    public Vector3 CalcMapPos(int x, int y)
    {
        var pos = Vector3.zero;
        pos.x = x * _massOffset.x;
        pos.z = y * _massOffset.y * -1;
        return pos;
    }
}
