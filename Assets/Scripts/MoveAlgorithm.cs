using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ActorBase;
using static CharacterBase;

/// <summary>
/// �G�̈ړ��A���S���Y��
/// </summary>
public class MoveAlgorithm : MonoBehaviour
{
    // �G�b�W���m���q���m�[�h
    class Node
    {
        public ActorBase.PosXZ _grid;
        public CharacterBase.Direction _dir;
        public int actual = 0;
        public int estimate = 0;
        public Node parent = null;

        /// <summary>�ړ��������v�Z����</summary>
        public CharacterBase.Direction CalcMoveDirection(ActorBase.PosXZ current, ActorBase.PosXZ target)
        {
            // ���݂̍��W
            _grid.x = current.x;
            _grid.z = current.z;
            // �m�[�h�}�b�v���쐬
            string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
            // ���݂̈ʒu��2�񒲂ׂȂ��悤�Ƀm�[�h�}�b�v��ł͕ǂɂ��Ă���
            map[_grid.x, _grid.z] = "W";
            // �ڕW�܂ł̃m�[�h���v�Z����
            Node node = CalcMoveAlgorithm(target, new List<Node>(), map);
            // �e�̃m�[�h��null�܂�ړ����Ȃ��ꍇ�͐Î~��Ԃ�
            if (node.parent == null) return CharacterBase.Direction.Neutral;
            // �e�̐e�m�[�h��null����Ȃ��A�܂�͌��݈ʒu����2�i�񂾐�܂ł̊�
            // �m�[�h�ɐe�̃m�[�h��������ƌ��݂̍��W������̕��������܂�
            while (node.parent.parent != null) node = node.parent;
            // �m�[�h�̕����A�܂�i�ނׂ�������Ԃ�
            return node._dir;
        }

        /// <summary>�ڕW�܂ł̃m�[�h���v�Z���ĕԂ�</summary>
        public Node CalcMoveAlgorithm(ActorBase.PosXZ target, List<Node> openNodes, string[,] map)
        {
            // �㉺���E�𒲂ׂ�
            foreach (CharacterBase.Direction d in System.Enum.GetValues(typeof(CharacterBase.Direction)))
            {
                // �������j���[�g�����̏ꍇ�͏������΂�
                if (d == CharacterBase.Direction.Neutral) continue;
                // ���W����e�����ɐi�񂾏ꍇ�̍��W���쐬����
                ActorBase.PosXZ dir = _grid;
                if (d == CharacterBase.Direction.Up) dir.z++;
                else if (d == CharacterBase.Direction.Down) dir.z--;
                else if (d == CharacterBase.Direction.Right) dir.x++;
                else if (d == CharacterBase.Direction.Left) dir.x--;
                // �ڕW�Ƃ�����W�����̕����̍��W�Ɠ����Ȃ�Ăяo�������Ԃ�
                if (target.x == dir.x && target.z == dir.z) return this;
                // ���̕����ɕǂ�����ꍇ�͐i�߂Ȃ��̂ŏ������΂�
                if (map[dir.x, dir.z] == "W") continue;
                // ���̕������ڕW�������͕ǂł͂Ȃ��ꍇ�A�V�����m�[�h���쐬����
                Node node = new Node();
                // ���W�����̕����ɐi�񂾐�̍��W�ɐݒ肷��
                node._grid = dir;
                // ���������̕����ɐݒ肷��
                node._dir = d;
                // �Ăяo�����̃m�[�h��e�Ƃ��Đݒ肷��
                node.parent = this;
                // �m�[�h�̎��R�X�g��e�̎��R�X�g+1�ɐݒ肷��
                node.actual = node.parent.actual + 1;
                // ���̕����ɓG���������荞�ނ悤�R�X�g��ǉ��Ńv���X����
                node.actual += 10;
                // �m�[�h�̐���R�X�g(��Q�����Ȃ��ꍇ�̃R�X�g)���v�Z����
                node.estimate = Mathf.Abs(target.x - node._grid.x) + Mathf.Abs(target.z - node._grid.z);
                // �I�[�v�����X�g�ɒǉ�����
                openNodes.Add(node);
                // ���̕����̍��W�͓�x�ƌv�Z����Ȃ��悤�ɕǂɂ��Ă���
                map[node._grid.x, node._grid.z] = "W";
            }
            // �J�����m�[�h�̃��X�g�Ƀm�[�h���Ȃ��ꍇ�͌Ăяo������Ԃ�
            if (openNodes.Count < 1) return this;
            // �J�����m�[�h�̃��X�g���X�R�A���Ƀ\�[�g����
            openNodes = openNodes.OrderBy(n => n.actual + n.estimate).ThenBy(n => n.actual).ToList();
            // ��ԃX�R�A���������m�[�h�����̊�m�[�h�ɂȂ�
            Node baseNode = openNodes[0];
            // �J�����m�[�h�̃��X�g�����ԃX�R�A���������m�[�h���폜����
            openNodes.RemoveAt(0);
            // �ċA�I�ɌĂяo���Atarget�͎Q�Ƃ���݂̂ŘM���Ă��Ȃ�
            // openList�ւ̒ǉ���nodeMap�̏����������s����
            return baseNode.CalcMoveAlgorithm(target, openNodes, map);
        }
    }

    /// <summary>�ړ����������Ԃ�</summary>
    public CharacterBase.Direction GetMoveDirection(ActorBase.PosXZ current, ActorBase.PosXZ target)
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
