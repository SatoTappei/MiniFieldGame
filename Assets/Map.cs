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
    Enemy,
    Treasure,
    FoodTreasure,
    WeaponTreasure,
    Trap,
    FoodTrap,
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

    /// <summary>�}�b�v�̐����ɗp����</summary>
    Dictionary<MassType, MassData> MassDataDict { get; set; }
    /// <summary>�}�b�v�̐����ɗp����A���̕�������`����Ă��邩�𔻒�</summary>
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

                // �����ɑΉ�����MassData
                var massData = MapCharDict[ch];
                var mass = new Mass();
                var pos = CalcMapPos(i, Data.Count);
                // ����if���͌�X�C������
                if (massData.isCharacter)
                {
                    mass.existObject = Instantiate(massData.prefab, transform);
                    var mapObject = mass.existObject.GetComponent<MapObjectBase>();
                    mapObject.SetPosAndForward(new Vector2Int(i, Data.Count), Direction.South);

                    // �L�����N�^�[�̎��͓����ꏏ�ɍ쐬����
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
    public Vector3 CalcMapPos(Vector2Int pos) => CalcMapPos(pos.x, pos.y);

    public Vector2Int CalcDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Vector2Int.down;
            case Direction.South: return Vector2Int.up;
            case Direction.East: return Vector2Int.right;
            case Direction.West: return Vector2Int.left;
            default: throw new NotImplementedException();
        }
    }

    public (Mass mass, Vector2Int movedPos) GetMovePos(Vector2Int currentPos, Direction moveDir)
    {
        var offset = CalcDirection(moveDir);
        var movedPos = currentPos + offset;
        if (movedPos.x < 0 || movedPos.y < 0) return (null, currentPos);
        if (movedPos.y >= MapSize.y) return (null, currentPos);
        var line = Data[movedPos.y];
        if (movedPos.x >= line.Count) return (null, currentPos);

        var mass = line[movedPos.x];
        return (mass, movedPos);
    }

    /// <summary>�E���̎��̎��̕�����Ԃ�</summary>
    public static Direction TurnRightDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Direction.East;
            case Direction.South: return Direction.West;
            case Direction.East: return Direction.South;
            case Direction.West: return Direction.North;
            default: throw new NotImplementedException();
        }
    }

    /// <summary>�����̎��̎��̕�����Ԃ�</summary>
    public static Direction TurnLeftDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Direction.West;
            case Direction.South: return Direction.East;
            case Direction.East: return Direction.North;
            case Direction.West: return Direction.South;
            default: throw new NotImplementedException();
        }
    }
}
