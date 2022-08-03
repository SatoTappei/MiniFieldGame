using UnityEngine;

public class S2_Field : MonoBehaviour
{
    public GameObject floor;
    public GameObject wall;
    public GameObject line;
    public GameObject enemies;
    public S2_ActorMovement playerMovement;

    S2_Array2D map;
    const float oneTile = 1.0f;
    const float floorSize = 10.0f / oneTile;

    //void Start()
    //{
    //    S2_Array2D mapdata = new S2_Array2D(10, 10);
    //    mapdata.Set(1, 1, 1);
    //    Create(mapdata);
    //}

    /// <summary>グリッド座標をワールド座標に変換 ワールド座標 = グリッド座標 * 2</summary>
    public static float ToWorldX(int xgrid) => xgrid * oneTile;
    public static float ToWorldZ(int zgrid) => zgrid * oneTile;

    /// <summary>ワールド座標をグリッド座標に変換 グリッド座標 = ワールド座標 / 2　して小数点以下を切り捨て</summary>
    public static int ToGridX(float xworld) => Mathf.FloorToInt(xworld / oneTile);
    public static int ToGridZ(float zworld) => Mathf.FloorToInt(zworld / oneTile);

    /// <summary>★マップデータの作成</summary>
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
        ShowGridEffects();
    }

    /// <summary>生成したマップのリセット</summary>
    public void MapReset()
    {
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            Destroy(enemies.transform.GetChild(i).gameObject);
        }

        Transform walls = floor.transform.GetChild(0);
        for (int i = 0; i < walls.childCount; i++)
        {
            Destroy(walls.GetChild(i).gameObject);
        }
        Transform effects = floor.transform.GetChild(1);
        for(int i = 0; i < effects.childCount; i++)
        {
            Destroy(effects.GetChild(i).gameObject);
        }
    }

    /// <summary>指定した座標が壁かどうかをチェック</summary>
    public bool IsCollide(int xgrid, int zgrid)
    {
        if (map.Get(xgrid, zgrid) != 0) return true;
        if (xgrid == playerMovement._newGrid.x && zgrid == playerMovement._newGrid.z)
            return true;
        foreach(var enemyMovement in enemies.GetComponentsInChildren<S2_ActorMovement>())
        {
            if (xgrid == enemyMovement._newGrid.x && zgrid == enemyMovement._newGrid.z)
                return true;
        }
        return false;
    }

    /// <summary>マス目のエフェクトを表示する</summary>
    void ShowGridEffects()
    {
        for (int x = 1; x < map.width; x++)
        {
            GameObject obj = Instantiate(line, floor.transform.GetChild(1));
            obj.transform.position = new Vector3(x * oneTile - oneTile / 2, 0.1f, -oneTile / 2);
            obj.transform.localScale = new Vector3(1, 1, floorSize * oneTile);
        }
        for(int z = 1; z < map.height; z++)
        {
            GameObject obj = Instantiate(line, floor.transform.GetChild(1));
            obj.transform.position = new Vector3(-oneTile / 2, 0.1f, z * oneTile - oneTile / 2);
            obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            obj.transform.localScale = new Vector3(1, 1, floorSize * oneTile);
        }
    }

    /// <summary>マップデータを返す</summary>
    public S2_Array2D GetMapData()
    {
        S2_Array2D mapdata = new S2_Array2D(map.width, map.height);
        for (int z = 0; z < map.height; z++)
        {
            for(int x = 0; x < map.width; x++)
            {
                mapdata.Set(x, z, map.Get(x, z));
            }
        }
        return mapdata;
    }
}
