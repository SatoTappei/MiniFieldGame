using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フロアに落ちているアイテムを制御する
/// </summary>
public class ItemManager : ActorBase
{
    public enum ItemType
    {
        Coin,
    }

    /// <summary>このアイテムの種類</summary>
    [SerializeField] ItemType _itemType;
    
    /// <summary>このアイテムの種類を返す</summary>
    public ItemType GetItemType() => _itemType;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// MapGeneratorでマップ生成時、配置場所が決まったら
    /// ワールド座標をタイル上の座標に変換してセットする
    /// </summary>
    public override void InitPosXZ()
    {
        _currentPosXZ.x = (int)transform.position.x;
        _currentPosXZ.z = (int)transform.position.z;

        // 生成された座標に自身をセットして攻撃や移動の判定に使えるようにする
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileItem(_currentPosXZ.x, _currentPosXZ.z, this);
    }

    /// <summary>このアイテムを取得した際の処理</summary>
    public void GetThisItemProc()
    {
        FindObjectOfType<ActionLogManager>().DispLog(_defeatedMessage);
        // TODO:現在はアイテムがコインだけなのでスコアを追加する処理を書いている
        FindObjectOfType<PlaySceneManager>().AddScore(100);
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileItem(_currentPosXZ.x, _currentPosXZ.z, null);
        SoundManager._instance.Play("SE_コイン");
        Destroy(gameObject);
        Debug.Log("コインを獲得");
    }
}
