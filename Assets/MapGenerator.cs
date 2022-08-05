using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// マップを生成する
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// 生成するタイルのデータ
    /// </summary>
    [System.Serializable]
    public class Tile
    {
        public GameObject _prefab;
        public char _char;
    }

    /// <summary>生成するタイルのデータ</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>生成したタイルを登録する親オブジェクト</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>文字に対応したタイルが格納してある辞書型</summary>
    Dictionary<char, Tile> _tileDic = new Dictionary<char, Tile>();
    /// <summary>生成したタイルを格納する、上にプレイヤーや敵を配置するのに使う</summary>
    List<GameObject> _generatedTiles = new List<GameObject>();

    void Awake()
    {
        _tileDatas.ToList().ForEach(l => _tileDic.Add(l._char, l));
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>文字列からマップを生成する</summary>
    public void GenerateMap(string mapStr)
    {
        // 文字列を一列ずつに分解する
        string[] lines = mapStr.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                // 対応する文字があれば生成して、そのタイルを生成後のタイルのリストに追加する
                if (_tileDic.TryGetValue(lines[i][j], out Tile tile))
                {
                    var obj = Instantiate(tile._prefab, new Vector3(i, 0, j), Quaternion.identity);
                    _generatedTiles.Add(obj);
                    obj.transform.SetParent(_tileParent);
                }
                else
                    Debug.LogWarning("生成できませんでした。文字が登録されてないです。");
            }
        }
    }

    /// <summary>生成したマップにプレイヤーを配置する</summary>
    public void SetPlayer(TileType canMove)
    {
        // プレイヤーが移動可能なタイルのリストを作成
        List<GameObject> canMoveTiles = new List<GameObject>(_generatedTiles.Where(l => l.GetComponent<TileData>().Type == canMove));

        // プレイヤーの位置をランダムなタイルの上に設定
        GameObject player = GameObject.FindWithTag("Player");
        int r = Random.Range(0, canMoveTiles.Count);
        player.transform.position = canMoveTiles[r].transform.position;
        player.GetComponent<PlayerManager>().InitPosXZ();
    }
}
