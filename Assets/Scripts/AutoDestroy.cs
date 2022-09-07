using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 指定した時間後に自動で削除されるオブジェクト
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    /// <summary>削除されるまでの時間</summary>
    [SerializeField] float _lifeTime;
    /// <summary>任意:削除される際に生成されるエフェクト</summary>
    [SerializeField] GameObject _destroyedEff;
    /// <summary>任意:エフェクトを生成する場所</summary>
    [SerializeField] Transform _effTrans;

    void Start()
    {
        DOVirtual.DelayedCall(_lifeTime, () =>
        {
            Destroy(gameObject);
            // エフェクトとその生成箇所が設定されていたら
            if (_destroyedEff != null && _effTrans != null)
                Instantiate(_destroyedEff, _effTrans.position, Quaternion.identity);
        });
    }
}
