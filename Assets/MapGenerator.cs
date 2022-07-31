using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// �}�b�v�𐶐�����
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// ��������^�C���̃f�[�^
    /// </summary>
    [Serializable]
    public class TileData
    {
        public GameObject _prefab;
        public char _char;
    }

    /// <summary>��������^�C���̃f�[�^</summary>
    [SerializeField] TileData[] _tileDatas;
    /// <summary>�����ɑΉ������^�C�����i�[���Ă��鎫���^</summary>
    Dictionary<char, GameObject> _tileDic = new Dictionary<char, GameObject>();

    void Awake()
    {
        _tileDatas.ToList().ForEach(l => _tileDic.Add(l._char, l._prefab));
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>�e�L�X�g�f�[�^����}�b�v�𐶐�����</summary>
    public void GenerateMap(string mapStr)
    {
        // ���������񂸂ɕ�������
        string[] lines = mapStr.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                // �Ή����镶��������ΐ�������
                if (_tileDic.TryGetValue(lines[i][j], out GameObject tile))
                    Instantiate(tile, new Vector3(i, 0, j), Quaternion.identity);
                else
                    Debug.LogWarning("�����ł��܂���ł����B�������o�^����ĂȂ��ł��B");
            }
        }
    }
}
