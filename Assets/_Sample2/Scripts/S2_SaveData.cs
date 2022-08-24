using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class S2_SaveData
{
    public ActorSaveData playerData;
    public ActorSaveData[] enemyDatas;
    public MapSaveData mapData;
}

[System.Serializable]
public class ActorSaveData
{
    public Pos2D grid;
    public EDir direction;
    public S2_Params parameter;
}

[System.Serializable]
public class MapSaveData
{
    public S2_Array2D map;
}