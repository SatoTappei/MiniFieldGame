using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// キャラクターが攻撃されたときに飛び散るDecal
/// </summary>
public class DecalBlood : MonoBehaviour
{
    DecalProjector _dp;
    /// <summary>飛び散った血の種類</summary>
    [SerializeField] Material[] _materials;

    void Awake()
    {
        _dp = GetComponent<DecalProjector>();
        int r = Random.Range(0, _materials.Length);
        _dp.material = _materials[r];
        float ScaleMag = Random.Range(0.9f, 1.1f);
        float posMagX = Random.Range(-0.3f, 0.3f);
        float posMagZ = Random.Range(-0.3f, 0.3f);
        float rotY = Random.Range(0, 360);
        transform.localScale *= ScaleMag;
        transform.position = new Vector3(transform.position.x + posMagX, transform.position.y, transform.position.z + posMagZ);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotY, transform.eulerAngles.z);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
