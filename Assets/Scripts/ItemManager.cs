using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// フロアに落ちているアイテムを制御する
/// </summary>
public class ItemManager : ActorBase
{
    public enum ItemType
    {
        Coin,
        PowerUp,
    }

    /// <summary>このアイテムの種類</summary>
    [SerializeField] ItemType _itemType;
    /// <summary>任意:このアイテムを獲得したときに出る演出</summary>
    [SerializeField] GameObject _getParticle;

    /// <summary>このアイテムの種類を返す</summary>
    //public ItemType GetItemType() => _itemType;

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
    public void GetThisItem()
    {
        // TODO:アイテムの種類が増えてきたらif文じゃ見栄えが良くないので直す
        FindObjectOfType<ActionLogManager>().DispLog(_defeatedMessage);
        if (_itemType == ItemType.Coin)
        {
            FindObjectOfType<PlaySceneManager>().AddScore(100);
            SoundManager._instance.Play("SE_コイン");
        }
        if (_itemType == ItemType.PowerUp)
        {
            FindObjectOfType<PlayerManager>().SetPowerUp();
            SoundManager._instance.Play("SE_パワーアップ");
        }
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileItem(_currentPosXZ.x, _currentPosXZ.z, null);
        SoundManager._instance.Play("SE_コイン");
        Instantiate(_getParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
