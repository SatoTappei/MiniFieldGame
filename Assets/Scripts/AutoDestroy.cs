using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w�肵�����Ԍ�Ɏ����ō폜�����I�u�W�F�N�g
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    /// <summary>�폜�����܂ł̎���</summary>
    [SerializeField] float _lifeTime;

    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
}
