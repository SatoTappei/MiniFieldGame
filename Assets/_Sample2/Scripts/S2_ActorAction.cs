using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��<���d�v>�s���Ǘ��p�̃N���X
/// </summary>
public class S2_ActorAction : MonoBehaviour
{
    public S2_ActorMovement actorMovement;
    public S2_ActorOperation actorOperation;
    public S2_ActorAttack actorAttack;

    EAct action = EAct.KeyInput;

    /// <summary>�Ǝ��̍X�V���\�b�h</summary>
    public void Proc()
    {
        switch (action)
        {
            case EAct.KeyInput:  KeyInput();  break;
            case EAct.ActBegin:  ActBegin();  break;
            case EAct.Act:       Act();       break;
            case EAct.ActEnd:    ActEnd();    break;
            case EAct.MoveBegin: MoveBegin(); break;
            case EAct.Move:      Move();      break;
            case EAct.MoveEnd:   MoveEnd();   break;
            case EAct.TurnEnd:   TurnEnd();   break;
        }
    }

    /// <summary>���݂̍s����Ԃ�Ԃ�</summary>
    public EAct GetAction() => action;

    /// <summary>�ҋ@��</summary>
    void KeyInput()
    {
        action = actorOperation.Operate(actorMovement);
        if (action != EAct.MoveBegin) actorMovement.Stop();
    }

    /// <summary>�A�N�V�������n�߂�</summary>
    void ActBegin()
    {
        actorAttack.Attack();
        action = EAct.Act;
    }

    /// <summary>�A�N�V������</summary>
    void Act()
    {
        action = actorAttack.Attacking();
        if (action == EAct.ActEnd)
        {
            Pos2D grid = DirUtil.GetNewGrid(actorMovement._grid, actorMovement._direction);
            actorAttack.DamageOpponent(GetComponentInParent<S2_Field>().GetExistActor(grid.x, grid.z));
        }
    }

    /// <summary>�A�N�V�������I�����</summary>
    void ActEnd()
    {
        action = EAct.TurnEnd;
    }

    /// <summary>�ړ����n�߂�</summary>
    void MoveBegin()
    {
        actorMovement.Walk();
        action = EAct.Move;
    }

    /// <summary>�ړ���</summary>
    void Move()
    {
        action = actorMovement.Walking();
    }

    /// <summary>�ړ����I�����</summary>
    void MoveEnd()
    {
        action = EAct.TurnEnd;
    }

    /// <summary>�^�[�����I�����</summary>
    void TurnEnd()
    {
        action = EAct.KeyInput;
    }

    /// <summary>���s�A�j���[�V�������X�g�b�v������</summary>
    public void StopWalkingAnimation()
    {
        actorMovement.Stop();
    }
}
