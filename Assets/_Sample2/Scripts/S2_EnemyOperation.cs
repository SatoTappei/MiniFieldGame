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

    /// <summary>A*アルゴリズムを用いた行動AI</summary>
    EAct AstarActionAI(S2_ActorMovement actorMovement)
    {
        // プレイヤーのいる方向を返す
        EDir d = GetPlayerDirection(actorMovement);
        // 静止？
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
        return node.GetAsterNextDirection(actorMovement._grid, target._newGrid, GetComponentInParent<S2_Field>());
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
        // 2次元上での座標
        public Pos2D grid;
        // 方向
        public EDir direction;
        // 実コスト
        public int actualCost = 0;
        // 推定コスト
        public int estimatedCost = 0;
        // 親ノード
        public Node parentNode = null;

        /// <summary>
        /// A*アルゴリズムにて算出した方向を返す
        /// </summary>
        /// <param name="pos">現在の座標</param>
        /// <param name="target">次の座標</param>
        /// <param name="field"></param>
        /// <returns></returns>
        public EDir GetAsterNextDirection(Pos2D pos, Pos2D target, S2_Field field)
        {
            // 現在の座標
            grid = new Pos2D();
            grid.x = pos.x;
            grid.z = pos.z;
            // ノードマップ = int型の配列(二次元ではない)をメンバーに持つクラス
            S2_Array2D nodeMap = field.GetMapData();
            // ノードマップのこのノードの位置を壁にする
            nodeMap.Set(grid.x, grid.z, 1);
            // 目標の座標とノードのリストを渡して計算
            Node node = Astar(target, field, new List<Node>(), nodeMap);
            // 親のノードがnullつまり移動しない場合は静止を返す
            if (node.parentNode == null) return EDir.Pause;
            // 親の親のノードがnullじゃない、つまりは現在位置から2つ進んだ先までの間
            // ノードに親のノードを代入する <- 一つ先の方向が求まる
            while (node.parentNode.parentNode != null) node = node.parentNode;
            // ノードの方向、つまり進むべき方向を返す
            return node.direction;
        }

        /// <summary>
        /// 再帰的にA*アルゴリズムを計算し、結果を返す
        /// </summary>
        /// <param name="target">次の座標</param>
        /// <param name="field"></param>
        /// <param name="openList">オープンにしたノードを格納しておくリスト</param>
        /// <param name="nodeMap">ノードマップ(実際のマップをメンバーに持つクラス)</param>
        /// <returns></returns>
        Node Astar(Pos2D target, S2_Field field, List<Node> openList, S2_Array2D nodeMap)
        {
            //4方向0,1,2,3,4の値の分だけ回す、現在の座標から4方向を調べる
            foreach (EDir d in System.Enum.GetValues(typeof(EDir)))
            {
                // 方向がPause = 0、つまり静止の場合は処理を飛ばす
                if (d == EDir.Pause) continue;
                // 方向の座標系を作成
                Pos2D newGrid = DirUtil.GetNewGrid(grid, d);
                // 目標とする座標がこの方向の座標と同じなら呼び出し元が返る
                if (target.x == newGrid.x && target.z == newGrid.z) return this;
                // この方向には壁がある場合は処理を飛ばす <- 開いた箇所は壁にするので2回は計算されない
                if (nodeMap.Get(newGrid.x, newGrid.z) > 0) continue;
                // 移動先が目標もしくは壁ではない場合、新しくノードを作成する
                Node node = new Node();
                // 座標をその方向に進んだ先の座標に設定する
                node.grid = newGrid;
                // 向きをその方向に設定する
                node.direction = d;
                // 呼び出し元のノードを親として設定する
                node.parentNode = this;
                // ノードの実コストを親の実コスト+1に設定する
                node.actualCost = node.parentNode.actualCost + 1;
                // 任意:ノードに敵がいたら、ノードの実コストにフィールドに存在する敵の数の2倍を足す
                node.actualCost += field.GetExistActor(node.grid.x, node.grid.z) == null ? 0 : field.enemies.transform.childCount * 2;
                // ノードの推定コストを計算、目標との距離を計算する
                node.estimatedCost = Mathf.Abs(target.x - node.grid.x) + Mathf.Abs(target.z - node.grid.z);
                // オープンリストに追加する
                openList.Add(node);
                // この方向は二度と計算されないように壁にしておく
                nodeMap.Set(node.grid.x, node.grid.z, 1);
            }
            // 開いたノードのリストが0以下、つまり移動できる場所がない場合は呼び出し元を返す
            if (openList.Count < 1) return this;
            // 開いたノードのリストをスコア順にソートする
            openList = openList.OrderBy(n => n.actualCost + n.estimatedCost).ThenBy(n => n.actualCost).ToList();
            // 一番スコアが小さいノードが次の基準ノードになる
            Node baseNode = openList[0];
            // 開いたノードのリストから一番スコアが小さいノードを削除する
            openList.RemoveAt(0);
            // 再帰的に呼び出す、targetとfieldは参照するのみで弄っていない
            // openListへの追加とnodeMapの書き換えを行った
            return baseNode.Astar(target, field, openList, nodeMap);
        }
    }
}
