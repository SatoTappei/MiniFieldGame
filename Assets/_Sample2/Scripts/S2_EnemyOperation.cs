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

    /// <summary>AStar�A���S���Y����p�����ړ�AI</summary>
    EDir AstarMovementAI(S2_ActorMovement actorMovement)
    {
        // �m�[�h�N���X�̃C���X�^���X���쐬
        Node node = new Node();
        // Astar��p���Ĉړ��������v�Z���ĕԂ�
        return node.GetAstarNextDirection(actorMovement._grid, target._newGrid, GetComponentInParent<S2_Field>());
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
        public Pos2D grid;
        public EDir direction;
        public int actualCost = 0;
        public int estimatedCost = 0;
        public Node parentNode = null;

        /// <summary>
        /// Astar�A���S���Y���ɂĎZ�o����������Ԃ�
        /// </summary>
        /// <param name="pos">���݈ʒu</param>
        /// <param name="target">�i�ނׂ��ڕW�̈ʒu</param>
        /// <returns></returns>
        public EDir GetAstarNextDirection(Pos2D pos, Pos2D target, S2_Field field)
        {
            // xz���W���̃N���X��new����
            grid = new Pos2D();
            // ���ꂼ��̍��W�Ɍ��݈ʒu��������
            grid.x = pos.x;
            grid.z = pos.z;
            // �m�[�h�}�b�v���쐬�A�ǂƏ������ꂼ��1��0�Ƃ���int�^�̔z��̃}�b�v�������o�[�Ɏ��N���X���쐬
            S2_Array2D nodeMap = field.GetMapData();
            // �m�[�h�}�b�v��̌��݈ʒu��1���Z�b�g
            nodeMap.Set(grid.x, grid.z, 1);
            // �ڕW�ʒu�ƃm�[�h�^�̃��X�g�Ɛ��������m�[�h�}�b�v��n����Astar�A���S���Y���Ōv�Z�������ʂ��i�[
            Node node = Astar(target, field, new List<Node>(), nodeMap);
            // �e�̃m�[�h��null�̏ꍇ�͐Î~��Ԃ�
            if (node.parentNode == null) return EDir.Pause;
            // �e�̃m�[�h�̐e�̃m�[�h��null�łȂ��Ԃ̓m�[�h�ɐe�̃m�[�h�������� <- ������
            while (node.parentNode.parentNode != null) node = node.parentNode;
            // �m�[�h�̕�����Ԃ�
            return node.direction;
        }

        /// <summary>
        /// �ċA�I��Astar�A���S���Y�����v�Z���A���ʂ�Ԃ�
        /// </summary>
        /// <param name="target">�ڕW�̈ʒu</param>
        /// <param name="field"></param>
        /// <param name="openList">�I�[�v�������m�[�h���i�[���Ă������X�g</param>
        /// <param name="nodeMap"></param>
        /// <returns></returns>
        Node Astar(Pos2D target, S2_Field field, List<Node> openList, S2_Array2D nodeMap)
        {
            foreach (EDir d in System.Enum.GetValues(typeof(EDir)))
            {
                // �������Î~�̏ꍇ�͏������X�L�b�v
                if (d == EDir.Pause) continue;
                // ������n����xz���W�n�ł̈ړ���̍��W���쐬
                Pos2D newGrid = DirUtil.GetNewGrid(grid, d);
                // �ڕW�̈ʒu�ƈړ���̍��W�������Ȃ烁�\�b�h���Ăяo���ꂽ�C���X�^���X���g(�Ăяo�����̕ϐ�node)��Ԃ�
                if (target.x == newGrid.x && target.z == newGrid.z) return this;
                // �ړ���̍��W���ǂȂ珈�����X�L�b�v
                if (nodeMap.Get(newGrid.x, newGrid.z) > 0) continue;
                // �V����Node�N���X�̃C���X�^���X�𐶐�
                Node node = new Node();
                // �m�[�h�̃O���b�h���ړ���̍��W�ɐݒ�
                node.grid = newGrid;
                // ������ݒ�
                node.direction = d;
                // �m�[�h�̐e���Ăяo�����̕ϐ��ɐݒ�
                node.parentNode = this;
                // �m�[�h�̎��R�X�g��e�̃m�[�h�̎��R�X�g+1�ɐݒ�
                node.actualCost = node.parentNode.actualCost + 1;
                // �m�[�h�̃O���b�h�ɓG����������R�X�g�ɑS�̂̓G�L�����N�^�[�̐���2�{�𑫂�
                node.actualCost += field.GetExistActor(node.grid.x, node.grid.z) == null ? 0 : field.enemies.transform.childCount * 2;
                // �m�[�h�̐���R�X�g�� �^�[�Q�b�g�Ƃ�x���� + �^�[�Q�b�g�Ƃ�z���� �ɐݒ�
                node.estimatedCost = Mathf.Abs(target.x - node.grid.x) + Mathf.Abs(target.z - node.grid.z);
                // �m�[�h���I�[�v�����X�g�ɒǉ�
                openList.Add(node);
                // �m�[�h�}�b�v�̃m�[�h�𐶐������O���b�h��1��ݒ�
                nodeMap.Set(node.grid.x, node.grid.z, 1);
            }
            // �I�[�v�����X�g�̒�����0�A�ړ��ł��Ȃ��Ȃ�Ăяo������Ԃ�
            if (openList.Count < 1) return this;
            // �I�[�v�����X�g����ёւ���A���R�X�g�Ɛ���R�X�g�𑫂����l���ɕ��ׂ���Ɏ��R�X�g���ɕ��ׂ�
            openList = openList.OrderBy(n => (n.actualCost + n.estimatedCost)).ThenBy(n => n.actualCost).ToList();
            // ��m�[�h���I�[�v�����X�g��0�ԂƂ���
            Node baseNode = openList[0];
            // �I�[�v�����X�g�̐擪(������Ŋ�m�[�h�ɂ�������)���I�[�v�����X�g����폜����
            openList.RemoveAt(0);
            // ��m�[�h���g���čċA�I�ɌĂяo��
            return baseNode.Astar(target, field, openList, nodeMap);
        }
    }
}
