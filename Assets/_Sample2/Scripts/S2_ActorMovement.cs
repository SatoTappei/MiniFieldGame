using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_ActorMovement : MonoBehaviour
{
    public Animator _animator;
    public Pos2D _grid;
    public EDir _direction = EDir.Up; // �� �v���C���[�̌����������A�d�v����
    public float _speed = 0.9f;
    public float _speedDampTime = 0.1f;
    //public int _maxFrame = 100;
    public float maxPerFrame = 1.67f;
    float completentFrame;

    int _currentFrame = 0;
    public Pos2D _newGrid = null;

    readonly int _hashSpeedPara = Animator.StringToHash("Speed");

    void Start()
    {
        completentFrame = maxPerFrame / Time.deltaTime;
        _newGrid = _grid;
    }

    /// <summary>���s��</summary>
    public EAct Walking()
    {
        if (_grid.Equals(_newGrid) && _currentFrame == 0)
        {
            _animator.SetFloat(_hashSpeedPara, 0.0f, _speedDampTime, Time.deltaTime);
            return EAct.MoveEnd;
        }
        _grid = Move(_grid, _newGrid, ref _currentFrame);
        return EAct.Move;
    }

    /// <summary>��~</summary>
    public void Stop()
    {
        if (_animator.GetFloat(_hashSpeedPara) > 0.0f)
        {
            _animator.SetFloat(_hashSpeedPara, 0.0f, _speedDampTime, Time.deltaTime);
        }
    }

    /// <summary>�폜����Update</summary>
    //void Update()
    //{
    //    if (_grid.Equals(_newGrid) && _currentFrame == 0)
    //        _animator.SetFloat(_hashSpeedPara, 0.0f, _speedDampTime, Time.deltaTime);
    //    else _grid = Move(_grid, _newGrid, ref _currentFrame);
    //    //if (_currentFrame == 0)
    //    //{
    //    //    EDir d = DirUtil.KeyToDir();
    //    //    if (d == EDir.Pause)
    //    //        _animator.SetFloat(_hashSpeedPara, 0.0f, _speedDampTime, Time.deltaTime);
    //    //    else
    //    //    {
    //    //        _direction = d;
    //    //        S2_Message.add(_direction.ToString());
    //    //        transform.rotation = DirUtil.DirToRotation(_direction);
    //    //        _newGrid = DirUtil.Move(GetComponentInParent<S2_Field>(), _grid, _direction);
    //    //        _grid = Move(_grid, _newGrid, ref _currentFrame);
    //    //    }
    //    //}
    //    //else _grid = Move(_grid, _newGrid, ref _currentFrame);
    //}

    /// <summary>�⊮�Ōv�Z���Đi��</summary>
    Pos2D Move(Pos2D currentPos, Pos2D newPos, ref int frame)
    {
        // ���݂̃O���b�h���W�����[���h���W�ɕϊ�
        float px1 = S2_Field.ToWorldX(currentPos.x);
        float pz1 = S2_Field.ToWorldZ(currentPos.z);
        // �ړI�n�̃O���b�h���W�����[���h���W�ɕϊ�
        float px2 = S2_Field.ToWorldX(newPos.x);
        float pz2 = S2_Field.ToWorldZ(newPos.z);
        // ���̊֐���maxFrame��Ăяo�����ƖړI�n�ɓ��B����
        frame++; // <= �Q�Ɠn���Ȃ̂Ō��̒l���ς��
        //float t = (float)frame / _maxFrame;
        float t = frame / completentFrame;
        // ���̃t���[���ł̈ʒu
        float newX = px1 + (px2 - px1) * t;
        float newZ = pz1 + (pz2 - pz1) * t;

        transform.position = new Vector3(newX, 0, newZ);
        _animator.SetFloat(_hashSpeedPara, _speed, _speedDampTime, Time.deltaTime);
        // �ړ����I������玟�̃}�X�ɗ������Ƃ�Ԃ�
        if(completentFrame <= frame)
        {
            frame = 0;
            transform.position = new Vector3(px2, 0, pz2);
            return newPos;
        }
        // �ړ����I����Ă��Ȃ���Ό��̃}�X�ɂ����ԂƂ������Ƃɂ��ĕԂ�
        return currentPos;
    }

    /// <summary>�w�肵���O���b�h���W�ɍ��킹�Ĉʒu��ύX����</summary>
    public void SetPosition(int xgrid, int zgrid)
    {
        _grid.x = xgrid;
        _grid.z = zgrid;
        transform.position = new Vector3(S2_Field.ToWorldX(xgrid), 0, S2_Field.ToWorldZ(zgrid));
        _newGrid = _grid;
    }

    /// <summary>�C���X�y�N�^�[�̒l���ς�����Ƃ��ɌĂяo�����C�x���g�֐�</summary>
    void OnValidate()
    {
        if (_grid.x != S2_Field.ToGridX(transform.position.x) || _grid.z != S2_Field.ToGridZ(transform.position.z))
        {
            transform.position = new Vector3(S2_Field.ToWorldX(_grid.x), 0, S2_Field.ToWorldZ(_grid.z));
        }
        if (_direction != DirUtil.RotationToDir(transform.rotation))
        {
            Debug.LogWarning("���������������ĂȂ���I");
        }
    }

    /// <summary>�w�肵�������ɍ��킹�ĉ�]�x�N�g�����ύX����</summary>
    public void SetDirection(EDir d)
    {
        _direction = d;
        transform.rotation = DirUtil.DirToRotation(d);
    }

    /// <summary>���s�A�j���[�V�����J�n</summary>
    public void Walk()
    {
        if (_currentFrame > 0) return;
        S2_Message.add(_direction.ToString());
    }

    /// <summary>�ړ��J�n�ł��邩�ǂ���</summary>
    public bool IsMoveBegin()
    {
        _newGrid = DirUtil.Move(GetComponentInParent<S2_Field>(), _grid, _direction);
        if (_grid.Equals(_newGrid)) return false;
        return true;
    }
}
