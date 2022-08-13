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
        ActorBase _onActor;

        public GameObject Prefab { get => _prefab; }
        public char Char { get => _char; }
        public TileType Type { get => _type; }
        /// <summary>���̃^�C���ɂ���L�����N�^�[��o�^���Ă���</summary>
        public ActorBase OnActor { get => _onActor; set => _onActor = value; }
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

        /// <summary>
        /// �w�肵�����W�̕������擾����
        /// ���̃}�X�ɂ��łɃL�����N�^�[�����邩�ȂǁA��X�^�C���̃f�[�^���擾�ł���悤�ɂ���
        /// </summary>
        public Tile GetMapTile(int x, int z) => _mapArray[x, z];
        /// <summary>�w�肵�����W�ɕ������Z�b�g����A����}�b�v�̐������ɂ����g���Ă��Ȃ�</summary>
        //public void SetMapTile(int x, int z, Tile s) => _mapArray[x, z] = s;
        /// <summary>
        /// �w�肵�����W�ɂ��̃^�C���ɂ���L�����N�^�[�̏����Z�b�g����
        /// ���̍��W�ɉ���������U���ł�����A�ړ��ł��Ȃ��悤�ɂ��邽��
        /// </summary>
        public void SetMapTileActor(int x, int z, ActorBase ab) => _mapArray[x, z].OnActor = ab;
        /// <summary>
        /// �w�肵�����W�̃^�C���ɂ���L�����N�^�[�̏����擾����
        /// ���̍��W�ɉ���������U���ł�����A�ړ��ł��Ȃ��悤�ɂ��邽��
        /// </summary>
        public ActorBase GetMapTileActor(int x, int z) => _mapArray[x, z].OnActor;
    }

    /// <summary>��������}�b�v�̕�</summary>
    //const int MapWidth = 7;
    /// <summary>��������}�b�v�̍���</summary>
    //const int MapHight = 7;

    /// <summary��������}�b�v�̕�����</summary>
    [TextArea(10, 10), SerializeField] string _mapStr;
    /// <summary>��������^�C���̃f�[�^</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>���������^�C����o�^����e�I�u�W�F�N�g</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>�t���A�ɐ������鐶������G</summary>
    [SerializeField] GameObject[] _generateEnemies;
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
        SetActorRandom(GameObject.FindWithTag("Player"), TileType.Floor);

        //TODO: �}�b�v�������ɓG�𐶐�����e�X�g�A��X�ɂ�����Ƃ����֐��ɒ���
        for (int i = 0; i < 3; i++)
        {
            int r = Random.Range(0, _generateEnemies.Length);
            var obj = Instantiate(_generateEnemies[r], Vector3.zero, Quaternion.identity);
            SetActorRandom(obj, TileType.Floor);
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
    public void GenerateMap(string mapStr)
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
    public void SetActorRandom(GameObject actor, TileType canMove)
    {
        // �L�����N�^�[���ړ��\�ȃ^�C���̃��X�g���쐬
        List<(int, int)> canMoveTiles = new List<(int, int)>();

        for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
            for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
                if (_currentMap._mapArray[i, j].Type == canMove)
                    canMoveTiles.Add((i, j));

        // �L�����N�^�[�̈ʒu�������_���ȃ^�C���̏�ɐݒ�
        int r = Random.Range(0, canMoveTiles.Count);
        actor.transform.position = new Vector3(canMoveTiles[r].Item1, 0, canMoveTiles[r].Item2);
        actor.GetComponent<ActorBase>().InitPosXZ();
    }

    /// <summary>�w�肵���^�C�����ړ��\�����ׂ�</summary>
    public bool CheckCanMoveTile(int x, int z)
    {
        // ���̃L�����N�^�[������A�ǂ�����A���ƃA�E�g
        // ���̃L�����N�^�[������ = _onActor�ϐ���null�ȊO
        //Debug.Log(x.ToString() + " " + z.ToString());
        // �w�肵���^�C�����ǂȂ�ړ��s�\
        if (_currentMap.GetMapTile(x, z).Type == TileType.Wall)
            return false;
        // �w�肵���^�C���ɃL�����N�^�[��������ړ��s�\
        else if (CurrentMap.GetMapTileActor(x, z) != null)
            return false;
        else
            return true;
    }
}
