using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>キャラクターがそのマスに侵入できるかの判定をするのに使う</summary>
public enum TileType
{
    Floor,
    Wall,
}

/// <summary>
/// マップを制御する
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary>生成するタイルのデータ</summary>
    [System.Serializable]
    public struct Tile
    {
        public GameObject _prefab;
        public char _char;
        public TileType _type;
    }

    /// <summary>生成するマップのデータ</summary>
    public class Map
    {
        /// <summary>マップを文字列の二次元配列で保持しておく</summary>
        public Tile[,] _mapArray;

        public Map(int x, int z)
        {
            _mapArray = new Tile[x,z];
        }

        /// <summary>
        /// 指定した座標の文字を取得する
        /// そのマスにすでにキャラクターがいるかなど、後々タイルのデータを取得できるようにする
        /// </summary>
        public Tile GetMapTile(int x, int z) => _mapArray[x, z];
        /// <summary>
        /// 指定した座標に文字をセットする、現状マップの生成時にしか使っていない
        /// </summary>
        public void SetMapTile(int x, int z, Tile s) => _mapArray[x, z] = s;
    }

    /// <summary>生成するマップの幅</summary>
    const int MapWidth = 7;
    /// <summary>生成するマップの高さ</summary>
    const int MapHight = 7;

    /// <summary生成するマップの文字列</summary>
    [TextArea(MapWidth, MapHight), SerializeField] string _mapStr;
    /// <summary>生成するタイルのデータ</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>生成したタイルを登録する親オブジェクト</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>文字に対応したタイルが格納してある辞書型</summary>
    Dictionary<char, Tile> _tileDic = new Dictionary<char, Tile>();
    /// <summary>生成したマップのデータ、マップやタイルを調べる際にはこれを参照する</summary>
    Map _currentMap;
    //List<GameObject> _generatedTiles = new List<GameObject>();

    // マップ
    // 二次元配列にする
    // 指定の座標のタイルを取得できるようにする

    void Awake()
    {
        _tileDatas.ToList().ForEach(l => _tileDic.Add(l._char, l));
    }

    void Start()
    {
        GenerateMap(_mapStr);
        SetPlayer(canMove: TileType.Floor);

        // 敵ターン開始時
        //      敵を生成するかどうかチェック
        //      敵を生成する
        // 敵ターン終了時
        //      敵全員が行動したかどうかチェック
        //      敵が全員行動したらターン終了

        // プレイヤーは壁や移動不可のマスには配置できない <= キャラクター毎に侵入不可能の地形をenumで設定してやる
        // 同じ場所には敵を配置できないので、マス毎に上に敵もしくはプレイヤーがいるかどうか見る必要がある

    }

    void Update()
    {
        
    }

    /// <summary>文字列からマップを生成する</summary>
    public void GenerateMap(string mapStr)
    {
        // 文字列を一列ずつに分解する
        string[] lines = mapStr.Split('\n');
        // マップのデータを初期化、マップの幅が行ごとに違う場合(ジャグ配列)にはエラーが出るので注意
        _currentMap = new Map(lines[0].Length, lines.Length);

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                // 対応する文字があれば生成して、そのタイルを生成済みのマップデータに登録する
                if (_tileDic.TryGetValue(lines[i][j], out Tile tile))
                {
                    var obj = Instantiate(tile._prefab, new Vector3(i, 0, j), Quaternion.identity);
                    _currentMap._mapArray[i, j] = tile;
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
        List<(int, int)> canMoveTiles = new List<(int, int)>();

        for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
            for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
                if (_currentMap._mapArray[i, j]._type == canMove)
                    canMoveTiles.Add((i, j));

        // プレイヤーの位置をランダムなタイルの上に設定
        GameObject player = GameObject.FindWithTag("Player");
        int r = Random.Range(0, canMoveTiles.Count);
        player.transform.position = new Vector3(canMoveTiles[r].Item1, 0, canMoveTiles[r].Item2);
        player.GetComponent<PlayerManager>().InitPosXZ();
    }
}
