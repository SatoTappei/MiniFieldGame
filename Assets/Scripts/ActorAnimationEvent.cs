using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの"モデル名_Root"オブジェクトにアタッチする
/// アニメーションイベントに任意のメソッドを登録するため
/// </summary>
public class ActorAnimationEvent : MonoBehaviour
{
    /// <summary>攻撃の際に生成するエフェクト</summary>
    [SerializeField] GameObject _attackEffect;
    /// <summary>攻撃の際に生成するエフェクトの親</summary>
    [SerializeField] Transform _effectParent;

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>攻撃する際にエフェクトを出す</summary>
    public void GenerateAttackEffect()
    {
        if (_attackEffect != null && _effectParent != null)
        {
            var go = Instantiate(_attackEffect, _effectParent.position, Quaternion.identity);
            go.transform.SetParent(_effectParent);
        }
    }

    /// <summary>
    /// アニメーションが終わったことをPlaySceneManagerに通知する、
    /// 現在は攻撃のアニメーションにのみ登録されている
    /// </summary>
    public void SendAnimationFinish() => FindObjectOfType<PlaySceneManager>().SendEndAction();
}
