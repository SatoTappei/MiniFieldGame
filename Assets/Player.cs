using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// �v���C���[���䂷��
/// </summary>
public class Player : MapObjectBase
{
    public enum Action
    {
        None,
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
    }

    [Range(0, 100)] public float _cameraDistance;
    public Vector3 _cameraDirection = new Vector3(0, 10, -3);
    public Action NowAction { get; private set; } = Action.None;
    public bool DoWaitEvent { get; set; } = false;

    void Start()
    {
        StartCoroutine(CameraMove());
        StartCoroutine(ActionCoroutine());
    }

    IEnumerator CameraMove()
    {
        var camera = Camera.main;
        while (true)
        {
            // �J�����̈ʒu���v���C���[����̑��Έʒu�ɐݒ肷��
            camera.transform.position = transform.position + _cameraDirection.normalized * _cameraDistance;
            camera.transform.LookAt(transform.position);
            yield return null;
        }
    }

    IEnumerator ActionCoroutine()
    {
        while (true)
        {
            // ���͑҂�
            StartCoroutine(WaitInput());
            yield return new WaitWhile(() => NowAction == Action.None);
            // �A�N�V�����̎��s
            switch (NowAction)
            {
                case Action.MoveUp:
                case Action.MoveDown:
                case Action.MoveRight:
                case Action.MoveLeft:
                    Move(ToDirection(NowAction));
                    // �A�N�V�������I���܂ő҂�
                    yield return new WaitWhile(() => _isNowMoving);
                    break;
            }
            NowAction = Action.None;

            // �C�x���g���m�F
            CheckEvent();
            yield return new WaitWhile(() => DoWaitEvent);
        }
    }

    Direction ToDirection(Action action)
    {
        switch (action)
        {
            case Action.MoveUp: return Direction.North;
            case Action.MoveDown: return Direction.South;
            case Action.MoveRight: return Direction.East;
            case Action.MoveLeft: return Direction.West;
            default: throw new NotImplementedException();
        }
    }

    IEnumerator WaitInput()
    {
        NowAction = Action.None;
        // �L�[�̓��͊m�F
        while(NowAction == Action.None)
        {
            yield return null;
            // ���͂��ꂽ�L�[�̊m�F
            if (Input.GetKeyDown(KeyCode.UpArrow)) NowAction = Action.MoveUp;
            if (Input.GetKeyDown(KeyCode.DownArrow)) NowAction = Action.MoveDown;
            if (Input.GetKeyDown(KeyCode.RightArrow)) NowAction = Action.MoveRight;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) NowAction = Action.MoveLeft;
        }
    }

    void CheckEvent()
    {
        // TODO:�g������
        DoWaitEvent = false;
    }
}
