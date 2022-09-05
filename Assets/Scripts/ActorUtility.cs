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

/// <summary>XZ���ʏ�ł̍��W</summary>
public struct PosXZ
{
    public int x;
    public int z;
}

/// <summary>
/// �Q�[���ɓo�ꂷ��A�C�e���A�L�����N�^�[�֗̕��N���X
/// </summary>
public static class ActorUtility
{
    /// <summary>���݂̍��W�ƕ�������ړ���̍��W���擾</summary>
    public static PosXZ GetTargetTile(PosXZ current, ActorDir dir)
    {
        PosXZ target = current;

        if (dir == ActorDir.Up) target.z++;
        else if (dir == ActorDir.Down) target.z--;
        else if (dir == ActorDir.Right) target.x++;
        else if (dir == ActorDir.Left) target.x--;

        // �ړ��悪�ǂȂ猻�݂̈ʒu��Ԃ��A���̍ۂ̓^�[����i�߂Ȃ��悤�ɂ���
        return target;
    }
}
