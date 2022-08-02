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
    //public int _maxFrame = 100;
    public float maxPerFrame = 1.67f;
    float completentFrame;

    int _currentFrame = 0;
    Pos2D _newGrid = null;

    readonly int _hashSpeedPara = Animator.StringToHash("Speed");

    void Start()
    {
        completentFrame = maxPerFrame / Time.deltaTime;
    }

    void Update()
    {
        if (_currentFrame == 0)
        {
            EDir d = DirUtil.KeyToDir();
            if (d == EDir.Pause)
                _animator.SetFloat(_hashSpeedPara, 0.0f, _speedDampTime, Time.deltaTime);
            else
            {
                _direction = d;
                S2_Message.add(_direction.ToString());
                transform.rotation = DirUtil.DirToRotation(_direction);
                _newGrid = DirUtil.Move(GetComponentInParent<S2_Field>(), _grid, _direction);
                _grid = Move(_grid, _newGrid, ref _currentFrame);
            }
        }
        else _grid = Move(_grid, _newGrid, ref _currentFrame);
    }

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
}
