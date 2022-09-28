using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// �L�����N�^�[���U�����ꂽ�Ƃ��ɔ�юU��Decal
/// </summary>
public class DecalBlood : MonoBehaviour
{
    /// <summary>��юU�������̎��</summary>
    [SerializeField] Material[] _materials;

    void Awake()
    {
        int r = Random.Range(0, _materials.Length);
        GetComponent<DecalProjector>().material = _materials[r];
        Vector3 pos = transform.position;
        Vector3 angle = transform.eulerAngles;
        float rotY = Random.Range(0, 360);
        transform.localScale *= Random.Range(0.9f, 1.1f);
        transform.position = new Vector3(pos.x + Random.Range(-0.3f, 0.3f), pos.y, pos.z + Random.Range(-0.3f, 0.3f));
        transform.eulerAngles = new Vector3(angle.x, rotY, angle.z);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
