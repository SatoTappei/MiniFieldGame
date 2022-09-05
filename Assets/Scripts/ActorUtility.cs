using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>キャラクターの方向</summary>
public enum ActorDir
{
    Neutral = 360, // 何も入力されていない状態
    Up = 0,
    Down = 180,
    Right = 90,
    Left = 270,
};

/// <summary>XZ平面上での座標</summary>
public struct PosXZ
{
    public int x;
    public int z;
}

/// <summary>
/// ゲームに登場するアイテム、キャラクターの便利クラス
/// </summary>
public static class ActorUtility
{
    /// <summary>現在の座標と方向から移動先の座標を取得</summary>
    public static PosXZ GetTargetTile(PosXZ current, ActorDir dir)
    {
        PosXZ target = current;

        if (dir == ActorDir.Up) target.z++;
        else if (dir == ActorDir.Down) target.z--;
        else if (dir == ActorDir.Right) target.x++;
        else if (dir == ActorDir.Left) target.x--;

        // 移動先が壁なら現在の位置を返す、その際はターンを進めないようにする
        return target;
    }
}
