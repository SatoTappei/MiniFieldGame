using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ★<超重要>行動管理用のクラス
/// </summary>
public class S2_ActorAction : MonoBehaviour
{
    public S2_ActorMovement actorMovement;
    public S2_ActorOperation actorOperation;
    public S2_ActorAttack actorAttack;

    EAct action = EAct.KeyInput;

    /// <summary>独自の更新メソッド</summary>
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

    /// <summary>現在の行動状態を返す</summary>
    public EAct GetAction() => action;

    /// <summary>待機中</summary>
    void KeyInput()
    {
        action = actorOperation.Operate(actorMovement);
        if (action != EAct.MoveBegin) actorMovement.Stop();
    }

    /// <summary>アクションを始める</summary>
    void ActBegin()
    {
        actorAttack.Attack();
        action = EAct.Act;
    }

    /// <summary>アクション中</summary>
    void Act()
    {
        action = actorAttack.Attacking();
        if (action == EAct.ActEnd)
        {
            Pos2D grid = DirUtil.GetNewGrid(actorMovement._grid, actorMovement._direction);
            actorAttack.DamageOpponent(GetComponentInParent<S2_Field>().GetExistActor(grid.x, grid.z));
        }
    }

    /// <summary>アクションが終わった</summary>
    void ActEnd()
    {
        action = EAct.TurnEnd;
    }

    /// <summary>移動を始める</summary>
    void MoveBegin()
    {
        actorMovement.Walk();
        action = EAct.Move;
    }

    /// <summary>移動中</summary>
    void Move()
    {
        action = actorMovement.Walking();
    }

    /// <summary>移動が終わった</summary>
    void MoveEnd()
    {
        action = EAct.TurnEnd;
    }

    /// <summary>ターンが終わった</summary>
    void TurnEnd()
    {
        action = EAct.KeyInput;
    }

    /// <summary>歩行アニメーションをストップさせる</summary>
    public void StopWalkingAnimation()
    {
        actorMovement.Stop();
    }
}
