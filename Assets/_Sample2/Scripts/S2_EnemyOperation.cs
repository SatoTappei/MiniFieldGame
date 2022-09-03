using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class S2_EnemyOperation : S2_ActorOperation
{
    // �u���ƐH�������target�����݂��Ȃ��̂Ńv���C���[���A�^�b�`����
    public S2_ActorMovement target;

    /// <summary>���ɍs���\��̍s����Ԃ�Ԃ�</summary>
    public override EAct Operate(S2_ActorMovement actorMovement)
    {
        return AstarActionAI(actorMovement);
    }

    /// <summary>�����_���ȍs��AI</summary>
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
    /// <summary>�ȈՓI�ȍs��AI</summary>
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

    /// <summary>A*�A���S���Y����p�����s��AI</summary>
    EAct AstarActionAI(S2_ActorMovement actorMovement)
    {
        // �v���C���[�̂��������Ԃ�
        EDir d = GetPlayerDirection(actorMovement);
        // �Î~�H
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

    /// <summary>AStar�A���S���Y����p�����ړ�AI</summary>
    EDir AstarMovementAI(S2_ActorMovement actorMovement)
    {
        // �m�[�h�N���X�̃C���X�^���X���쐬
        Node node = new Node();
        // Astar��p���Ĉړ��������v�Z���ĕԂ�
        return node.GetAsterNextDirection(actorMovement._grid, target._newGrid, GetComponentInParent<S2_Field>());
    }

    /// <summary>
    /// �ȈՓI�Ȉړ�AI
    /// �v���C���[�Ƃ�x���Az���̋��������ꂼ��}���Ĉړ�����
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

    /// <summary>���͂̃v���C���[�����������Ԃ�</summary>
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

    /// <summary>AStar�p�̃m�[�h�̎���</summary>
    private class Node
    {
        // 2������ł̍��W
        public Pos2D grid;
        // ����
        public EDir direction;
        // ���R�X�g
        public int actualCost = 0;
        // ����R�X�g
        public int estimatedCost = 0;
        // �e�m�[�h
        public Node parentNode = null;

        /// <summary>
        /// A*�A���S���Y���ɂĎZ�o����������Ԃ�
        /// </summary>
        /// <param name="pos">���݂̍��W</param>
        /// <param name="target">���̍��W</param>
        /// <param name="field"></param>
        /// <returns></returns>
        public EDir GetAsterNextDirection(Pos2D pos, Pos2D target, S2_Field field)
        {
            // ���݂̍��W
            grid = new Pos2D();
            grid.x = pos.x;
            grid.z = pos.z;
            // �m�[�h�}�b�v = int�^�̔z��(�񎟌��ł͂Ȃ�)�������o�[�Ɏ��N���X
            S2_Array2D nodeMap = field.GetMapData();
            // �m�[�h�}�b�v�̂��̃m�[�h�̈ʒu��ǂɂ���
            nodeMap.Set(grid.x, grid.z, 1);
            // �ڕW�̍��W�ƃm�[�h�̃��X�g��n���Čv�Z
            Node node = Astar(target, field, new List<Node>(), nodeMap);
            // �e�̃m�[�h��null�܂�ړ����Ȃ��ꍇ�͐Î~��Ԃ�
            if (node.parentNode == null) return EDir.Pause;
            // �e�̐e�̃m�[�h��null����Ȃ��A�܂�͌��݈ʒu����2�i�񂾐�܂ł̊�
            // �m�[�h�ɐe�̃m�[�h�������� <- ���̕��������܂�
            while (node.parentNode.parentNode != null) node = node.parentNode;
            // �m�[�h�̕����A�܂�i�ނׂ�������Ԃ�
            return node.direction;
        }

        /// <summary>
        /// �ċA�I��A*�A���S���Y�����v�Z���A���ʂ�Ԃ�
        /// </summary>
        /// <param name="target">���̍��W</param>
        /// <param name="field"></param>
        /// <param name="openList">�I�[�v���ɂ����m�[�h���i�[���Ă������X�g</param>
        /// <param name="nodeMap">�m�[�h�}�b�v(���ۂ̃}�b�v�������o�[�Ɏ��N���X)</param>
        /// <returns></returns>
        Node Astar(Pos2D target, S2_Field field, List<Node> openList, S2_Array2D nodeMap)
        {
            //4����0,1,2,3,4�̒l�̕������񂷁A���݂̍��W����4�����𒲂ׂ�
            foreach (EDir d in System.Enum.GetValues(typeof(EDir)))
            {
                // ������Pause = 0�A�܂�Î~�̏ꍇ�͏������΂�
                if (d == EDir.Pause) continue;
                // �����̍��W�n���쐬
                Pos2D newGrid = DirUtil.GetNewGrid(grid, d);
                // �ڕW�Ƃ�����W�����̕����̍��W�Ɠ����Ȃ�Ăяo�������Ԃ�
                if (target.x == newGrid.x && target.z == newGrid.z) return this;
                // ���̕����ɂ͕ǂ�����ꍇ�͏������΂� <- �J�����ӏ��͕ǂɂ���̂�2��͌v�Z����Ȃ�
                if (nodeMap.Get(newGrid.x, newGrid.z) > 0) continue;
                // �ړ��悪�ڕW�������͕ǂł͂Ȃ��ꍇ�A�V�����m�[�h���쐬����
                Node node = new Node();
                // ���W�����̕����ɐi�񂾐�̍��W�ɐݒ肷��
                node.grid = newGrid;
                // ���������̕����ɐݒ肷��
                node.direction = d;
                // �Ăяo�����̃m�[�h��e�Ƃ��Đݒ肷��
                node.parentNode = this;
                // �m�[�h�̎��R�X�g��e�̎��R�X�g+1�ɐݒ肷��
                node.actualCost = node.parentNode.actualCost + 1;
                // �C��:�m�[�h�ɓG��������A�m�[�h�̎��R�X�g�Ƀt�B�[���h�ɑ��݂���G�̐���2�{�𑫂�
                node.actualCost += field.GetExistActor(node.grid.x, node.grid.z) == null ? 0 : field.enemies.transform.childCount * 2;
                // �m�[�h�̐���R�X�g���v�Z�A�ڕW�Ƃ̋������v�Z����
                node.estimatedCost = Mathf.Abs(target.x - node.grid.x) + Mathf.Abs(target.z - node.grid.z);
                // �I�[�v�����X�g�ɒǉ�����
                openList.Add(node);
                // ���̕����͓�x�ƌv�Z����Ȃ��悤�ɕǂɂ��Ă���
                nodeMap.Set(node.grid.x, node.grid.z, 1);
            }
            // �J�����m�[�h�̃��X�g��0�ȉ��A�܂�ړ��ł���ꏊ���Ȃ��ꍇ�͌Ăяo������Ԃ�
            if (openList.Count < 1) return this;
            // �J�����m�[�h�̃��X�g���X�R�A���Ƀ\�[�g����
            openList = openList.OrderBy(n => n.actualCost + n.estimatedCost).ThenBy(n => n.actualCost).ToList();
            // ��ԃX�R�A���������m�[�h�����̊�m�[�h�ɂȂ�
            Node baseNode = openList[0];
            // �J�����m�[�h�̃��X�g�����ԃX�R�A���������m�[�h���폜����
            openList.RemoveAt(0);
            // �ċA�I�ɌĂяo���Atarget��field�͎Q�Ƃ���݂̂ŘM���Ă��Ȃ�
            // openList�ւ̒ǉ���nodeMap�̏����������s����
            return baseNode.Astar(target, field, openList, nodeMap);
        }
    }
}
