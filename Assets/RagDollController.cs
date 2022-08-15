using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターが死んだときのラグドール
/// </summary>
public class RagDollController : MonoBehaviour
{
    Rigidbody[] _rbs;
    /// <summary>ぶっ飛んでいく方向</summary>
    Vector3 _dir;
    /// <summary>ぶっ飛んでいく強さ</summary>
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
