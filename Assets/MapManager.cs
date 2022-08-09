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
        public GameObject _prefab;
        public char _char;
        public TileType _type;
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
        /// <summary>
        /// �w�肵�����W�ɕ������Z�b�g����A����}�b�v�̐������ɂ����g���Ă��Ȃ�
        /// </summary>
        public void SetMapTile(int x, int z, Tile s) => _mapArray[x, z] = s;
    }

    /// <summary>��������}�b�v�̕�</summary>
    const int MapWidth = 7;
    /// <summary>��������}�b�v�̍���</summary>
    const int MapHight = 7;

    /// <summary��������}�b�v�̕�����</summary>
    [TextArea(MapWidth, MapHight), SerializeField] string _mapStr;
    /// <summary>��������^�C���̃f�[�^</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>���������^�C����o�^����e�I�u�W�F�N�g</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>�����ɑΉ������^�C�����i�[���Ă��鎫���^</summary>
    Dictionary<char, Tile> _tileDic = new Dictionary<char, Tile>();
    /// <summary>���������}�b�v�̃f�[�^�A�}�b�v��^�C���𒲂ׂ�ۂɂ͂�����Q�Ƃ���</summary>
    Map _currentMap;
    //List<GameObject> _generatedTiles = new List<GameObject>();

    // �}�b�v
    // �񎟌��z��ɂ���
    // �w��̍��W�̃^�C�����擾�ł���悤�ɂ���

    void Awake()
    {
        _tileDatas.ToList().ForEach(l => _tileDic.Add(l._char, l));
    }

    void Start()
    {
        GenerateMap(_mapStr);
        SetPlayer(canMove: TileType.Floor);

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
                    var obj = Instantiate(tile._prefab, new Vector3(i, 0, j), Quaternion.identity);
                    _currentMap._mapArray[i, j] = tile;
                    obj.transform.SetParent(_tileParent);
                }
                else
                    Debug.LogWarning("�����ł��܂���ł����B�������o�^����ĂȂ��ł��B");
            }
        }
    }

    /// <summary>���������}�b�v�Ƀv���C���[��z�u����</summary>
    public void SetPlayer(TileType canMove)
    {
        // �v���C���[���ړ��\�ȃ^�C���̃��X�g���쐬
        List<(int, int)> canMoveTiles = new List<(int, int)>();

        for (int i = 0; i < _currentMap._mapArray.GetLength(0); i++)
            for (int j = 0; j < _currentMap._mapArray.GetLength(1); j++)
                if (_currentMap._mapArray[i, j]._type == canMove)
                    canMoveTiles.Add((i, j));

        // �v���C���[�̈ʒu�������_���ȃ^�C���̏�ɐݒ�
        GameObject player = GameObject.FindWithTag("Player");
        int r = Random.Range(0, canMoveTiles.Count);
        player.transform.position = new Vector3(canMoveTiles[r].Item1, 0, canMoveTiles[r].Item2);
        player.GetComponent<PlayerManager>().InitPosXZ();
    }
}
