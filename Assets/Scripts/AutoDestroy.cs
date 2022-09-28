using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;

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
    /// <summary>任意:対象を破壊するときの音</summary>
    [SerializeField] string _soundEffectName;
    /// <summary>カメラを揺らすか</summary>
    [SerializeField] bool _doCameraShake;

    async void Start()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(_lifeTime));
        Destroy(gameObject);
        // エフェクトとその生成箇所が設定されていたら
        if (_destroyedEff != null && _effTrans != null)
            Instantiate(_destroyedEff, _effTrans.position, Quaternion.identity);
        // 音が設定されていたら
        if (_soundEffectName != "")
            SoundManager._instance.Play(_soundEffectName);
        // カメラを揺らすフラグがオンならば
        if (_doCameraShake)
        {
            FindObjectOfType<CinemachineImpulseSource>().GenerateImpulse();
        }
    }
}
