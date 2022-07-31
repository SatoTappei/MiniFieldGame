using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// マップを生成する
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// 生成するタイルのデータ
    /// </summary>
    [Serializable]
    public class TileData
    {
        public GameObject _prefab;
        public char _char;
    }

    /// <summary>生成するタイルのデータ</summary>
    [SerializeField] TileData[] _tileDatas;
    /// <summary>文字に対応したタイルが格納してある辞書型</summary>
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

    /// <summary>テキストデータからマップを生成する</summary>
    public void GenerateMap(string mapStr)
    {
        // 文字列を一列ずつに分解する
        string[] lines = mapStr.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                // 対応する文字があれば生成する
                if (_tileDic.TryGetValue(lines[i][j], out GameObject tile))
                    Instantiate(tile, new Vector3(i, 0, j), Quaternion.identity);
                else
                    Debug.LogWarning("生成できませんでした。文字が登録されてないです。");
            }
        }
    }
}
