using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのモデルのRootオブジェクトにアタッチする
/// アニメーションイベントに任意のメソッドを登録するため
/// </summary>
public class ActorPrefabRoot : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// アニメーションが終わったことをPlaySceneManagerに通知する、
    /// 現在は攻撃のアニメーションにのみ登録されている
    /// </summary>
    public void SendAnimationFinish() => FindObjectOfType<PlaySceneManager>().SendEndAction();

    /// <summary>攻撃されて死ぬ時の演出、Deadアニメーションの最後に呼ばれる</summary>
    public void EffectActorIsDead()
    {
        SendAnimationFinish();
        // このスクリプトがついているオブジェクトはキャラのモデルなので親ごと消す
        Destroy(transform.parent.gameObject);
    }
}
