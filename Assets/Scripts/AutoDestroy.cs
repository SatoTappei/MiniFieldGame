using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �w�肵�����Ԍ�Ɏ����ō폜�����I�u�W�F�N�g
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    /// <summary>�폜�����܂ł̎���</summary>
    [SerializeField] float _lifeTime;
    /// <summary>�C��:�폜�����ۂɐ��������G�t�F�N�g</summary>
    [SerializeField] GameObject _destroyedEff;
    /// <summary>�C��:�G�t�F�N�g�𐶐�����ꏊ</summary>
    [SerializeField] Transform _effTrans;

    void Start()
    {
        DOVirtual.DelayedCall(_lifeTime, () =>
        {
            Destroy(gameObject);
            // �G�t�F�N�g�Ƃ��̐����ӏ����ݒ肳��Ă�����
            if (_destroyedEff != null && _effTrans != null)
                Instantiate(_destroyedEff, _effTrans.position, Quaternion.identity);
        });
    }
}
