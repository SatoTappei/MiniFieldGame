using UnityEngine;

public static class DirUtil
{
    /// <summary>
    /// ���͂��ꂽ�L�[�ɑΉ����������Ԃ�
    /// �������͂���Ă��Ȃ����Pause(Null�ɊY��)��Ԃ�
    /// </summary>
    public static EDir KeyToDir()
    {
        // ���t���[����4�̔��������͖̂��ʂȂ̂ŁA�L�[�����͂���Ă��Ȃ�����Pause��Ԃ�
        if (!Input.anyKey) return EDir.Pause;

        if (Input.GetKey(KeyCode.LeftArrow)) return EDir.Left;
        if (Input.GetKey(KeyCode.UpArrow)) return EDir.Up;
        if (Input.GetKey(KeyCode.RightArrow)) return EDir.Right;
        if (Input.GetKey(KeyCode.DownArrow)) return EDir.Down;

        // �����L�[�ȊO��������Ă���Ƃ���Pause��Ԃ�
        return EDir.Pause;
    }

    /// <summary>�����ŗ^����ꂽ�����ɑΉ������]�̃x�N�g����Ԃ�</summary>
    public static Quaternion DirToRotation(EDir d)
    {
        // �����Ƃ��Ď��v���
        // Euler�֐��c�p�x���w�肷�邱�Ƃŉ�]�x�N�g���ɕϊ����Ă����
        Quaternion r = Quaternion.Euler(0, 0, 0);
        switch (d)
        {
            case EDir.Right:
                r = Quaternion.Euler(0, 270, 0); break;
            case EDir.Down:
                r = Quaternion.Euler(0, 0, 0); break;
            case EDir.Left:
                r = Quaternion.Euler(0, 90, 0); break;
            case EDir.Up:
                r = Quaternion.Euler(0, 180, 0); break;
        }
        return r;
    }

    /// <summary>�����ŗ^����ꂽ��]�̃x�N�g���ɑΉ����������Ԃ�</summary>
    public static EDir RotationToDir(Quaternion r)
    {
        float y = r.eulerAngles.y;
        if (y < 45)
        {
            return EDir.Down;
        }
        else if (y < 135)
        {
            return EDir.Left;
        }
        else if (y < 225)
        {
            return EDir.Up;
        }
        else if (y < 315)
        {
            return EDir.Right;
        }

        return EDir.Down;
    }

    /// <summary>���݂̍��W�ƈړ�������������n���ƈړ���̍��W���擾</summary>
    public static Pos2D GetNewGrid(Pos2D position, EDir d)
    {
        Pos2D newP = new Pos2D();
        newP.x = position.x;
        newP.z = position.z;
        switch (d)
        {
            case EDir.Left:
                newP.x += 1; break;
            case EDir.Up:
                newP.z += -1; break;
            case EDir.Right:
                newP.x += -1; break;
            case EDir.Down:
                newP.z += 1; break;
        }
        return newP;
    }

    /// <summary>
    /// �}�b�v�̃f�[�^(map)�ƌ��݂̍��W(position)�ƈړ�����������(d)��n����
    /// (�����ړ��ł���Ȃ��)�ړ���̍��W���擾
    /// </summary>
    public static Pos2D Move(S2_Field field, Pos2D position, EDir d)
    {
        // ��Q��������Ό��݂̃O���b�h���W�A�Ȃ���Έړ���̃O���b�h���W���擾����
        Pos2D newP = GetNewGrid(position, d);
        if (field.IsCollide(newP.x, newP.z) || field.GetExistActor(newP.x,newP.z) != null) 
            return position;
        return newP;
    }

    /// <summary>�����_���Ȍ�����Ԃ�</summary>
    public static EDir RandomDirection()
    {
        int dirnum = Random.Range(0, 4);
        switch (dirnum)
        {
            case 0:
                return EDir.Left;
            case 1:
                return EDir.Up;
            case 2:
                return EDir.Right;
            case 3:
                return EDir.Down;
        }
        return EDir.Down;
    }
}