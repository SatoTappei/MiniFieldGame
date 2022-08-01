using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップに登場するキャラクターの基底クラス
/// </summary>
public class ActorBase : MonoBehaviour
{
    /// <summary>このキャラクターが侵入できるタイル</summary>
    [SerializeField] TileType[] _canMoveTile;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
