using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ActorBase;
using static CharacterBase;

/// <summary>
/// 敵の移動アルゴリズム
/// </summary>
public class MoveAlgorithm : MonoBehaviour
{
    // エッジ同士を繋ぐノード
    class Node
    {
        public ActorBase.PosXZ _grid;
        public CharacterBase.Direction _dir;
        public int actual = 0;
        public int estimate = 0;
        public Node parent = null;

        /// <summary>移動方向を計算する</summary>
        public CharacterBase.Direction CalcMoveDirection(ActorBase.PosXZ current, ActorBase.PosXZ target)
        {
            // 現在の座標
            _grid.x = current.x;
            _grid.z = current.z;
            // ノードマップを作成
            string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
            Node node = CalcMoveAlgorithm(target, new List<Node>(), map);
            //string[] map = { "haha","gaga","fafa"}/*FindObjectOfType<MapManager>().MapStr.Split('\n').Take(3).ToArray()*/;
            // マップ上のこのノードの位置を壁にする
            //map[_grid.x][_grid.z] = "W";
            return CharacterBase.Direction.Neutral;
        }

        /// <summary>目標までのノードを計算して返す</summary>
        public Node CalcMoveAlgorithm(ActorBase.PosXZ target, List<Node> openNodes, string[,] map)
        {
            // 4方向を調べる
            foreach (CharacterBase.Direction d in System.Enum.GetValues(typeof(CharacterBase.Direction)))
            {
                // 方向がニュートラルの場合は処理を飛ばす
                if (d == CharacterBase.Direction.Neutral) continue;
                // 座標から各方向に進んだ場合の座標を作成する
                // 目標とする座標がこの方向の座標と同じなら呼び出し元が返る
                // この方向に壁がある場合は進めないので処理を飛ばす

            }
            return new Node();
        }
    }

    /// <summary>移動する方向を返す</summary>
    public CharacterBase.Direction GetMoveDirection()
    {
        return CharacterBase.Direction.Neutral;
    }

    // スコア(実コスト+推定コスト)の計算方法
    // このノードが基点なら実コストは0、親のノードがあるなら親の実コスト+1
    // actual = parent == null ? 0 : parent.actual + 1
    // 推定コストはターゲットの座標-自身の座標で求める
    // estimate = (target.x - grid.x) + (target.z - grid.z);
    // スコアは実コストと推定コストの合計となる
    // int score = actual + estimate;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
