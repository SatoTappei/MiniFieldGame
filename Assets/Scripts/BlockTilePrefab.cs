using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ステージを構成するブロック
/// </summary>
public class BlockTilePrefab : MonoBehaviour
{
    /// <summary>ランダムで別の色にするための床のマテリアル</summary>
    [SerializeField] Color32[] colors;
    /// <summary>生成後に位置をずらしたり後処理をする</summary>
    [SerializeField] UnityEvent _setPrefabMethod;

    void Start()
    {
        // 色をランダムに変更させる
        int r = Random.Range(0, colors.Length);
        GetComponent<MeshRenderer>().material.color = colors[r];

        _setPrefabMethod.Invoke();
    }

    void Update()
    {
        
    }

    /// <summary>位置を縦方向にランダムにずらす</summary>
    public void SetRandomY()
    {
        float r = Random.Range(0.0f, 0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y + r, transform.position.z);
    }
}
