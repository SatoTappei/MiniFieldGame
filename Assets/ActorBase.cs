using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップに登場するキャラとアイテムの基底クラス
/// </summary>
public abstract class ActorBase : MonoBehaviour
{
    /// <summary>XZ平面上での座標</summary>
    protected struct PosXZ
    {
        public int x;
        public int z;
    }

    /// <summary>現在のXZ平面上での位置</summary>
    protected PosXZ _currentPosXZ;

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
    public abstract void InitPosXZ();
}
