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
}
