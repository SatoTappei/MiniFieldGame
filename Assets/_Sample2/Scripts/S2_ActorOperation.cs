using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class S2_ActorOperation : MonoBehaviour
{
    /// <summary>���ɍs���\��̍s����Ԃ�Ԃ�</summary>
    public abstract EAct Operate(S2_ActorMovement actorMovement);
}
