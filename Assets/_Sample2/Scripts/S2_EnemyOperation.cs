using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class S2_EnemyOperation : S2_ActorOperation
{
    // 講座と食い違ってtargetが存在しないのでプレイヤーをアタッチする
    public S2_ActorMovement target;

    /// <summary>次に行う予定の行動状態を返す</summary>
    public override EAct Operate(S2_ActorMovement actorMovement)
    {
        return AstarActionAI(actorMovement);
    }

    /// <summary>ランダムな行動AI</summary>
    EAct RandomActionAI(S2_ActorMovement actorMovement)
    {
        actorMovement.SetDirection(DirUtil.RandomDirection());
        if (Random.Range(0, 2) > 0)
        {
            actorMovement.IsMoveBegin();
            return EAct.MoveBegin;
        }
        return EAct.ActBegin;
    }
    /// <summary>簡易的な行動AI</summary>
    EAct EasyActionAI(S2_ActorMovement actorMovement)
    {
        EDir d = GetPlayerDirection(actorMovement);
        if (d == EDir.Pause)
        {
            d = EasyMovementAI(actorMovement);
            actorMovement.SetDirection(d);
            if (actorMovement.IsMoveBegin()) return EAct.MoveBegin;
            return EAct.KeyInput;
        }
        actorMovement.SetDirection(d);
        return EAct.ActBegin;
    }

    EAct AstarActionAI(S2_ActorMovement actorMovement)
    {
        EDir d = GetPlayerDirection(actorMovement);
        if (d == EDir.Pause)
        {
            d = AstarMovementAI(actorMovement);
            if (d == EDir.Pause) return EAct.KeyInput;
            actorMovement.SetDirection(d);
            if (actorMovement.IsMoveBegin()) return EAct.MoveBegin;
            return EAct.KeyInput;
        }
        actorMovement.SetDirection(d);
        return EAct.ActBegin;
    }

    /// <summary>AStarアルゴリズムを用いた移動AI</summary>
    EDir AstarMovementAI(S2_ActorMovement actorMovement)
    {
        // ノードクラスのインスタンスを作成
        Node node = new Node();
        // Astarを用いて移動方向を計算して返す
        return node.GetAstarNextDirection(actorMovement._grid, target._newGrid, GetComponentInParent<S2_Field>());
    }

    /// <summary>
    /// 簡易的な移動AI
    /// プレイヤーとのx軸、z軸の距離をそれぞれ図って移動する
    /// </summary>
    EDir EasyMovementAI(S2_ActorMovement actorMovement)
    {
        int dx = target._newGrid.x - actorMovement._newGrid.x;
        int dz = target._newGrid.z - actorMovement._newGrid.z;

        if (Mathf.Abs(dx) > Mathf.Abs(dz))
        {
            if (dx > 0) return EDir.Left;
            else return EDir.Right;
        }
        else
        {
            if (dz < 0) return EDir.Up;
            else return EDir.Down;
        }
    }

    /// <summary>周囲のプレイヤーがいる方向を返す</summary>
    EDir GetPlayerDirection(S2_ActorMovement actorMovement)
    {
        S2_Field field = GetComponentInParent<S2_Field>();
        Pos2D grid;
        foreach (EDir d in System.Enum.GetValues(typeof(EDir)))
        {
            if (d == EDir.Pause) continue;
            grid = DirUtil.GetNewGrid(actorMovement._grid, d);
            GameObject actor = field.GetExistActor(grid.x, grid.z);
            if (actor == null) continue;
            int id = actor.GetComponent<S2_ActorParamsController>().parameter.id;
            if (id == 0) return d;
        }
        return EDir.Pause;
    }

    /// <summary>AStar用のノードの実装</summary>
    private class Node
    {
        public Pos2D grid;
        public EDir direction;
        public int actualCost = 0;
        public int estimatedCost = 0;
        public Node parentNode = null;

        /// <summary>
        /// Astarアルゴリズムにて算出した方向を返す
        /// </summary>
        /// <param name="pos">現在位置</param>
        /// <param name="target">進むべき目標の位置</param>
        /// <returns></returns>
        public EDir GetAstarNextDirection(Pos2D pos, Pos2D target, S2_Field field)
        {
            // xz座標軸のクラスをnewする
            grid = new Pos2D();
            // それぞれの座標に現在位置を代入する
            grid.x = pos.x;
            grid.z = pos.z;
            // ノードマップを作成、壁と床をそれぞれ1と0としたint型の配列のマップをメンバーに持つクラスを作成
            S2_Array2D nodeMap = field.GetMapData();
            // ノードマップ上の現在位置に1をセット
            nodeMap.Set(grid.x, grid.z, 1);
            // 目標位置とノード型のリストと生成したノードマップを渡してAstarアルゴリズムで計算した結果を格納
            Node node = Astar(target, field, new List<Node>(), nodeMap);
            // 親のノードがnullの場合は静止を返す
            if (node.parentNode == null) return EDir.Pause;
            // 親のノードの親のノードがnullでない間はノードに親のノードを代入する <- ここ謎
            while (node.parentNode.parentNode != null) node = node.parentNode;
            // ノードの方向を返す
            return node.direction;
        }

        /// <summary>
        /// 再帰的にAstarアルゴリズムを計算し、結果を返す
        /// </summary>
        /// <param name="target">目標の位置</param>
        /// <param name="field"></param>
        /// <param name="openList">オープンしたノードを格納しておくリスト</param>
        /// <param name="nodeMap"></param>
        /// <returns></returns>
        Node Astar(Pos2D target, S2_Field field, List<Node> openList, S2_Array2D nodeMap)
        {
            foreach (EDir d in System.Enum.GetValues(typeof(EDir)))
            {
                // 方向が静止の場合は処理をスキップ
                if (d == EDir.Pause) continue;
                // 方向を渡してxz座標系での移動先の座標を作成
                Pos2D newGrid = DirUtil.GetNewGrid(grid, d);
                // 目標の位置と移動先の座標が同じならメソッドが呼び出されたインスタンス自身(呼び出し元の変数node)を返す
                if (target.x == newGrid.x && target.z == newGrid.z) return this;
                // 移動先の座標が壁なら処理をスキップ
                if (nodeMap.Get(newGrid.x, newGrid.z) > 0) continue;
                // 新しくNodeクラスのインスタンスを生成
                Node node = new Node();
                // ノードのグリッドを移動先の座標に設定
                node.grid = newGrid;
                // 方向を設定
                node.direction = d;
                // ノードの親を呼び出し元の変数に設定
                node.parentNode = this;
                // ノードの実コストを親のノードの実コスト+1に設定
                node.actualCost = node.parentNode.actualCost + 1;
                // ノードのグリッドに敵がいたら実コストに全体の敵キャラクターの数の2倍を足す
                node.actualCost += field.GetExistActor(node.grid.x, node.grid.z) == null ? 0 : field.enemies.transform.childCount * 2;
                // ノードの推定コストを ターゲットとのx距離 + ターゲットとのz距離 に設定
                node.estimatedCost = Mathf.Abs(target.x - node.grid.x) + Mathf.Abs(target.z - node.grid.z);
                // ノードをオープンリストに追加
                openList.Add(node);
                // ノードマップのノードを生成したグリッドに1を設定
                nodeMap.Set(node.grid.x, node.grid.z, 1);
            }
            // オープンリストの長さが0、移動できないなら呼び出し元を返す
            if (openList.Count < 1) return this;
            // オープンリストを並び替える、実コストと推定コストを足した値順に並べた後に実コスト順に並べる
            openList = openList.OrderBy(n => (n.actualCost + n.estimatedCost)).ThenBy(n => n.actualCost).ToList();
            // 基準ノードをオープンリストの0番とする
            Node baseNode = openList[0];
            // オープンリストの先頭(すぐ上で基準ノードにしたもの)をオープンリストから削除する
            openList.RemoveAt(0);
            // 基準ノードを使って再帰的に呼び出す
            return baseNode.Astar(target, field, openList, nodeMap);
        }
    }
}
