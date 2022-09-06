using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �G�̈ړ��A���S���Y��
/// </summary>
public class MoveAlgorithm : MonoBehaviour
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

    /// <summary>�ړ��������v�Z����</summary>
    public ActorDir GetMoveDirection(PosXZ current, PosXZ target)
    {
        // �m�[�h�}�b�v���쐬
        string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
        // ��m�[�h(���݂̍��W)���쐬
        Node baseNode = new Node(current, 1);
        // ���݂̈ʒu��2�񒲂ׂȂ��悤�Ƀm�[�h�}�b�v��ł͕ǂɂ��Ă���
        map[baseNode._pos.x, baseNode._pos.z] = "W";
        // �ڕW�܂ł̃m�[�h���v�Z����
        Node node = CalcMoveAlgorithm(baseNode, current, target, new List<Node>(), map);
        // �e�̃m�[�h��null�܂�ړ����Ȃ��ꍇ�͐Î~��Ԃ�
        if (node._parent == null) return ActorDir.Neutral;
        // �e�̐e�m�[�h��null����Ȃ��A�܂�͌��݈ʒu����2�i�񂾐�܂ł̊�
        // �m�[�h�ɐe�̃m�[�h��������ƌ��݂̍��W������̕��������܂�
        while (node._parent._parent != null) node = node._parent;
        // �m�[�h�̕����A�܂�i�ނׂ�������Ԃ�
        return node._dir;
    }

    /// <summary>�ڕW�܂ł̃m�[�h���v�Z���ĕԂ�</summary>
    public Node CalcMoveAlgorithm(Node currentNode, PosXZ current, PosXZ target, List<Node> openNodes, string[,] map)
    {
        // �㉺���E�𒲂ׂ�
        foreach (ActorDir dir in System.Enum.GetValues(typeof(ActorDir)))
        {
            // �������j���[�g�����̏ꍇ�͏������΂�
            if (dir == ActorDir.Neutral) continue;
            // ���W����e�����ɐi�񂾏ꍇ�̍��W���쐬����
            PosXZ dirPos = ActorUtility.GetTargetTile(current, dir);
            // ���̕����ɕǂ�����ꍇ�͐i�߂Ȃ��̂ŏ������΂�
            if (map[dirPos.x, dirPos.z] == "W") continue;
            // ���̕������ǂł͂Ȃ��ꍇ�A�V�����m�[�h���쐬����
            Node node = new Node(dirPos, dir, currentNode, 1);
            // �m�[�h�̎��R�X�g�Ɛ���R�X�g��ݒ肷��
            node._actual = node._parent._actual + node._massCost;
            node._estimate = Mathf.Abs(target.x - node._pos.x) + Mathf.Abs(target.z - node._pos.z);
            // ���̕����Ƀ^�[�Q�b�g�������炻�̍��W��Ԃ�
            if (target.x == dirPos.x && target.z == dirPos.z) return node;
            // �I�[�v�����X�g�ɒǉ�����
            openNodes.Add(node);
            // ���̕����̍��W�͓�x�ƌv�Z����Ȃ��悤�ɕǂɂ��Ă���
            map[node._pos.x, node._pos.z] = "W";
        }
        // �J�����m�[�h�̃��X�g�Ƀm�[�h���Ȃ��ꍇ�͌Ăяo������Ԃ�
        if (openNodes.Count < 1) return currentNode;
        // �J�����m�[�h�̃��X�g���X�R�A���Ƀ\�[�g����
        openNodes = openNodes.OrderBy(n => n._actual + n._estimate).ThenBy(n => n._actual).ToList();
        // ��ԃX�R�A���������m�[�h�����̊�m�[�h�ɂȂ�
        Node nextNode = openNodes[0];
        // �J�����m�[�h�̃��X�g�����ԃX�R�A���������m�[�h���폜����
        openNodes.RemoveAt(0);
        // �ċA�I�ɌĂяo���Atarget�͎Q�Ƃ���݂̂ŘM���Ă��Ȃ�
        // openList�ւ̒ǉ���nodeMap�̏����������s����
        return CalcMoveAlgorithm(nextNode, nextNode._pos, target, openNodes, map);
    }
}
