using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 血のパーティクルがぶつかったらオブジェクトの表面に血のデカールを生成する
/// </summary>
public class DecalBloodParticle : MonoBehaviour
{
    /// <summary>血のデカール</summary>
    [SerializeField] GameObject _bloodDecal;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        Instantiate(_bloodDecal, new Vector3(transform.position.x, 0.15f, transform.position.z), Quaternion.Euler(90, 0, 0));
        // 2回以上生成しないようにコンポーネントを破棄しておく。したくなったら下の処理を消す
        Destroy(this);
    }
}
