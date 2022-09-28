using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵の移動アルゴリズムの基底クラス
/// </summary>
public abstract class MoveAlgorithmBase : MonoBehaviour
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

    /// <summary>移動方向を返す</summary>
    public abstract ActorDir GetMoveDirection(PosXZ current, PosXZ target);
}
