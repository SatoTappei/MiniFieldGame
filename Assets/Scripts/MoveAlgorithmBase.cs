using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �G�̈ړ��A���S���Y���̊��N���X
/// </summary>
public abstract class MoveAlgorithmBase : MonoBehaviour
{
    /// <summary>�m�[�h(�t���A�̊e�}�X)</summary>
    public class Node
    {
        public PosXZ _pos;
        /// <summary>�e���猩������</summary>
        public ActorDir _dir;
        /// <summary>���R�X�g</summary>
        public int _actual = 0;
        /// <summary>����R�X�g</summary>
        public int _estimate = 0;
        /// <summary>���̃}�X��ʉ߂���Ƃ��̃R�X�g</summary>
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

    /// <summary>���̃^�C���ɓG�����邩�ǂ����𒲂ׂ邽�߂�MapManager�ւ̎Q�Ƃ�����</summary>
    //MapManager _mapManager;

    //void Start()
    //{
    //    _mapManager = FindObjectOfType<MapManager>();
    //}

    /// <summary>�ړ�������Ԃ�</summary>
    public abstract ActorDir GetMoveDirection(PosXZ current, PosXZ target);

    /// <summary>�w�肵���^�C���ɉ������邩�`�F�b�N����</summary>
    //public bool CheckTile(int x, int y)
    //{
    //    return _mapManager.CurrentMap.GetMapTileActor(x, y);
    //}
}