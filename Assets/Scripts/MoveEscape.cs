using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �v���C���[���瓦����G
/// </summary>
public class MoveEscape : MoveAlgorithmBase
{
    void Update()
    {
        
    }

    /// <summary>�ړ�������Ԃ�</summary>
    public override ActorDir GetMoveDirection(PosXZ current, PosXZ target)
    {
        // �m�[�h�}�b�v���쐬
        string[,] map = FindObjectOfType<MapManager>().GetMapCopy();
        // ���݂̍��W�̃m�[�h���쐬
        Node currentNode = new Node(current, 1);
        // ���ӂ̃m�[�h���i�[���Ă������X�g
        List<Node> nextNodes = new List<Node>();
        foreach (ActorDir dir in System.Enum.GetValues(typeof(ActorDir)))
        {
            // ���W����e�����ɐi�񂾏ꍇ�̍��W���쐬����
            PosXZ dirPos = ActorUtility.GetTargetTile(current, dir);
            // ���̕������ǂȂ珈�����΂�
            if (map[dirPos.x, dirPos.z] == "W") continue;
            // ���̕����ɐi�񂾐�̃m�[�h���쐬
            Node node = new Node(dirPos, dir, null, 1);
            // ����R�X�g���v�Z
            node._estimate = Mathf.Abs(target.x - node._pos.x) + Mathf.Abs(target.z - node._pos.z);
            // ���肷��m�[�h�̃��X�g�ɒǉ�����
            nextNodes.Add(node);
        }
        // ���ӂ��S���ړ��ł��Ȃ��ꍇ�͈ړ����Ȃ�(�j���[�g������Ԃ�)
        if (nextNodes.Count < 1) return ActorDir.Neutral;
        // �R�X�g���������Ƀ\�[�g���Ĉ�ԃR�X�g�������}�X��Ԃ�
        Node next = nextNodes.OrderByDescending(n => n._estimate).FirstOrDefault();
        return next._dir;
    }
}
