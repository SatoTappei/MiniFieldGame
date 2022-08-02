using UnityEngine;

public class S2_Field : MonoBehaviour
{
    public GameObject floor;
    public GameObject wall;

    S2_Array2D map;
    const float oneTile = 1.0f;
    const float floorSize = 10.0f / oneTile;

    //void Start()
    //{
    //    S2_Array2D mapdata = new S2_Array2D(10, 10);
    //    mapdata.Set(1, 1, 1);
    //    Create(mapdata);
    //}

    /// <summary>�O���b�h���W�����[���h���W�ɕϊ� ���[���h���W = �O���b�h���W * 2</summary>
    public static float ToWorldX(int xgrid) => xgrid * oneTile;
    public static float ToWorldZ(int zgrid) => zgrid * oneTile;

    /// <summary>���[���h���W���O���b�h���W�ɕϊ� �O���b�h���W = ���[���h���W / 2�@���ď����_�ȉ���؂�̂�</summary>
    public static int ToGridX(float xworld) => Mathf.FloorToInt(xworld / oneTile);
    public static int ToGridZ(float zworld) => Mathf.FloorToInt(zworld / oneTile);

    /// <summary>���}�b�v�f�[�^�̍쐬</summary>
    public void Create(S2_Array2D mapdata)
    {
        map = mapdata;
        float floorw = map.width / floorSize;
        float floorh = map.height / floorSize;
        floor.transform.localScale = new Vector3(floorw, 1, floorh);
        float floorx = (map.width - 1) / 2.0f * oneTile;
        float floorz = (map.height - 1) / 2.0f * oneTile;
        floor.transform.position = new Vector3(floorx, 0, floorz);
        for(int z = 0; z < map.height; z++)
        {
            for(int x = 0; x < map.width; x++)
            {
                if (map.Get(x, z) > 0)
                {
                    GameObject block = Instantiate(wall);
                    float xblock = ToWorldX(x);
                    float zblock = ToWorldZ(z);
                    block.transform.localScale = new Vector3(oneTile, 2, oneTile);
                    block.transform.position = new Vector3(xblock, 1, zblock);
                    block.transform.SetParent(floor.transform.GetChild(0));
                }
            }
        }
    }

    /// <summary>���������}�b�v�̃��Z�b�g</summary>
    public void MapReset()
    {
        Transform walls = floor.transform.GetChild(0);
        for (int i = 0; i < walls.childCount; i++)
        {
            Destroy(walls.GetChild(i).gameObject);
        }
    }

    /// <summary>�w�肵�����W���ǂ��ǂ������`�F�b�N</summary>
    public bool IsCollide(int xgrid, int zgrid)
    {
        return map.Get(xgrid, zgrid) != 0;
    }
}
