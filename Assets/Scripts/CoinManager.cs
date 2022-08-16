using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フロアに落ちているコインを制御する
/// 現在は獲得したらスコアを増やすだけなのでActorBaseを継承していない
/// </summary>
public class CoinManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // コライダーで検知するのでそのためだけにPlayerにリジットボディとコライダーを付けている
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<PlaySceneManager>().AddScore(100);
        }
    }
}
