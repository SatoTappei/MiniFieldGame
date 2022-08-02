using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_PlayerMovement : MonoBehaviour
{
    public Animator _animator;
    public Pos2D _grid;
    public EDir _direction = EDir.Up; // �� �v���C���[�̌����������A�d�v����
    public float _speed = 0.9f;
    public float _speedDampTime = 0.1f;
    public int _maxFrame = 100;

    int _currentFrame = 0;
    Pos2D _newGrid = null;

    readonly int _hashSpeedPara = Animator.StringToHash("Speed");

    void Start()
    {
        
    }

    void Update()
    {
        if (_currentFrame == 0)
        {
            EDir d = KeyToDir();
            if (d == EDir.Pause)
                _animator.SetFloat(_hashSpeedPara, 0.0f, _speedDampTime, Time.deltaTime);
            else
            {
                _direction = d;
                transform.rotation = DirToRotation(_direction);
                _newGrid = GetNewGrid(_grid, _direction);
                _grid = Move(_grid, _newGrid, ref _currentFrame);
                //transform.position += transform.forward * _speed * Time.deltaTime;
                //_animator.SetFloat(_hashSpeedPara, _speed, _speedDampTime, Time.deltaTime);
            }
        }
        else _grid = Move(_grid, _newGrid, ref _currentFrame);
    }

    /// <summary>
    /// ���͂��ꂽ�L�[�ɑΉ����������Ԃ�
    /// �������͂���Ă��Ȃ����Pause(Null�ɊY��)��Ԃ�
    /// </summary>
    EDir KeyToDir()
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
    Quaternion DirToRotation(EDir d)
    {
        // �����Ƃ��Ď��v���
        // Euler�֐��c�p�x���w�肷�邱�Ƃŉ�]�x�N�g���ɕϊ����Ă����
        Quaternion r = Quaternion.Euler(0, 0, 0);
        switch (d)
        {
            case EDir.Left:
                r = Quaternion.Euler(0, 270, 0);break;
            case EDir.Up:
                r = Quaternion.Euler(0, 0, 0); break;
            case EDir.Right:
                r = Quaternion.Euler(0, 90, 0); break;
            case EDir.Down:
                r = Quaternion.Euler(0, 180, 0); break;
        }
        return r;
    }

    /// <summary>�O���b�h���W�����[���h���W�ɕϊ� ���[���h���W = �O���b�h���W * 2</summary>
    float ToWorldX(int xgrid) => xgrid * 2;
    float ToWorldZ(int zgrid) => zgrid * 2;

    /// <summary>���[���h���W���O���b�h���W�ɕϊ� �O���b�h���W = ���[���h���W / 2�@���ď����_�ȉ���؂�̂�</summary>
    int ToGridX(float xworld) => Mathf.FloorToInt(xworld / 2);
    int ToGridZ(float zworld) => Mathf.FloorToInt(zworld / 2);

    /// <summary>�⊮�Ōv�Z���Đi��</summary>
    Pos2D Move(Pos2D currentPos, Pos2D newPos, ref int frame)
    {
        // ���݂̃O���b�h���W�����[���h���W�ɕϊ�
        float px1 = ToWorldX(currentPos.x);
        float pz1 = ToWorldZ(currentPos.z);
        // �ړI�n�̃O���b�h���W�����[���h���W�ɕϊ�
        float px2 = ToWorldX(newPos.x);
        float pz2 = ToWorldZ(newPos.z);
        // ���̊֐���maxFrame��Ăяo�����ƖړI�n�ɓ��B����
        frame++; // <= �Q�Ɠn���Ȃ̂Ō��̒l���ς��
        float t = (float)frame / _maxFrame;
        // ���̃t���[���ł̈ʒu
        float newX = px1 + (px2 - px1) * t;
        float newZ = pz1 + (pz2 - pz1) * t;

        transform.position = new Vector3(newX, 0, newZ);
        _animator.SetFloat(_hashSpeedPara, _speed, _speedDampTime, Time.deltaTime);
        // �ړ����I������玟�̃}�X�ɗ������Ƃ�Ԃ�
        if(_maxFrame == frame)
        {
            frame = 0;
            return newPos;
        }
        // �ړ����I����Ă��Ȃ���Ό��̃}�X�ɂ����ԂƂ������Ƃɂ��ĕԂ�
        return currentPos;
    }

    /// <summary>���݂̍��W�ƈړ�������������n���ƈړ���̍��W���擾</summary>
    Pos2D GetNewGrid(Pos2D position, EDir d)
    {
        Pos2D newP = new Pos2D();
        newP.x = position.x;
        newP.z = position.z;
        switch (d)
        {
            case EDir.Left:
                newP.x += -1; break;
            case EDir.Up:
                newP.z += 1; break;
            case EDir.Right:
                newP.x += 1; break;
            case EDir.Down:
                newP.z += -1; break;
        }
        return newP;
    }
}
