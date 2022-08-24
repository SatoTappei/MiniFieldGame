using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_SaveDataManager : MonoBehaviour
{
    public S2_Field field;
    public GameObject player;
    public GameObject enemies;
    S2_SaveData saveData;
    const string saveKey = "GameData";

    void Start()
    {
        saveData = new S2_SaveData();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
                S2_Message.add("セーブ");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
                S2_Message.add("ロード");
            }
        }
    }

    /// <summary>データを保存する</summary>
    void Save()
    {
        saveData.playerData = MakePlayerData();
        saveData.enemyDatas = MakeEnemyDatas();
        saveData.mapData = MakeMapData();
        PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(saveData));
    }

    /// <summary>データを読み込む</summary>
    void Load()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            var data = PlayerPrefs.GetString(saveKey);
            JsonUtility.FromJsonOverwrite(data, saveData);
            LoadMapData(saveData);
            LoadEnemyDatas(saveData);
            LoadPlayerData(saveData);
        }
    }

    /// <summary>アクターデータを作成、返す</summary>
    ActorSaveData MakeActorData(Transform actor)
    {
        S2_ActorMovement move = actor.GetComponent<S2_ActorMovement>();
        ActorSaveData actorSaveData = new ActorSaveData();
        actorSaveData.grid = new Pos2D();
        actorSaveData.grid.x = move._grid.x;
        actorSaveData.grid.z = move._grid.z;
        actorSaveData.direction = move._direction;
        actorSaveData.parameter = actor.GetComponent<S2_ActorParamsController>().GetParameter();
        return actorSaveData;
    }

    /// <summary>アクターデータをアクターに反映する</summary>
    void LoadActorData(ActorSaveData data, Transform actor)
    {
        S2_ActorMovement move = actor.GetComponent<S2_ActorMovement>();
        move.SetPosition(data.grid.x, data.grid.z);
        move.SetDirection(data.direction);
        actor.GetComponent<S2_ActorParamsController>().SetParameter(data.parameter);
    }

    /// <summary>プレイヤーデータを作成、返す</summary>
    ActorSaveData MakePlayerData()
    {
        return MakeActorData(player.transform);
    }

    /// <summary>プレイヤーデータを反映する</summary>
    void LoadPlayerData(S2_SaveData saveData)
    {
        LoadActorData(saveData.playerData, player.transform);
    }

    /// <summary>敵データを作成、返す</summary>
    ActorSaveData[] MakeEnemyDatas()
    {
        int enemyCount = enemies.transform.childCount;
        ActorSaveData[] enemySaveDatas = new ActorSaveData[enemyCount];
        for(int i = 0; i < enemyCount; i++)
        {
            Transform enemy = enemies.transform.GetChild(i);
            enemySaveDatas[i] = MakeActorData(enemy);
        }
        return enemySaveDatas;
    }

    /// <summary>敵データを反映する</summary>
    void LoadEnemyDatas(S2_SaveData saveData)
    {
        foreach (var data in saveData.enemyDatas)
        {
            GameObject enemyObj = (GameObject)Resources.Load("Prefabs/Enemy" + data.parameter.id);
            GameObject enemy = Instantiate(enemyObj, enemies.transform);
            LoadActorData(data, enemy.transform);
            enemy.GetComponent<S2_EnemyOperation>().target = player.GetComponent<S2_ActorMovement>();
        }
    }

    /// <summary>マップデータを作成、返す</summary>
    MapSaveData MakeMapData()
    {
        MapSaveData mapSaveData = new MapSaveData();
        mapSaveData.map = field.GetMapData();
        return mapSaveData;
    }

    /// <summary>マップデータを反映する</summary>
    void LoadMapData(S2_SaveData saveData)
    {
        field.MapReset();
        field.Create(saveData.mapData.map);
    }
}
