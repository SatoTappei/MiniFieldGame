using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_EnemyOperation : S2_ActorOperation
{
    /// <summary>次に行う予定の行動状態を返す</summary>
    public override EAct Operate(S2_ActorMovement actorMovement)
    {
        return RandomActionAI(actorMovement);
    }

    /// <summary>ランダムな行動AI</summary>
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
