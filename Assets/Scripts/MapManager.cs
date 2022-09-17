using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>�L�����N�^�[�����̃}�X�ɐN���ł��邩�̔��������̂Ɏg��</summary>
public enum TileType
{
    Floor,
    Wall,
}

/// <summary>
/// �}�b�v�𐧌䂷��
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary>��������^�C���̃f�[�^</summary>
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
        /// <summary>���̃^�C���ɂ���L�����N�^�[��o�^���Ă���</summary>
        public CharacterBase OnCharacter { get => _onCharacter; set => _onCharacter = value; }
        /// <summary>�q�̃^�C���ɗ����Ă���A�C�e����o�^���Ă���</summary>
        public ItemManager OnItem { get => _onItem; set => _onItem = value; }
    }

    /// <summary>��������}�b�v�̃f�[�^</summary>
    public class Map
    {
        /// <summary>�}�b�v���^�C���̓񎟌��z��ŕێ����Ă���</summary>
        public Tile[,] _mapArray;
        /// <summary>�L������A�C�e���̔z�u�Ɏg�����̃��X�g</summary>
        public List<(int, int)> _floorList = new List<(int, int)>();

        public Map(int x, int z)
        {
            _mapArray = new Tile[x,z];
        }

        /// <summary>�w�肵�����W�̃^�C���̏����擾����</summary>
        public Tile GetMapTile(int x, int z) => _mapArray[x, z];
        /// <summary>�w�肵�����W�ɂ��̃^�C���ɂ���L�����N�^�[�̏����Z�b�g����</summary>
        public void SetMapTileCharacter(int x, int z, CharacterBase ab) => _mapArray[x, z].OnCharacter = ab;
        /// <summary>�w�肵�����W�ɂ��̃^�C���ɂ���A�C�e���̏����Z�b�g����</summary>
        public void SetMapTileItem(int x, int z, ItemManager im) => _mapArray[x, z].OnItem = im;
        /// <summary>�w�肵�����W�̃^�C���ɂ���A�C�e���̏����擾����</summary>
        public ItemManager GetMapTileItem(int x, int z) => _mapArray[x, z].OnItem;
        /// <summary>�w�肵�����W�̃^�C���ɂ���L�����N�^�[�̏����擾����</summary>
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

    /// <summary>��������^�C���̃f�[�^</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>���������^�C����o�^����e�I�u�W�F�N�g</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>���������G��o�^����e�I�u�W�F�N�g</summary>
    [SerializeField] Transform _enemyParent;
    /// <summary>���������R�C����o�^����e�I�u�W�F�N�g</summary>
    [SerializeField] Transform _coinParent;
    /// <summary>�^�C�����B���_</summary>
    [SerializeField] GameObject _cloud;
    /// <summary>�f�o�b�O�p��R�L�[�ŃV�[���̍ēǂݍ��݂������邩�ǂ���</summary>
    [SerializeField] bool isDebug;
    /// <summary>�t���A�ɉ_�𐶐����邩</summary>
    bool _isCloudy;
    /// <summary>�t���A�ɐ������鐶������G</summary>
    GameObject[] _enemies;
    /// <summary>�t���A�ɐ��������Q��</summary>
    //GameObject[] _obstacles;
    /// <summary>�t���A�ɐ�������R�C��</summary>
    //GameObject[] _coins;
    /// <summary>�����ɑΉ������^�C�����i�[���Ă��鎫���^</summary>
    Dictionary<char, Tile> _tileDic = new Dictionary<char, Tile>();
    /// <summary>���������}�b�v�̃f�[�^�A�}�b�v��^�C���𒲂ׂ�ۂɂ͂�����Q�Ƃ���</summary>
    Map _currentMap;
    /// <summary>�}�b�v�̌��ɂȂ镶����</summary>
    string _mapStr;

    public Map CurrentMap { get => _currentMap; }
    public string MapStr { get => _mapStr; }

    /// <summary>�X�e�[�W�Ɏc���Ă���G�̐���Ԃ�</summary>
    public int RemainingEnemy() => _enemyParent.childCount;
    /// <summary>�X�e�[�W�Ɏc���Ă���R�C���̐���Ԃ�</summary>
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
        // �}�b�v�������[�h����e�X�g
        if (Input.GetKeyDown(KeyCode.R) && isDebug)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>�X�e�[�W�̏������A�J�n����PlayerSceneManager����Ă΂��</summary>
    public void Init(StageDataSO so)
    {
        _enemies = so.Enemies;
        //_obstacles = so.Obstacles;
        //_coins = so.Coins;
        _isCloudy = so.IsCoudy;

        // �}�b�v�̂��ƂɂȂ镶����𐶐�����
        _mapStr = so.MapGenerator.GenerateRandomMap(so.Width, so.Height);
        // �����񂩂�}�b�v�𐶐�����
        GenerateMap(_mapStr);
        // �R�C���𐶐����Ĕz�u����
        //GenerateCoinRandom(so.MaxCoin);
        GenerateItemRandom(so.Coins, so.MaxCoin, _coinParent);
        GenerateItemRandom(so.Items, so.MaxItem);
        // �v���C���[�����߂�ꂽ�ʒu(P)�ɔz�u����
        SetPlayerTile(GameObject.FindWithTag("Player"));
        // �G�𐶐����Ĕz�u����
        for (int i = 0; i < so.MaxEnemy; i++)
            GenerateCharacterRandom(_enemies, _enemyParent);
        // ��Q���𐶐����Ĕz�u����
        //for (int i = 0; i < so.MaxObst; i++)
        //    GenerateCharacterRandom(_obstacles, _enemyParent);
    }

    /// <summary>�}�b�v���R�s�[���ĕԂ�</summary>
    public string[,] GetMapCopy()
    {
        string[,] copy = new string[_currentMap._mapArray.GetLength(0), _currentMap._mapArray.GetLength(1)];
        for (int i = 0; i < copy.GetLength(0); i++)
            for (int j = 0; j < copy.GetLength(1); j++)
                copy[i, j] = _currentMap._mapArray[i, j].Char.ToString();
        return copy;
    }

    /// <summary>�����񂩂�}�b�v�𐶐�����</summary>
    void GenerateMap(string mapStr)
    {
        // ���������񂸂ɕ�������
        string[] lines = mapStr.Split('\n');
        // �}�b�v�̃f�[�^���������A�}�b�v�̕����s���ƂɈႤ�ꍇ(�W���O�z��)�ɂ̓G���[���o��̂Œ���
        _currentMap = new Map(lines[0].Length, lines.Length);

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                // �Ή����镶��������ΐ������āA���̃^�C���𐶐��ς݂̃}�b�v�f�[�^�ɓo�^����
                if (_tileDic.TryGetValue(lines[i][j], out Tile tile))
                {
                    var obj = Instantiate(tile.Prefab, new Vector3(i, 0, j), Quaternion.identity);
                    _currentMap._mapArray[i, j] = tile;
                    obj.transform.SetParent(_tileParent);
                    if (tile.Type == TileType.Floor) _currentMap._floorList.Add((i, j)); // �e�X�g
                    // �^�C���̏�ɉ_���������Ă���
                    if(_isCloudy) Instantiate(_cloud, new Vector3(i, 2, j), Quaternion.identity);
                }
                else
                    Debug.LogWarning($"�����ł��܂���ł����B�������o�^����ĂȂ��ł��B<{lines[i][j]}>");
            }
        }
    }

    /// <summary>�v���C���[�������ʒu�ɔz�u����</summary>
    void SetPlayerTile(GameObject actor)
    {
        (int, int) tile = _currentMap._floorList
            .Find(t => _currentMap.GetMapTile(t.Item1, t.Item2).Char == 'P');

        actor.transform.position = new Vector3(tile.Item1, 0, tile.Item2);
        actor.GetComponent<CharacterBase>().InitPosXZ();
    }

    /// <summary>�L�����N�^�[���}�b�v��̃����_���ȃ^�C���ɔz�u����</summary>
    void SetCharacterRandom(GameObject actor, TileType canMove)
    {
        //// �L�����N�^�[��u�����̃^�C���̃��X�g���쐬
        //List<(int, int)> canMoveTiles = new List<(int, int)>();

        //for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
        //    for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
        //        if (_currentMap._mapArray[i, j].Type == canMove && _currentMap.GetMapTileActor(i,j) == null)
        //            canMoveTiles.Add((i, j));

        // ���̃��X�g����L�����N�^�[�����Ȃ����̃��X�g�𐶐�����
        List<(int, int)> canMoveTiles = new List<(int, int)>
            (_currentMap._floorList.Where(f=>_currentMap.GetMapTileActor(f.Item1,f.Item2) == null));

        // �L�����N�^�[�̈ʒu�������_���ȃ^�C���̏�ɐݒ�
        int r = Random.Range(0, canMoveTiles.Count);
        actor.transform.position = new Vector3(canMoveTiles[r].Item1, 0, canMoveTiles[r].Item2);
        actor.GetComponent<CharacterBase>().InitPosXZ();
    }

    /// <summary>�w�肵���^�C�����ړ��\�����ׂ�</summary>
    public bool CheckCanMoveTile(int x, int z)
    {
        // �w�肵���^�C�����ǂȂ�ړ��s�\
        if (_currentMap.GetMapTile(x, z).Type == TileType.Wall)
            return false;
        // �w�肵���^�C���ɃL�����N�^�[��������ړ��s�\
        else if (CurrentMap.GetMapTileActor(x, z) != null)
            return false;
        else
            return true;
    }

    /// <summary>�����_���Ȉʒu�ɃL�����N�^�[�𐶐�����</summary>
    void GenerateCharacterRandom(GameObject[] characters, Transform parent)
    {
        int r = Random.Range(0, characters.Length);
        var obj = Instantiate(characters[r], Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(parent);
        SetCharacterRandom(obj, TileType.Floor);
    }

    /// <summary>�����_���Ȉʒu�Ɏw�肳�ꂽ�������R�C���𐶐�����</summary>
    //void GenerateCoinRandom(int amount)
    //{
    //    // ���̃^�C���̃��X�g���쐬
    //    //List<(int, int)> floorTiles = new List<(int, int)>();

    //    //for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
    //    //    for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
    //    //        if (_currentMap._mapArray[i, j].Type == TileType.Floor)
    //    //            floorTiles.Add((i, j));

    //    // ���̃^�C���̃��X�g�𕡐�
    //    List<(int, int)> floorTiles = new List<(int, int)>(_currentMap._floorList);

    //    // �w�肳�ꂽ�������R�C���𐶐��A���̃^�C���̕������Ȃ��ꍇ�͂��̐�������������
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

    /// <summary>�w�肳�ꂽ�������n���ꂽ�A�C�e���𐶐�����</summary>
    void GenerateItemRandom(GameObject[] items, int amount, Transform parent = null)
    {
        // ���̃^�C���̃��X�g�𕡐�
        List<(int, int)> canSetTiles = new List<(int, int)>
            (_currentMap._floorList.Where(f=>_currentMap.GetMapTileItem(f.Item1,f.Item2) == null));
        // �w�肳�ꂽ��������������
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
