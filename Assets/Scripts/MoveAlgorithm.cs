using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵の移動アルゴリズム
/// </summary>
public class MoveAlgorithm : MonoBehaviour
{
    /// <summary>ノード(フロアの各マス)</summary>
    public class Node
    {
        public PosXZ _pos;
        /// <summary>親から見た方向</summary>
        public ActorDir _dir;
        /// <summary>実コスト</summary>
        public int _actual = 0;
        /// <summary>推定コスト</summary>
        public int _estimate = 0;
        /// <summary>このマスを通過するときのコスト</summary>
        public int _massCost = 1;
        public Node _parent = null;

        public Node(PosXZ pos, int massCost)
        {
            _pos = pos;
            _massCost = massCost;
        }
        public Node(PosXZ pos, ActorDir dir, Node parent, int massCost)
        {
            _pos = pos;
            _dir = dir;
            _parent = parent;
            _massCost = massCost;
        }
    }

    /// <summary>移動方向を計算する</summary>
    public ActorDir GetMoveDirection(PosXZ current, PosXZ target)
    {
        // ノードマップを作成
        string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
        // 基準ノード(現在の座標)を作成
        Node baseNode = new Node(current, 1);
        // 現在の位置を2回調べないようにノードマップ上では壁にしておく
        map[baseNode._pos.x, baseNode._pos.z] = "W";
        // 目標までのノードを計算する
        Node node = CalcMoveAlgorithm(baseNode, current, target, new List<Node>(), map);
        // 親のノードがnullつまり移動しない場合は静止を返す
        if (node._parent == null) return ActorDir.Neutral;
        // 親の親ノードがnullじゃない、つまりは現在位置から2つ進んだ先までの間
        // ノードに親のノードを代入すると現在の座標から一つ先の方向が求まる
        while (node._parent._parent != null) node = node._parent;
        // ノードの方向、つまり進むべき方向を返す
        return node._dir;
    }

    /// <summary>目標までのノードを計算して返す</summary>
    public Node CalcMoveAlgorithm(Node currentNode, PosXZ current, PosXZ target, List<Node> openNodes, string[,] map)
    {
        // 上下左右を調べる
        foreach (ActorDir dir in System.Enum.GetValues(typeof(ActorDir)))
        {
            // 方向がニュートラルの場合は処理を飛ばす
            if (dir == ActorDir.Neutral) continue;
            // 座標から各方向に進んだ場合の座標を作成する
            PosXZ dirPos = ActorUtility.GetTargetTile(current, dir);
            // この方向に壁がある場合は進めないので処理を飛ばす
            if (map[dirPos.x, dirPos.z] == "W") continue;
            // この方向が壁ではない場合、新しくノードを作成する
            Node node = new Node(dirPos, dir, currentNode, 1);
            // ノードの実コストと推定コストを設定する
            node._actual = node._parent._actual + node._massCost;
            node._estimate = Mathf.Abs(target.x - node._pos.x) + Mathf.Abs(target.z - node._pos.z);
            // その方向にターゲットがいたらその座標を返す
            if (target.x == dirPos.x && target.z == dirPos.z) return node;
            // オープンリストに追加する
            openNodes.Add(node);
            // この方向の座標は二度と計算されないように壁にしておく
            map[node._pos.x, node._pos.z] = "W";
        }
        // 開いたノードのリストにノードがない場合は呼び出し元を返す
        if (openNodes.Count < 1) return currentNode;
        // 開いたノードのリストをスコア順にソートする
        openNodes = openNodes.OrderBy(n => n._actual + n._estimate).ThenBy(n => n._actual).ToList();
        // 一番スコアが小さいノードが次の基準ノードになる
        Node nextNode = openNodes[0];
        // 開いたノードのリストから一番スコアが小さいノードを削除する
        openNodes.RemoveAt(0);
        // 再帰的に呼び出す、targetは参照するのみで弄っていない
        // openListへの追加とnodeMapの書き換えを行った
        return CalcMoveAlgorithm(nextNode, nextNode._pos, target, openNodes, map);
    }
}
