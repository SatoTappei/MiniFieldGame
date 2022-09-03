using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップに登場するキャラとアイテムの基底クラス
/// </summary>
public abstract class ActorBase : MonoBehaviour
{
    /// <summary>撃破された時に出るログのメッセージ</summary>
    [SerializeField] protected string _defeatedMessage;

    /// <summary>XZ平面上での座標</summary>
    public struct PosXZ
    {
        public int x;
        public int z;
    }

    /// <summary>現在のXZ平面上での位置</summary>
    protected PosXZ _currentPosXZ;

    public PosXZ CurrentPosXZ { get => _currentPosXZ; }

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
