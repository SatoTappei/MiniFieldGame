using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyOperation : S2_ActorOperation
{
    /// <summary>���ɍs���\��̍s����Ԃ�Ԃ�</summary>
    public override EAct Operate(S2_ActorMovement actorMovement)
    {
        return RandomActionAI(actorMovement);
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
}
