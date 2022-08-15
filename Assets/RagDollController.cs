using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�����񂾂Ƃ��̃��O�h�[��
/// </summary>
public class RagDollController : MonoBehaviour
{
    Rigidbody[] _rbs;
    /// <summary>�Ԃ����ł�������</summary>
    Vector3 _dir;
    /// <summary>�Ԃ����ł�������</summary>
    [SerializeField] float _power;

    public Vector3 Dir { set => _dir = value; }

    void Awake()
    {
        _rbs = GetComponentsInChildren<Rigidbody>();
    }

    void Start()
    {
        foreach (var rb in _rbs)
            rb.AddForce(_dir * -_power, ForceMode.Impulse);
    }

    void Update()
    {
        
    }
}
