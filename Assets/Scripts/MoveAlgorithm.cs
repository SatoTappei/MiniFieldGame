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
            Node node = CalcMoveAlgorithm(target, new List<Node>(), map);
            //string[] map = { "haha","gaga","fafa"}/*FindObjectOfType<MapManager>().MapStr.Split('\n').Take(3).ToArray()*/;
            // �}�b�v��̂��̃m�[�h�̈ʒu��ǂɂ���
            //map[_grid.x][_grid.z] = "W";
            return CharacterBase.Direction.Neutral;
        }

        /// <summary>�ڕW�܂ł̃m�[�h���v�Z���ĕԂ�</summary>
        public Node CalcMoveAlgorithm(ActorBase.PosXZ target, List<Node> openNodes, string[,] map)
        {
            // 4�����𒲂ׂ�
            foreach (CharacterBase.Direction d in System.Enum.GetValues(typeof(CharacterBase.Direction)))
            {
                // �������j���[�g�����̏ꍇ�͏������΂�
                if (d == CharacterBase.Direction.Neutral) continue;
                // ���W����e�����ɐi�񂾏ꍇ�̍��W���쐬����
                // �ڕW�Ƃ�����W�����̕����̍��W�Ɠ����Ȃ�Ăяo�������Ԃ�
                // ���̕����ɕǂ�����ꍇ�͐i�߂Ȃ��̂ŏ������΂�

            }
            return new Node();
        }
    }

    /// <summary>�ړ����������Ԃ�</summary>
    public CharacterBase.Direction GetMoveDirection()
    {
        return CharacterBase.Direction.Neutral;
    }

    // �X�R�A(���R�X�g+����R�X�g)�̌v�Z���@
    // ���̃m�[�h����_�Ȃ���R�X�g��0�A�e�̃m�[�h������Ȃ�e�̎��R�X�g+1
    // actual = parent == null ? 0 : parent.actual + 1
    // ����R�X�g�̓^�[�Q�b�g�̍��W-���g�̍��W�ŋ��߂�
    // estimate = (target.x - grid.x) + (target.z - grid.z);
    // �X�R�A�͎��R�X�g�Ɛ���R�X�g�̍��v�ƂȂ�
    // int score = actual + estimate;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
