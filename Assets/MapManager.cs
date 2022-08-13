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
        [SerializeField] GameObject _prefab;
        [SerializeField] char _char;
        [SerializeField] TileType _type;
        ActorBase _onActor;

        public GameObject Prefab { get => _prefab; }
        public char Char { get => _char; }
        public TileType Type { get => _type; }
        /// <summary>このタイルにいるキャラクターを登録しておく</summary>
        public ActorBase OnActor { get => _onActor; set => _onActor = value; }
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
        /// <summary>指定した座標に文字をセットする、現状マップの生成時にしか使っていない</summary>
        //public void SetMapTile(int x, int z, Tile s) => _mapArray[x, z] = s;
        /// <summary>
        /// 指定した座標にそのタイルにいるキャラクターの情報をセットする
        /// その座標に何かいたら攻撃できたり、移動できないようにするため
        /// </summary>
        public void SetMapTileActor(int x, int z, ActorBase ab) => _mapArray[x, z].OnActor = ab;
        /// <summary>
        /// 指定した座標のタイルにいるキャラクターの情報を取得する
        /// その座標に何かいたら攻撃できたり、移動できないようにするため
        /// </summary>
        public ActorBase GetMapTileActor(int x, int z) => _mapArray[x, z].OnActor;
    }

    /// <summary>生成するマップの幅</summary>
    //const int MapWidth = 7;
    /// <summary>生成するマップの高さ</summary>
    //const int MapHight = 7;

    /// <summary生成するマップの文字列</summary>
    [TextArea(10, 10), SerializeField] string _mapStr;
    /// <summary>生成するタイルのデータ</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>生成したタイルを登録する親オブジェクト</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>フロアに生成する生成する敵</summary>
    [SerializeField] GameObject[] _generateEnemies;
    /// <summary>文字に対応したタイルが格納してある辞書型</summary>
    Dictionary<char, Tile> _tileDic = new Dictionary<char, Tile>();
    /// <summary>生成したマップのデータ、マップやタイルを調べる際にはこれを参照する</summary>
    Map _currentMap;

    public Map CurrentMap { get => _currentMap; }

    // マップ
    // 二次元配列にする
    // 指定の座標のタイルを取得できるようにする

    void Awake()
    {
        _tileDatas.ToList().ForEach(l => _tileDic.Add(l.Char, l));
    }

    void Start()
    {
        GenerateMap(_mapStr);
        SetActorRandom(GameObject.FindWithTag("Player"), TileType.Floor);

        //TODO: マップ生成時に敵を生成するテスト、後々にきちんとした関数に直す
        for (int i = 0; i < 3; i++)
        {
            int r = Random.Range(0, _generateEnemies.Length);
            var obj = Instantiate(_generateEnemies[r], Vector3.zero, Quaternion.identity);
            SetActorRandom(obj, TileType.Floor);
        }

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
                    var obj = Instantiate(tile.Prefab, new Vector3(i, 0, j), Quaternion.identity);
                    _currentMap._mapArray[i, j] = tile;
                    obj.transform.SetParent(_tileParent);
                }
                else
                    Debug.LogWarning($"生成できませんでした。文字が登録されてないです。<{lines[i][j]}>");
            }
        }
    }

    /// <summary>キャラクターをマップ上のランダムなタイルに配置する</summary>
    public void SetActorRandom(GameObject actor, TileType canMove)
    {
        // キャラクターが移動可能なタイルのリストを作成
        List<(int, int)> canMoveTiles = new List<(int, int)>();

        for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
            for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
                if (_currentMap._mapArray[i, j].Type == canMove)
                    canMoveTiles.Add((i, j));

        // キャラクターの位置をランダムなタイルの上に設定
        int r = Random.Range(0, canMoveTiles.Count);
        actor.transform.position = new Vector3(canMoveTiles[r].Item1, 0, canMoveTiles[r].Item2);
        actor.GetComponent<ActorBase>().InitPosXZ();
    }

    /// <summary>指定したタイルが移動可能か調べる</summary>
    public bool CheckCanMoveTile(int x, int z)
    {
        // 他のキャラクターがいる、壁がある、だとアウト
        // 他のキャラクターがいる = _onActor変数がnull以外
        //Debug.Log(x.ToString() + " " + z.ToString());
        // 指定したタイルが壁なら移動不可能
        if (_currentMap.GetMapTile(x, z).Type == TileType.Wall)
            return false;
        // 指定したタイルにキャラクターが居たら移動不可能
        else if (CurrentMap.GetMapTileActor(x, z) != null)
            return false;
        else
            return true;
    }
}
