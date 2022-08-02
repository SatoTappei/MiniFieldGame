using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�L�����N�^�[�̕���</summary>
public enum ActorDir
{
    Neutral = 360, // �������͂���Ă��Ȃ����
    Up = 0,
    Down = 180,
    Right = 90,
    Left = 270,
};

/// <summary>
/// �}�b�v�ɓo�ꂷ��L�����N�^�[�̊��N���X
/// </summary>
public abstract class ActorBase : MonoBehaviour
{
    /// <summary>���̃L�����N�^�[���N���ł���^�C��</summary>
    [SerializeField] TileType[] _canMoveTile;
    /// <summary>���݂̃L�����N�^�[�̌���</summary>
    ActorDir _currentDir = ActorDir.Up;
    /// <summary>�s�������ǂ���</summary>
    bool _inAction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
