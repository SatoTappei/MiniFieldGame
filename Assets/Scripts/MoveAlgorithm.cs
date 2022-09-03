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

        /// <summary>
        /// 進む方向を計算して返す
        /// </summary>
        /// <param name="current">現在の座標</param>
        /// <param name="target">目標(プレイヤー)の座標</param>
        /// <returns></returns>
        public CharacterBase.Direction CalcMoveDirection(ActorBase.PosXZ current, ActorBase.PosXZ target)
        {
            // 現在の座標
            _grid.x = current.x;
            _grid.z = current.z;
            // ノードマップを作成
            string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
            // 目標までのノードをアルゴリズムを用いて計算
            // nodeのparentに再帰的にparentがくっついた状態で返ってくる
            Node node = CalcMoveAlgorithm(target, current, new List<Node>(), map);
            // NodeのParentがnull、つまりどこにもノードが繋がっていないので移動しない
            if (node.parent == null) return CharacterBase.Direction.Neutral;
            // 親の親のノードがnullじゃない、つまりは現在位置から2つ進んだ先までの間
            // ノードに親のノードを代入する <- 一つ先の方向が求まる
            while (node.parent.parent != null) node = node.parent;
            // 1つ先のノードの方向を返す
            return node._dir;
        }

        /// <summary>
        /// 目標までのノードを計算して返す
        /// </summary>
        /// <param name="target">目標(プレイヤー)の座標</param>
        /// <param name="openNodes">開けたノードを格納しておくリスト</param>
        /// <param name="map">ノードマップ</param>
        /// <returns></returns>
        public Node CalcMoveAlgorithm(ActorBase.PosXZ target, ActorBase.PosXZ current, List<Node> openNodes, string[,] map)
        {
            
            // 4方向を調べる
            foreach (CharacterBase.Direction d in System.Enum.GetValues(typeof(CharacterBase.Direction)))
            {
                // 方向がニュートラルの場合は処理を飛ばす
                if (d == CharacterBase.Direction.Neutral) continue;
                // 座標から各方向に進んだ場合の座標を作成する
                PosXZ dir = current;
                // TODO:↓ここどうにかする
                if (d == CharacterBase.Direction.Up) dir.z++;
                else if (d == CharacterBase.Direction.Down) dir.z--;
                else if (d == CharacterBase.Direction.Right) dir.x++;
                else if (d == CharacterBase.Direction.Left) dir.x--;
                // 目標とする座標がこの方向の座標と同じなら呼び出し元が返る
                if (dir.x == target.x && dir.z == target.z) return this;
                // この方向に壁がある場合は進めないので処理を飛ばす
                if (map[dir.x, dir.z] == "W") continue;
                // 移動先が目標もしくは壁でない場合は、新しくノードを作成する
                Node node = new Node();
                // 座標をその方向に進んだ先の座標に設定する
                node._grid = dir;
                // 向きをその方向に設定する
                node._dir = d;
                // 呼び出し元のノードを親として設定する
                node.parent = this;
                // ノードの実コストを親の実コストの+1に設定する
                node.actual = node.parent.actual + 1;
                // 任意:その方向に敵がいたらノードの実コストにフィールドに数値を足す
                node.actual += FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor(dir.x, dir.z) == null ? 0 : 10;
                // ノードの推定コストを計算、目標との距離を計算する
                node.estimate = Mathf.Abs(target.x - node._grid.x) + Mathf.Abs(target.z - node._grid.z);
                // オープンリストに追加する
                openNodes.Add(node);
                // この方向は二度と計算されないように壁にしておく
                map[node._grid.x, node._grid.z] = "W";
            }
            // 開いたノードのリストが0以下、つまり移動できる場所がない場合は呼び出し元を返す
            if (openNodes.Count < 1) return this;
            // 開いたノードのリストをスコア順にソートする
            openNodes = openNodes.OrderBy(n => n.actual + n.estimate).ThenBy(n => n.actual).ToList();
            // 一番スコアが小さいノードが次の基準ノードになる
            Node baseNode = openNodes[0];
            // 開いたノードのリストから一番スコアが小さいノードを削除する
            openNodes.RemoveAt(0);
            return baseNode.CalcMoveAlgorithm(target, current, openNodes, map);
        }
    }

    /// <summary>移動する方向を返す</summary>
    public CharacterBase.Direction GetMoveDirection(ActorBase.PosXZ current, ActorBase.PosXZ target)
    {
        //Node node = new Node();
        return CharacterBase.Direction.Neutral;/*node.CalcMoveDirection(current, target);*/
    }

    // スコア(実コスト+推定コスト)の計算方法
    // このノードが基点なら実コストは0、親のノードがあるなら親の実コスト+1
    // actual = parent == null ? 0 : parent.actual + 1
    // 推定コストはターゲットの座標-自身の座標で求める
    // estimate = (target.x - grid.x) + (target.z - grid.z);
    // スコアは実コストと推定コストの合計となる
    // int score = actual + estimate;

    //void Start()
    //{
        
    //}

    //void Update()
    //{
        
    //}
}
