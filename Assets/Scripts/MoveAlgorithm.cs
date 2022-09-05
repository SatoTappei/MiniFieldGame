using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵の移動アルゴリズム
/// </summary>
public class MoveAlgorithm : MonoBehaviour
{
    // エッジ同士を繋ぐノード
    public class Node
    {
        public PosXZ _grid;
        public ActorDir _dir;
        public int actual = 0;
        public int estimate = 0;
        public Node parent = null;

        ///// <summary>移動方向を計算する</summary>
        //public ActorDir CalcMoveDirection(PosXZ current, PosXZ target)
        //{
        //    // 現在の座標
        //    _grid.x = current.x;
        //    _grid.z = current.z;
        //    // ノードマップを作成
        //    string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
        //    // 現在の位置を2回調べないようにノードマップ上では壁にしておく
        //    map[_grid.x, _grid.z] = "W";
        //    // 目標までのノードを計算する
        //    Node node = CalcMoveAlgorithm(target, new List<Node>(), map);
        //    // 親のノードがnullつまり移動しない場合は静止を返す
        //    if (node.parent == null) return ActorDir.Neutral;
        //    // 親の親ノードがnullじゃない、つまりは現在位置から2つ進んだ先までの間
        //    // ノードに親のノードを代入すると現在の座標から一つ先の方向が求まる
        //    while (node.parent.parent != null) node = node.parent;
        //    // ノードの方向、つまり進むべき方向を返す
        //    return node._dir;
        //}

        ///// <summary>目標までのノードを計算して返す</summary>
        //public Node CalcMoveAlgorithm(PosXZ target, List<Node> openNodes, string[,] map)
        //{
        //    // 上下左右を調べる
        //    foreach (ActorDir d in System.Enum.GetValues(typeof(ActorDir)))
        //    {
        //        // 方向がニュートラルの場合は処理を飛ばす
        //        if (d == ActorDir.Neutral) continue;
        //        // 座標から各方向に進んだ場合の座標を作成する
        //        PosXZ dir = _grid;
        //        dir = ActorUtility.GetTargetTile(dir, d);
        //        // 目標とする座標がこの方向の座標と同じなら呼び出し元が返る
        //        if (target.x == dir.x && target.z == dir.z) return this;
        //        // この方向に壁がある場合は進めないので処理を飛ばす
        //        if (map[dir.x, dir.z] == "W") continue;
        //        // この方向が目標もしくは壁ではない場合、新しくノードを作成する
        //        Node node = new Node();
        //        // 座標をこの方向に進んだ先の座標に設定する
        //        node._grid = dir;
        //        // 向きをその方向に設定する
        //        node._dir = d;
        //        // 呼び出し元のノードを親として設定する
        //        node.parent = this;
        //        // ノードの実コストを親の実コスト+1に設定する
        //        node.actual = node.parent.actual + 1;
        //        // その方向に敵がいたら回り込むようコストを追加でプラスする
        //        node.actual += 10;
        //        // ノードの推定コスト(障害物がない場合のコスト)を計算する
        //        node.estimate = Mathf.Abs(target.x - node._grid.x) + Mathf.Abs(target.z - node._grid.z);
        //        // オープンリストに追加する
        //        openNodes.Add(node);
        //        // この方向の座標は二度と計算されないように壁にしておく
        //        map[node._grid.x, node._grid.z] = "W";
        //    }
        //    // 開いたノードのリストにノードがない場合は呼び出し元を返す
        //    if (openNodes.Count < 1) return this;
        //    // 開いたノードのリストをスコア順にソートする
        //    openNodes = openNodes.OrderBy(n => n.actual + n.estimate).ThenBy(n => n.actual).ToList();
        //    // 一番スコアが小さいノードが次の基準ノードになる
        //    Node baseNode = openNodes[0];
        //    // 開いたノードのリストから一番スコアが小さいノードを削除する
        //    openNodes.RemoveAt(0);
        //    // 再帰的に呼び出す、targetは参照するのみで弄っていない
        //    // openListへの追加とnodeMapの書き換えを行った
        //    return baseNode.CalcMoveAlgorithm(target, openNodes, map);
        //}
    }

    /// <summary>移動方向を計算する</summary>
    public ActorDir CalcMoveDirection(PosXZ current, PosXZ target)
    {
        // ノードマップを作成
        string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
        // 基準ノード(現在の座標)を作成
        Node baseNode = new Node();
        baseNode._grid.x = current.x;
        baseNode._grid.z = current.z;
        // 現在の位置を2回調べないようにノードマップ上では壁にしておく
        map[baseNode._grid.x, baseNode._grid.z] = "W";
        // 目標までのノードを計算する
        Node node = CalcMoveAlgorithm(baseNode, current, target, new List<Node>(), map);
        // 親のノードがnullつまり移動しない場合は静止を返す
        if (node.parent == null) return ActorDir.Neutral;
        // 親の親ノードがnullじゃない、つまりは現在位置から2つ進んだ先までの間
        // ノードに親のノードを代入すると現在の座標から一つ先の方向が求まる
        while (node.parent.parent != null) node = node.parent;
        // ノードの方向、つまり進むべき方向を返す
        return node._dir;
    }

    /// <summary>目標までのノードを計算して返す</summary>
    public Node CalcMoveAlgorithm(Node currentNode, PosXZ current, PosXZ target, List<Node> openNodes, string[,] map)
    {
        // 上下左右を調べる
        foreach (ActorDir d in System.Enum.GetValues(typeof(ActorDir)))
        {
            // 方向がニュートラルの場合は処理を飛ばす
            if (d == ActorDir.Neutral) continue;
            // 座標から各方向に進んだ場合の座標を作成する
            PosXZ dir = ActorUtility.GetTargetTile(current, d);
            // 目標とする座標がこの方向の座標と同じなら呼び出し元が返る
            if (target.x == dir.x && target.z == dir.z) return currentNode;
            // この方向に壁がある場合は進めないので処理を飛ばす
            if (map[dir.x, dir.z] == "W") continue;
            // この方向が目標もしくは壁ではない場合、新しくノードを作成する
            Node node = new Node();
            // 座標をこの方向に進んだ先の座標に設定する
            node._grid = dir;
            // 向きをその方向に設定する
            node._dir = d;
            // 呼び出し元のノードを親として設定する
            node.parent = this;
            // ノードの実コストを親の実コスト+1に設定する
            node.actual = node.parent.actual + 1;
            // その方向に敵がいたら回り込むようコストを追加でプラスする
            node.actual += 10;
            // ノードの推定コスト(障害物がない場合のコスト)を計算する
            node.estimate = Mathf.Abs(target.x - node._grid.x) + Mathf.Abs(target.z - node._grid.z);
            // オープンリストに追加する
            openNodes.Add(node);
            // この方向の座標は二度と計算されないように壁にしておく
            map[node._grid.x, node._grid.z] = "W";
        }
        // 開いたノードのリストにノードがない場合は呼び出し元を返す
        if (openNodes.Count < 1) return this;
        // 開いたノードのリストをスコア順にソートする
        openNodes = openNodes.OrderBy(n => n.actual + n.estimate).ThenBy(n => n.actual).ToList();
        // 一番スコアが小さいノードが次の基準ノードになる
        Node baseNode = openNodes[0];
        // 開いたノードのリストから一番スコアが小さいノードを削除する
        openNodes.RemoveAt(0);
        // 再帰的に呼び出す、targetは参照するのみで弄っていない
        // openListへの追加とnodeMapの書き換えを行った
        return baseNode.CalcMoveAlgorithm(target, openNodes, map);
    }

    /// <summary>移動する方向を返す</summary>
    public ActorDir GetMoveDirection(PosXZ current, PosXZ target)
    {
        Node node = new Node();  
        return node.CalcMoveDirection(current, target);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
