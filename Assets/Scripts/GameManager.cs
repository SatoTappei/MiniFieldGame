using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    /// <summary>最大ステージ番号</summary>
    int _maxStageNum;
    /// <summary>現在のステージ番号、最初は1なので注意</summary>
    int _currentStageNum = 1;
    /// <summary>合計スコア</summary>
    int _totalScore;

    public int CurrentStageNum { get => _currentStageNum; }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
