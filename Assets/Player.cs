using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// プレイヤー制御する
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
            // カメラの位置をプレイヤーからの相対位置に設定する
            camera.transform.position = transform.position + _cameraDirection.normalized * _cameraDistance;
            camera.transform.LookAt(transform.position);
            yield return null;
        }
    }

    IEnumerator ActionCoroutine()
    {
        while (true)
        {
            // 入力待ち
            StartCoroutine(WaitInput());
            yield return new WaitWhile(() => NowAction == Action.None);
            // アクションの実行
            switch (NowAction)
            {
                case Action.MoveUp:
                case Action.MoveDown:
                case Action.MoveRight:
                case Action.MoveLeft:
                    Move(ToDirection(NowAction));
                    // アクションが終わるまで待つ
                    yield return new WaitWhile(() => _isNowMoving);
                    break;
            }
            NowAction = Action.None;

            // イベントを確認
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
        // キーの入力確認
        while(NowAction == Action.None)
        {
            yield return null;
            // 入力されたキーの確認
            if (Input.GetKeyDown(KeyCode.UpArrow)) NowAction = Action.MoveUp;
            if (Input.GetKeyDown(KeyCode.DownArrow)) NowAction = Action.MoveDown;
            if (Input.GetKeyDown(KeyCode.RightArrow)) NowAction = Action.MoveRight;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) NowAction = Action.MoveLeft;
        }
    }

    void CheckEvent()
    {
        // TODO:拡張する
        DoWaitEvent = false;
    }
}
