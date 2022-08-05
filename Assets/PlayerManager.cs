using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�𐧌䂷��
/// </summary>
public class PlayerManager : ActorBase
{
    void OnEnable()
    {
        // PlaySceneManager��State�Ő��䂷�邽�߂Ɏ��g��o�^���Ă���
        FindObjectOfType<PlaySceneManager>().SetPlayer(this);
    }

    void Start()
    {

    }

    void Update()
    {
        // TODO:�{�^�����͂�A������ƈړ��ł��Ȃ��Ȃ�̂Œ����c�Ƃ������^�[���̊T�O�����

        Direction inpugDir = GetKeyToDir();
        // �s�����ł͂Ȃ����ɁA�㉺���E�̓��͂��������ꍇ
        if (!_inAction && inpugDir != Direction.Neutral)
        {
            // �s�����ɂ���
            _inAction = true;
            // �ړ���̍��W���擾
            PosXZ tartgetPosXZ = GetTargetTile(inpugDir);
            // �ڕW�̍��W�Ɍ����ړ�������
            transform.rotation = Quaternion.Euler(0, (float)inpugDir, 0);
            StartCoroutine(Move(tartgetPosXZ));
        }
    }

    /// <summary>
    /// MapGenerator�Ń}�b�v�������A�v���C���[�̔z�u�ꏊ�����܂�����
    /// ���[���h���W���^�C����̍��W�ɕϊ����ăZ�b�g����
    /// </summary>
    public void InitPosXZ()
    {
        _currentPosXZ.x = (int)transform.position.x;
        _currentPosXZ.z = (int)transform.position.z;
    }

    ///// <summary>�v���C���[�̃^�[�����n�܂�����Ă΂�鏈��</summary>
    //void TurnStart()
    //{
    //    Debug.Log("�v���C���[�^�[���J�n");
    //    StartCoroutine(Action());
    //}

    ///// <summary>�v���C���[���s��������</summary>
    //IEnumerator Action()
    //{
    //    // TODO:�s���̏����������A"�ړ�"��������"�U��"��������v���C���[�̍s���͏I��
    //    yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Space)); // �e�X�g Space�L�[�������܂ő҂�
    //    PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
    //   // psm.EndPlayerTurn = true;
    //}

    /// <summary>�w�肵�����W�ɕ⊮���ړ�������</summary>
    IEnumerator Move(PosXZ target)
    {
        Vector3 currentPos = new Vector3(_currentPosXZ.x, 0, _currentPosXZ.z);
        Vector3 targetPos = new Vector3(target.x, 0, target.z);

        int count = 0;
        while (transform.position != targetPos)
        {
            float value = count / MoveTileTime;
            transform.position = Vector3.Lerp(currentPos, targetPos, value);
            yield return null;
            count++;
        }

        // �ړ������������猻�݂̃^�C����̈ʒu���ړ���̍��W�ɕύX����
        _currentPosXZ = target;
    }

    /// <summary>���͂ɑΉ������L�����N�^�[�̌�����Ԃ�</summary>
    Direction GetKeyToDir()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        if (vert > 0) return Direction.Up;
        else if (vert < 0) return Direction.Down;
        else if (hori > 0) return Direction.Right;
        else if (hori < 0) return Direction.Left;
        
        return Direction.Neutral;
    }

    /// <summary>���݂̍��W�ƕ�������ړ���̍��W���擾</summary>
    PosXZ GetTargetTile(Direction dir)
    {
        PosXZ target = _currentPosXZ;

        if (dir == Direction.Up) target.z++;
        else if (dir == Direction.Down) target.z--;
        else if (dir == Direction.Right) target.x++;
        else if (dir == Direction.Left) target.x--;

        return target;
    }

    /// <summary>�^�[���̍ŏ��ɌĂ΂�鏈��</summary>
    public override void TurnInit()
    {
        Debug.Log(gameObject.name + " �^�[���̏��߂ɏ��������܂�");
    }

    /// <summary>�L�[���͑҂����ɌĂ΂�鏈��</summary>
    public override void StandBy()
    {
        Debug.Log(gameObject.name + " �L�[�̓��͑҂��ł�");

        // TODO:�����A�ړ��̃L�[�ƍU���̃L�[�ɒ���
        if (Input.GetKeyDown(KeyCode.M))
        {
            FindObjectOfType<PlaySceneManager>().PushKey = KeyCode.M;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            FindObjectOfType<PlaySceneManager>().PushKey = KeyCode.A;
        }
    }

    /// <summary>�L�����N�^�[���ړ����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public override void MoveStart()
    {
        Debug.Log(gameObject.name + " �ړ��J�n���܂�");
    }

    /// <summary>�L�����N�^�[���ړ����ɌĂ΂�鏈��</summary>
    public override void Move()
    {
        Debug.Log(gameObject.name + " �ړ����ł�");
    }

    /// <summary>�L�����N�^�[���ړ����I�����Ƃ��ɌĂ΂�鏈��</summary>
    public override void MoveEnd()
    {
        Debug.Log(gameObject.name + " �ړ����I���܂���");
    }

    /// <summary>�L�����N�^�[���s�����J�n����Ƃ��ɌĂ΂�鏈��</summary>
    public override void ActionStart()
    {
        Debug.Log(gameObject.name + " �s�����J�n���܂�");
    }

    /// <summary>�L�����N�^�[���s�����ɌĂ΂�鏈��</summary>
    public override void Action()
    {
        Debug.Log(gameObject.name + " �s�����ł�");
    }

    /// <summary>�L�����N�^�[���s�����I����Ƃ��ɌĂ΂�鏈��</summary>
    public override void ActionEnd()
    {
        Debug.Log(gameObject.name + " �s�����I���܂���");
    }
}
