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

        /// <summary>
        /// �i�ޕ������v�Z���ĕԂ�
        /// </summary>
        /// <param name="current">���݂̍��W</param>
        /// <param name="target">�ڕW(�v���C���[)�̍��W</param>
        /// <returns></returns>
        public CharacterBase.Direction CalcMoveDirection(ActorBase.PosXZ current, ActorBase.PosXZ target)
        {
            // ���݂̍��W
            _grid.x = current.x;
            _grid.z = current.z;
            // �m�[�h�}�b�v���쐬
            string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
            // �ڕW�܂ł̃m�[�h���A���S���Y����p���Čv�Z
            // node��parent�ɍċA�I��parent������������ԂŕԂ��Ă���
            Node node = CalcMoveAlgorithm(target, current, new List<Node>(), map);
            // Node��Parent��null�A�܂�ǂ��ɂ��m�[�h���q�����Ă��Ȃ��̂ňړ����Ȃ�
            if (node.parent == null) return CharacterBase.Direction.Neutral;
            // �e�̐e�̃m�[�h��null����Ȃ��A�܂�͌��݈ʒu����2�i�񂾐�܂ł̊�
            // �m�[�h�ɐe�̃m�[�h�������� <- ���̕��������܂�
            while (node.parent.parent != null) node = node.parent;
            // 1��̃m�[�h�̕�����Ԃ�
            return node._dir;
        }

        /// <summary>
        /// �ڕW�܂ł̃m�[�h���v�Z���ĕԂ�
        /// </summary>
        /// <param name="target">�ڕW(�v���C���[)�̍��W</param>
        /// <param name="openNodes">�J�����m�[�h���i�[���Ă������X�g</param>
        /// <param name="map">�m�[�h�}�b�v</param>
        /// <returns></returns>
        public Node CalcMoveAlgorithm(ActorBase.PosXZ target, ActorBase.PosXZ current, List<Node> openNodes, string[,] map)
        {
            
            // 4�����𒲂ׂ�
            foreach (CharacterBase.Direction d in System.Enum.GetValues(typeof(CharacterBase.Direction)))
            {
                // �������j���[�g�����̏ꍇ�͏������΂�
                if (d == CharacterBase.Direction.Neutral) continue;
                // ���W����e�����ɐi�񂾏ꍇ�̍��W���쐬����
                PosXZ dir = current;
                // TODO:�������ǂ��ɂ�����
                if (d == CharacterBase.Direction.Up) dir.z++;
                else if (d == CharacterBase.Direction.Down) dir.z--;
                else if (d == CharacterBase.Direction.Right) dir.x++;
                else if (d == CharacterBase.Direction.Left) dir.x--;
                // �ڕW�Ƃ�����W�����̕����̍��W�Ɠ����Ȃ�Ăяo�������Ԃ�
                if (dir.x == target.x && dir.z == target.z) return this;
                // ���̕����ɕǂ�����ꍇ�͐i�߂Ȃ��̂ŏ������΂�
                if (map[dir.x, dir.z] == "W") continue;
                // �ړ��悪�ڕW�������͕ǂłȂ��ꍇ�́A�V�����m�[�h���쐬����
                Node node = new Node();
                // ���W�����̕����ɐi�񂾐�̍��W�ɐݒ肷��
                node._grid = dir;
                // ���������̕����ɐݒ肷��
                node._dir = d;
                // �Ăяo�����̃m�[�h��e�Ƃ��Đݒ肷��
                node.parent = this;
                // �m�[�h�̎��R�X�g��e�̎��R�X�g��+1�ɐݒ肷��
                node.actual = node.parent.actual + 1;
                // �C��:���̕����ɓG��������m�[�h�̎��R�X�g�Ƀt�B�[���h�ɐ��l�𑫂�
                node.actual += FindObjectOfType<MapManager>().CurrentMap.GetMapTileActor(dir.x, dir.z) == null ? 0 : 10;
                // �m�[�h�̐���R�X�g���v�Z�A�ڕW�Ƃ̋������v�Z����
                node.estimate = Mathf.Abs(target.x - node._grid.x) + Mathf.Abs(target.z - node._grid.z);
                // �I�[�v�����X�g�ɒǉ�����
                openNodes.Add(node);
                // ���̕����͓�x�ƌv�Z����Ȃ��悤�ɕǂɂ��Ă���
                map[node._grid.x, node._grid.z] = "W";
            }
            // �J�����m�[�h�̃��X�g��0�ȉ��A�܂�ړ��ł���ꏊ���Ȃ��ꍇ�͌Ăяo������Ԃ�
            if (openNodes.Count < 1) return this;
            // �J�����m�[�h�̃��X�g���X�R�A���Ƀ\�[�g����
            openNodes = openNodes.OrderBy(n => n.actual + n.estimate).ThenBy(n => n.actual).ToList();
            // ��ԃX�R�A���������m�[�h�����̊�m�[�h�ɂȂ�
            Node baseNode = openNodes[0];
            // �J�����m�[�h�̃��X�g�����ԃX�R�A���������m�[�h���폜����
            openNodes.RemoveAt(0);
            return baseNode.CalcMoveAlgorithm(target, current, openNodes, map);
        }
    }

    /// <summary>�ړ����������Ԃ�</summary>
    public CharacterBase.Direction GetMoveDirection(ActorBase.PosXZ current, ActorBase.PosXZ target)
    {
        //Node node = new Node();
        return CharacterBase.Direction.Neutral;/*node.CalcMoveDirection(current, target);*/
    }

    // �X�R�A(���R�X�g+����R�X�g)�̌v�Z���@
    // ���̃m�[�h����_�Ȃ���R�X�g��0�A�e�̃m�[�h������Ȃ�e�̎��R�X�g+1
    // actual = parent == null ? 0 : parent.actual + 1
    // ����R�X�g�̓^�[�Q�b�g�̍��W-���g�̍��W�ŋ��߂�
    // estimate = (target.x - grid.x) + (target.z - grid.z);
    // �X�R�A�͎��R�X�g�Ɛ���R�X�g�̍��v�ƂȂ�
    // int score = actual + estimate;

    //void Start()
    //{
        
    //}

    //void Update()
    //{
        
    //}
}
