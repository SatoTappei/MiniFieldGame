using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_ActorAttack : MonoBehaviour
{
    public Animator animator;
    public float animationLength = 1.0f;

    readonly int hashAttackPara = Animator.StringToHash("Attack");
    float time = 0.0f;

    /// <summary>攻撃アニメーションを開始</summary>
    public void Attack()
    {
        S2_Message.add("Attack");
        animator.SetTrigger(hashAttackPara);
    }

    /// <summary>攻撃中</summary>
    public EAct Attacking()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        time += Time.deltaTime;
        if (time > animationLength)
        {
            time = 0.0f;
            return EAct.ActEnd;
        }
        return EAct.Act;
    }
}
