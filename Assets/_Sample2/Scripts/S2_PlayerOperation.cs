using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_PlayerOperation : S2_ActorOperation
{
    //public S2_ActorMovement actorMovement;

    void Start()
    {
        
    }

    //void Update()
    //{
    //    EDir d = DirUtil.KeyToDir();
    //    if (d != EDir.Pause)
    //    {
    //        actorMovement.SetDirection(d);
    //        actorMovement.Walk();
    //    }
    //}

    /// <summary>éüÇ…çsÇ§ó\íËÇÃçsìÆèÛë‘Çï‘Ç∑</summary>
    public override EAct Operate(S2_ActorMovement actorMovement)
    {
        if (!Input.anyKey) return EAct.KeyInput;
        if (Input.GetKey(KeyCode.Space)) return EAct.ActBegin;

        EDir d = DirUtil.KeyToDir();
        if(d != EDir.Pause)
        {
            actorMovement.SetDirection(d);
            if (actorMovement.IsMoveBegin()) return EAct.MoveBegin;
        }
        return EAct.KeyInput;
    }
}
