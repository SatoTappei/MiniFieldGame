using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���}�l�[�W���[
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    /// <summary>�ő�X�e�[�W�ԍ�</summary>
    int _maxStageNum;
    /// <summary>���݂̃X�e�[�W�ԍ��A�ŏ���1�Ȃ̂Œ���</summary>
    int _currentStageNum = 1;
    /// <summary>���v�X�R�A</summary>
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
