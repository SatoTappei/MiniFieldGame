using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージの床となるオブジェクト
/// </summary>
public class FloorPrefabObject : MonoBehaviour
{
    /// <summary>ランダムで別の色にするための床のマテリアル</summary>
    [SerializeField] Material _mat;

    void Start()
    {
        int r = Random.Range(1, 3);
        if (r == 1) GetComponent<MeshRenderer>().material = _mat;
    }

    void Update()
    {
        
    }
}
