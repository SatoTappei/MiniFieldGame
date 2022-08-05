using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �}�b�v�𐶐�����
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// ��������^�C���̃f�[�^
    /// </summary>
    [System.Serializable]
    public class Tile
    {
        public GameObject _prefab;
        public char _char;
    }

    /// <summary>��������^�C���̃f�[�^</summary>
    [SerializeField] Tile[] _tileDatas;
    /// <summary>���������^�C����o�^����e�I�u�W�F�N�g</summary>
    [SerializeField] Transform _tileParent;
    /// <summary>�����ɑΉ������^�C�����i�[���Ă��鎫���^</summary>
    Dictionary<char, Tile> _tileDic = new Dictionary<char, Tile>();
    /// <summary>���������^�C�����i�[����A��Ƀv���C���[��G��z�u����̂Ɏg��</summary>
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

    /// <summary>�����񂩂�}�b�v�𐶐�����</summary>
    public void GenerateMap(string mapStr)
    {
        // ���������񂸂ɕ�������
        string[] lines = mapStr.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                // �Ή����镶��������ΐ������āA���̃^�C���𐶐���̃^�C���̃��X�g�ɒǉ�����
                if (_tileDic.TryGetValue(lines[i][j], out Tile tile))
                {
                    var obj = Instantiate(tile._prefab, new Vector3(i, 0, j), Quaternion.identity);
                    _generatedTiles.Add(obj);
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
        List<GameObject> canMoveTiles = new List<GameObject>(_generatedTiles.Where(l => l.GetComponent<TileData>().Type == canMove));

        // �v���C���[�̈ʒu�������_���ȃ^�C���̏�ɐݒ�
        GameObject player = GameObject.FindWithTag("Player");
        int r = Random.Range(0, canMoveTiles.Count);
        player.transform.position = canMoveTiles[r].transform.position;
        player.GetComponent<PlayerManager>().InitPosXZ();
    }
}
