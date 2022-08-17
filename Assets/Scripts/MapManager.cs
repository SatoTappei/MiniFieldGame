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
        /// <summary>�}�b�v�𕶎���̓񎟌��z��ŕێ����Ă���</summary>
        public Tile[,] _mapArray;

        public Map(int x, int z)
        {
            _mapArray = new Tile[x,z];
        }

        /// <summary>�w�肵�����W�̃^�C���̏����擾����</summary>
        public Tile GetMapTile(int x, int z) => _mapArray[x, z];
        /// <summary>�w�肵�����W�ɂ��̃^�C���ɂ���L�����N�^�[�̏����Z�b�g����</summary>
        public void SetMapTileCharacter(int x, int z, CharacterBase ab) => _mapArray[x, z].OnCharacter = ab;
        /// <summary>�w�肵�����W�̃^�C���ɂ���L�����N�^�[�̏����擾����</summary>
        public CharacterBase GetMapTileActor(int x, int z) => _mapArray[x, z].OnCharacter;
        /// <summary>�w�肵�����W�ɂ��̃^�C���ɂ���A�C�e���̏����Z�b�g����</summary>
        public void SetMapTileItem(int x, int z, ItemManager im) => _mapArray[x, z].OnItem = im;
        /// <summary>�w�肵�����W�̃^�C���ɂ���A�C�e���̏����擾����</summary>
        public ItemManager GetMapTileItem(int x, int z) => _mapArray[x, z].OnItem;
    }

    /// <summary��������}�b�v�̕�����</summary>
    [TextArea(10, 10), SerializeField] string _mapStr;
    /// <summary>��������^�C���̃f�[�^</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>���������^�C����o�^����e�I�u�W�F�N�g</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>�t���A�ɐ������鐶������G</summary>
    [SerializeField] GameObject[] _generateEnemies;
    /// <summary>�t���A�ɐ�������R�C��</summary>
    [SerializeField] GameObject _coin;
    /// <summary>�����ɑΉ������^�C�����i�[���Ă��鎫���^</summary>
    Dictionary<char, Tile> _tileDic = new Dictionary<char, Tile>();
    /// <summary>���������}�b�v�̃f�[�^�A�}�b�v��^�C���𒲂ׂ�ۂɂ͂�����Q�Ƃ���</summary>
    Map _currentMap;

    public Map CurrentMap { get => _currentMap; }

    // �}�b�v
    // �񎟌��z��ɂ���
    // �w��̍��W�̃^�C�����擾�ł���悤�ɂ���

    void Awake()
    {
        _tileDatas.ToList().ForEach(l => _tileDic.Add(l.Char, l));
    }

    void Start()
    {
        GenerateMap(_mapStr);
        GenerateCoinRandom(15);
        SetCharacterRandom(GameObject.FindWithTag("Player"), TileType.Floor);

        //TODO: �}�b�v�������ɓG�𐶐�����e�X�g�A��X�ɂ�����Ƃ����֐��ɒ���
        for (int i = 0; i < 15; i++)
        {
            int r = Random.Range(0, _generateEnemies.Length);
            var obj = Instantiate(_generateEnemies[r], Vector3.zero, Quaternion.identity);
            SetCharacterRandom(obj, TileType.Floor);
        }

        // �G�^�[���J�n��
        //      �G�𐶐����邩�ǂ����`�F�b�N
        //      �G�𐶐�����
        // �G�^�[���I����
        //      �G�S�����s���������ǂ����`�F�b�N
        //      �G���S���s��������^�[���I��

        // �v���C���[�͕ǂ�ړ��s�̃}�X�ɂ͔z�u�ł��Ȃ� <= �L�����N�^�[���ɐN���s�\�̒n�`��enum�Őݒ肵�Ă��
        // �����ꏊ�ɂ͓G��z�u�ł��Ȃ��̂ŁA�}�X���ɏ�ɓG�������̓v���C���[�����邩�ǂ�������K�v������

    }

    void Update()
    {
        
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
                }
                else
                    Debug.LogWarning($"�����ł��܂���ł����B�������o�^����ĂȂ��ł��B<{lines[i][j]}>");
            }
        }
    }

    /// <summary>�L�����N�^�[���}�b�v��̃����_���ȃ^�C���ɔz�u����</summary>
    void SetCharacterRandom(GameObject actor, TileType canMove)
    {
        // �L�����N�^�[���ړ��\�ȃ^�C���̃��X�g���쐬
        List<(int, int)> canMoveTiles = new List<(int, int)>();

        for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
            for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
                if (_currentMap._mapArray[i, j].Type == canMove && _currentMap.GetMapTileActor(i,j) == null)
                    canMoveTiles.Add((i, j));

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

    /// <summary>�����_���Ȉʒu�Ɏw�肳�ꂽ�������R�C���𐶐�����</summary>
    void GenerateCoinRandom(int amount)
    {
        // ���̃^�C���̃��X�g���쐬
        List<(int, int)> floorTiles = new List<(int, int)>();

        for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
            for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
                if (_currentMap._mapArray[i, j].Type == TileType.Floor)
                    floorTiles.Add((i, j));

        // �w�肳�ꂽ�������R�C���𐶐��A���̃^�C���̕������Ȃ��ꍇ�͂��̐�������������
        for (int i = 0; i < Mathf.Min(amount, floorTiles.Count); i++)
        {
            int r = Random.Range(0, floorTiles.Count);
            var obj = Instantiate(_coin, new Vector3(floorTiles[r].Item1, 0.5f, floorTiles[r].Item2), Quaternion.identity);
            obj.GetComponent<ItemManager>().InitPosXZ();
            floorTiles.RemoveAt(r);
        }
    }
}