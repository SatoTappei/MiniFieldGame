using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class S2_ActorOperation : MonoBehaviour
{
    /// <summary>次に行う予定の行動状態を返す</summary>
    public abstract EAct Operate(S2_ActorMovement actorMovement);
}
