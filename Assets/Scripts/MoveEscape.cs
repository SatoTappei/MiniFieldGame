using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// プレイヤーから逃げる敵
/// </summary>
public class MoveEscape : MoveAlgorithmBase
{
    void Update()
    {
        
    }

    /// <summary>移動方向を返す</summary>
    public override ActorDir GetMoveDirection(PosXZ current, PosXZ target)
    {
        // ノードマップを作成
        string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
        // 現在の座標のノードを作成
        Node currentNode = new Node(current, 1);
        // 周辺のノードを格納しておくリスト
        List<Node> nextNodes = new List<Node>();
        foreach (ActorDir dir in System.Enum.GetValues(typeof(ActorDir)))
        {
            // 座標から各方向に進んだ場合の座標を作成する
            PosXZ dirPos = ActorUtility.GetTargetTile(current, dir);
            // その方向が壁なら処理を飛ばす
            if (map[dirPos.x, dirPos.z] == "W") continue;
            // その方向に進んだ先のノードを作成
            Node node = new Node(dirPos, dir, null, 1);
            // 推定コストを計算
            node._estimate = Mathf.Abs(target.x - node._pos.x) + Mathf.Abs(target.z - node._pos.z);
            // 判定するノードのリストに追加する
            nextNodes.Add(node);
        }
        // 周辺が全部移動できない場合は移動しない(ニュートラルを返す)
        if (nextNodes.Count < 1) return ActorDir.Neutral;
        // コストが高い順にソートして一番コストが高いマスを返す
        Node next = nextNodes.OrderByDescending(n => n._estimate).FirstOrDefault();
        return next._dir;
    }
}
