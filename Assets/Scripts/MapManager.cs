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
        CharacterBase _onCharacter;
        ItemManager _onItem;

        public GameObject Prefab { get => _prefab; }
        public char Char { get => _char; }
        public TileType Type { get => _type; }
        /// <summary>このタイルにいるキャラクターを登録しておく</summary>
        public CharacterBase OnCharacter { get => _onCharacter; set => _onCharacter = value; }
        /// <summary>子のタイルに落ちているアイテムを登録しておく</summary>
        public ItemManager OnItem { get => _onItem; set => _onItem = value; }
    }

    /// <summary>生成するマップのデータ</summary>
    public class Map
    {
        /// <summary>マップをタイルの二次元配列で保持しておく</summary>
        public Tile[,] _mapArray;
        /// <summary>キャラやアイテムの配置に使う床のリスト</summary>
        public List<(int, int)> _floorList = new List<(int, int)>();

        public Map(int x, int z)
        {
            _mapArray = new Tile[x,z];
        }

        /// <summary>指定した座標のタイルの情報を取得する</summary>
        public Tile GetMapTile(int x, int z) => _mapArray[x, z];
        /// <summary>指定した座標にそのタイルにいるキャラクターの情報をセットする</summary>
        public void SetMapTileCharacter(int x, int z, CharacterBase ab) => _mapArray[x, z].OnCharacter = ab;
        /// <summary>指定した座標にそのタイルにあるアイテムの情報をセットする</summary>
        public void SetMapTileItem(int x, int z, ItemManager im) => _mapArray[x, z].OnItem = im;
        /// <summary>指定した座標のタイルにあるアイテムの情報を取得する</summary>
        public ItemManager GetMapTileItem(int x, int z) => _mapArray[x, z].OnItem;
        /// <summary>指定した座標のタイルにいるキャラクターの情報を取得する</summary>
        public CharacterBase GetMapTileActor(int x, int z)
        {
            try
            {
                return _mapArray[x, z].OnCharacter;
            }
            catch(System.IndexOutOfRangeException)
            {
                return null;
            }
        } /*=> _mapArray[x, z].OnCharacter;*/
    }

    /// <summary>生成するタイルのデータ</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>生成したタイルを登録する親オブジェクト</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>生成した敵を登録する親オブジェクト</summary>
    [SerializeField] Transform _enemyParent;
    /// <summary>生成したコインを登録する親オブジェクト</summary>
    [SerializeField] Transform _coinParent;
    /// <summary>タイルを隠す雲</summary>
    [SerializeField] GameObject _cloud;
    /// <summary>デバッグ用にRキーでシーンの再読み込みをさせるかどうか</summary>
    [SerializeField] bool isDebug;
    /// <summary>フロアに雲を生成するか</summary>
    bool _isCloudy;
    /// <summary>フロアに生成する生成する敵</summary>
    GameObject[] _enemies;
    /// <summary>フロアに生成する障害物</summary>
    //GameObject[] _obstacles;
    /// <summary>フロアに生成するコイン</summary>
    //GameObject[] _coins;
    /// <summary>文字に対応したタイルが格納してある辞書型</summary>
    Dictionary<char, Tile> _tileDic = new Dictionary<char, Tile>();
    /// <summary>生成したマップのデータ、マップやタイルを調べる際にはこれを参照する</summary>
    Map _currentMap;
    /// <summary>マップの元になる文字列</summary>
    string _mapStr;

    public Map CurrentMap { get => _currentMap; }
    public string MapStr { get => _mapStr; }

    /// <summary>ステージに残っている敵の数を返す</summary>
    public int RemainingEnemy() => _enemyParent.childCount;
    /// <summary>ステージに残っているコインの数を返す</summary>
    public int RemainingCoin() => _coinParent.childCount;

    void Awake()
    {
        _tileDatas.ToList().ForEach(l => _tileDic.Add(l.Char, l));
    }

    void Start()
    {

    }

    void Update()
    {
        // マップをリロードするテスト
        if (Input.GetKeyDown(KeyCode.R) && isDebug)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>ステージの初期化、開始時にPlayerSceneManagerから呼ばれる</summary>
    public void Init(StageDataSO so)
    {
        _enemies = so.Enemies;
        //_obstacles = so.Obstacles;
        //_coins = so.Coins;
        _isCloudy = so.IsCoudy;

        // マップのもとになる文字列を生成する
        _mapStr = so.MapGenerator.GenerateRandomMap(so.Width, so.Height);
        // 文字列からマップを生成する
        GenerateMap(_mapStr);
        // コインを生成して配置する
        //GenerateCoinRandom(so.MaxCoin);
        GenerateItemRandom(so.Coins, so.MaxCoin, _coinParent);
        GenerateItemRandom(so.Items, so.MaxItem);
        // プレイヤーを決められた位置(P)に配置する
        SetPlayerTile(GameObject.FindWithTag("Player"));
        // 敵を生成して配置する
        for (int i = 0; i < so.MaxEnemy; i++)
            GenerateCharacterRandom(_enemies, _enemyParent);
        // 障害物を生成して配置する
        //for (int i = 0; i < so.MaxObst; i++)
        //    GenerateCharacterRandom(_obstacles, _enemyParent);
    }

    /// <summary>マップをコピーして返す</summary>
    public string[,] GetMapCopy()
    {
        string[,] copy = new string[_currentMap._mapArray.GetLength(0), _currentMap._mapArray.GetLength(1)];
        for (int i = 0; i < copy.GetLength(0); i++)
            for (int j = 0; j < copy.GetLength(1); j++)
                copy[i, j] = _currentMap._mapArray[i, j].Char.ToString();
        return copy;
    }

    /// <summary>文字列からマップを生成する</summary>
    void GenerateMap(string mapStr)
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
                    if (tile.Type == TileType.Floor) _currentMap._floorList.Add((i, j)); // テスト
                    // タイルの上に雲も生成しておく
                    if(_isCloudy) Instantiate(_cloud, new Vector3(i, 2, j), Quaternion.identity);
                }
                else
                    Debug.LogWarning($"生成できませんでした。文字が登録されてないです。<{lines[i][j]}>");
            }
        }
    }

    /// <summary>プレイヤーを初期位置に配置する</summary>
    void SetPlayerTile(GameObject actor)
    {
        (int, int) tile = _currentMap._floorList
            .Find(t => _currentMap.GetMapTile(t.Item1, t.Item2).Char == 'P');

        actor.transform.position = new Vector3(tile.Item1, 0, tile.Item2);
        actor.GetComponent<CharacterBase>().InitPosXZ();
    }

    /// <summary>キャラクターをマップ上のランダムなタイルに配置する</summary>
    void SetCharacterRandom(GameObject actor, TileType canMove)
    {
        //// キャラクターを置く床のタイルのリストを作成
        //List<(int, int)> canMoveTiles = new List<(int, int)>();

        //for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
        //    for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
        //        if (_currentMap._mapArray[i, j].Type == canMove && _currentMap.GetMapTileActor(i,j) == null)
        //            canMoveTiles.Add((i, j));

        // 床のリストからキャラクターがいない床のリストを生成する
        List<(int, int)> canMoveTiles = new List<(int, int)>
            (_currentMap._floorList.Where(f=>_currentMap.GetMapTileActor(f.Item1,f.Item2) == null));

        // キャラクターの位置をランダムなタイルの上に設定
        int r = Random.Range(0, canMoveTiles.Count);
        actor.transform.position = new Vector3(canMoveTiles[r].Item1, 0, canMoveTiles[r].Item2);
        actor.GetComponent<CharacterBase>().InitPosXZ();
    }

    /// <summary>指定したタイルが移動可能か調べる</summary>
    public bool CheckCanMoveTile(int x, int z)
    {
        // 指定したタイルが壁なら移動不可能
        if (_currentMap.GetMapTile(x, z).Type == TileType.Wall)
            return false;
        // 指定したタイルにキャラクターが居たら移動不可能
        else if (CurrentMap.GetMapTileActor(x, z) != null)
            return false;
        else
            return true;
    }

    /// <summary>ランダムな位置にキャラクターを生成する</summary>
    void GenerateCharacterRandom(GameObject[] characters, Transform parent)
    {
        int r = Random.Range(0, characters.Length);
        var obj = Instantiate(characters[r], Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(parent);
        SetCharacterRandom(obj, TileType.Floor);
    }

    /// <summary>ランダムな位置に指定された数だけコインを生成する</summary>
    //void GenerateCoinRandom(int amount)
    //{
    //    // 床のタイルのリストを作成
    //    //List<(int, int)> floorTiles = new List<(int, int)>();

    //    //for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
    //    //    for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
    //    //        if (_currentMap._mapArray[i, j].Type == TileType.Floor)
    //    //            floorTiles.Add((i, j));

    //    // 床のタイルのリストを複製
    //    List<(int, int)> floorTiles = new List<(int, int)>(_currentMap._floorList);

    //    // 指定された数だけコインを生成、床のタイルの方が少ない場合はその数だけ生成する
    //    for (int i = 0; i < Mathf.Min(amount, floorTiles.Count); i++)
    //    {
    //        int r = Random.Range(0, floorTiles.Count);
    //        int x = floorTiles[r].Item1;
    //        int y = floorTiles[r].Item2;
    //        var obj = Instantiate(_coin, new Vector3(x, 0.5f, y), Quaternion.identity);
    //        obj.GetComponent<ItemManager>().InitPosXZ();
    //        obj.transform.SetParent(_coinParent);
    //        floorTiles.RemoveAt(r);
    //    }
    //}

    /// <summary>指定された数だけ渡されたアイテムを生成する</summary>
    void GenerateItemRandom(GameObject[] items, int amount, Transform parent = null)
    {
        // 床のタイルのリストを複製
        List<(int, int)> canSetTiles = new List<(int, int)>
            (_currentMap._floorList.Where(f=>_currentMap.GetMapTileItem(f.Item1,f.Item2) == null));
        // 指定された数だけ生成する
        for (int i = 0; i < Mathf.Min(amount, canSetTiles.Count); i++)
        {
            int itemRnd = Random.Range(0, items.Length);
            int posRnd = Random.Range(0, canSetTiles.Count);
            int x = canSetTiles[posRnd].Item1;
            int y = canSetTiles[posRnd].Item2;
            var obj = Instantiate(items[itemRnd], new Vector3(x, 0.5f, y), Quaternion.identity);
            obj.GetComponent<ItemManager>().InitPosXZ();
            if (parent != null)
                obj.transform.SetParent(parent);
            canSetTiles.RemoveAt(posRnd);
        }
    }
}
