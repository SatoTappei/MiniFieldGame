using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_SequenceManager : MonoBehaviour
{
    public S2_ActorAction playerAction;
    public GameObject enemies;

    bool isEnemyDeterminedBehaviour = false;
    bool isEnemyEndedAct = false;
    bool isEnemyEndedMovement = false;

    void Start()
    {
        
    }

    void Update()
    {
        EAct pAct = playerAction.GetAction();
        if (pAct == EAct.KeyInput || pAct == EAct.ActBegin || pAct == EAct.Act)
        {
            playerAction.Proc();
            foreach (var enemyAction in enemies.GetComponentsInChildren<S2_ActorAction>())
            {
                enemyAction.StopWalkingAnimation();
            }
            return;
        }
        if (pAct == EAct.TurnEnd)
        {
            playerAction.Proc();
            AllEnemyProc();
            isEnemyDeterminedBehaviour = false;
            isEnemyEndedAct = false;
            isEnemyEndedMovement = false;
            return;
        }
        if (isEnemyEndedAct && isEnemyEndedMovement)
        {
            playerAction.Proc();
            AllEnemyProc();
            return;
        }
        if (pAct == EAct.ActEnd && !isEnemyDeterminedBehaviour)
        {
            AllEnemyProc();
            isEnemyDeterminedBehaviour = true;
            return;
        }
        if (pAct == EAct.MoveBegin)
        {
            playerAction.Proc();
            AllEnemyProc();
            isEnemyDeterminedBehaviour = true;
            return;
        }
        if (pAct == EAct.ActEnd && isEnemyEndedAct)
        {
            bool isNotEnd = false;
            foreach (var enemyAction in enemies.GetComponentsInChildren<S2_ActorAction>())
            {
                EAct eAct = enemyAction.GetAction();
                if (eAct == EAct.MoveBegin || eAct == EAct.Move)
                {
                    enemyAction.Proc();
                    isNotEnd = true;
                }
            }
            isEnemyEndedMovement = !isNotEnd;
            return;
        }
        if (pAct == EAct.ActEnd && isEnemyDeterminedBehaviour)
        {
            bool isNotEnd = false;
            playerAction.StopWalkingAnimation();
            foreach (var enemyAction in enemies.GetComponentsInChildren<S2_ActorAction>())
            {
                EAct eAct = enemyAction.GetAction();
                if (eAct == EAct.ActBegin || eAct == EAct.Act)
                {
                    enemyAction.Proc();
                    isNotEnd = true;
                }
                enemyAction.StopWalkingAnimation();
            }
            isEnemyEndedAct = !isNotEnd;
            return;
        }
        if (pAct == EAct.MoveEnd && isEnemyEndedMovement)
        {
            bool isNotEnd = false;
            playerAction.StopWalkingAnimation();
            foreach (var enemyAction in enemies.GetComponentsInChildren<S2_ActorAction>())
            {
                EAct eAct = enemyAction.GetAction();
                if (eAct == EAct.ActBegin || eAct == EAct.Act)
                {
                    enemyAction.Proc();
                    isNotEnd = true;
                }
                enemyAction.StopWalkingAnimation();
            }
            isEnemyEndedAct = !isNotEnd;
            return;
        }
        if (isEnemyDeterminedBehaviour)
        {
            bool isNotEnd = false;
            if (pAct == EAct.Move)
            {
                playerAction.Proc();
                isNotEnd = true;
            }
            foreach (var enemyAction in enemies.GetComponentsInChildren<S2_ActorAction>())
            {
                EAct eAct = enemyAction.GetAction();
                if (eAct == EAct.MoveBegin || eAct == EAct.Move)
                {
                    enemyAction.Proc();
                    isNotEnd = true;
                }
            }
            isEnemyEndedMovement = !isNotEnd;
            return;
        }
    }

    /// <summary>ìGëSàıÇçsìÆÇ≥ÇπÇÈ</summary>
    void AllEnemyProc()
    {
        foreach (var enemyAction in enemies.GetComponentsInChildren<S2_ActorAction>())
        {
            enemyAction.Proc();
        }
    }
}
