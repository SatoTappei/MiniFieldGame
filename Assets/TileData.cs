using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�L�����N�^�[�����̃}�X�ɐN���ł��邩�̔��������̂Ɏg��</summary>
public enum TileType
{
    Floor,
    Wall,
}

/// <summary>
/// �^�C�����̂̃f�[�^
/// </summary>
public class TileData : MonoBehaviour
{
    /// <summary>���̃^�C���̎��</summary>
    [SerializeField] TileType _type;

    public TileType Type { get => _type; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}