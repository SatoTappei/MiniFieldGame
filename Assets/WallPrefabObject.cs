using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージの壁となるオブジェクト
/// </summary>
public class WallPrefabObject : MonoBehaviour
{
    void Start()
    {
        // 生成された後、位置をy軸方向に少しずらしてステージの見た目に変化を付ける
        float r = Random.Range(0.0f, 0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y + r, transform.position.z);
    }

    void Update()
    {
        
    }
}
