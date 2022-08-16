using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定した時間後に自動で削除されるオブジェクト
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    /// <summary>削除されるまでの時間</summary>
    [SerializeField] float _lifeTime;

    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
}
