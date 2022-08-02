using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̍s���𐧌䂷��
/// </summary>
public class PlayerController : ActorBase
{
    /// <summary>XZ���ʏ�ł̍��W</summary>
    public struct PosXZ
    {
        public int x;
        public int z;
    }

    /// <summary>���݂�XZ���ʏ�ł̈ʒu</summary>
    PosXZ _currentPosXZ;

    void OnEnable()
    {
        FindObjectOfType<PlaySceneManager>()._playerAction += TurnStart; 
    }

    void Start()
    {

    }

    void Update()
    {
        // TODO:�{�^�����͂�A������ƈړ��ł��Ȃ��Ȃ�̂Œ����c�Ƃ������^�[���̊T�O�����

        ActorDir inpugDir = GetKeyToDir();
        // �s�����ł͂Ȃ����ɁA�㉺���E�̓��͂��������ꍇ
        if (!_inAction && inpugDir != ActorDir.Neutral)
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

    /// <summary>�v���C���[�̃^�[�����n�܂�����Ă΂�鏈��</summary>
    void TurnStart()
    {
        Debug.Log("�v���C���[�^�[���J�n");
        StartCoroutine(Action());
    }

    /// <summary>�v���C���[���s��������</summary>
    IEnumerator Action()
    {
        // TODO:�s���̏����������A"�ړ�"��������"�U��"��������v���C���[�̍s���͏I��
        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Space)); // �e�X�g Space�L�[�������܂ő҂�
        PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
        psm.EndPlayerTurn = true;
    }

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
    ActorDir GetKeyToDir()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        if (vert > 0) return ActorDir.Up;
        else if (vert < 0) return ActorDir.Down;
        else if (hori > 0) return ActorDir.Right;
        else if (hori < 0) return ActorDir.Left;
        
        return ActorDir.Neutral;
    }

    /// <summary>���݂̍��W�ƕ�������ړ���̍��W���擾</summary>
    PosXZ GetTargetTile(ActorDir dir)
    {
        PosXZ target = _currentPosXZ;

        if (dir == ActorDir.Up) target.z++;
        else if (dir == ActorDir.Down) target.z--;
        else if (dir == ActorDir.Right) target.x++;
        else if (dir == ActorDir.Left) target.x--;

        return target;
    }
}
