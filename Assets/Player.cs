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
    public int _level = 1;
    public int _food = 99;
    MessageWindow _messageWindow;
    public Action NowAction { get; private set; } = Action.None;
    public bool DoWaitEvent { get; set; } = false;
    public MessageWindow MessageWindow {
        get => _messageWindow != null ? _messageWindow : (_messageWindow = MessageWindow.Find());
    }

    void Start()
    {
        var playerUI = FindObjectOfType<PlayerUI>();
        playerUI.Set(this);

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
            UpdateFood();
            NowAction = Action.None;

            // イベントを確認
            CheckEvent();
            yield return new WaitWhile(() => DoWaitEvent);
        }
    }

    void UpdateFood()
    {
        _food--;
        if(_food <= 0)
        {
            _food = 0;
            _life--;
            MessageWindow.AppendMessage($"Food Damage!! Life({_life}) - 1");
            if(_life <= 0)
            {
                Dead();
            }
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
        // 敵の移動はこのメソッド内で行う、イベントの1つとしてマップ上の敵を動かす
        DoWaitEvent = false;
        StartCoroutine(RunEvents());
    }

    IEnumerator RunEvents()
    {
        // 敵の移動処理
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            enemy.MoveStart();
        }
        // 全ての敵が移動完了するまで待つ
        yield return new WaitWhile(() => FindObjectsOfType<Enemy>().All(_e => !_e._isNowMoving));
        // 終了処理
        DoWaitEvent = true;
    }

    public override void Dead()
    {
        base.Dead();

        var mapManager = FindObjectOfType<MapSceneManager>();
        mapManager._gameOver.SetActive(true);
    }

    public override bool AttackTo(MapObjectBase other)
    {
        MessageWindow.AppendMessage($"Attack! Life({other._life}) - {_attack}");
        other._life -= _attack;
        other.Damaged(_attack);
        if (other._life <= 0)
        {
            MessageWindow.AppendMessage($"Enemy Dead! Exp +{other._exp}");
            other.Dead();
            _exp += other._exp;
            if(_exp >= 10)
            {
                LevelUp();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LevelUp()
    {
        _level += 1;
        _life += 5;
        _attack += 1;
        _exp = 0;

        MessageWindow.AppendMessage($"Level UP! {_level - 1} > {_level}");
        MessageWindow.AppendMessage($"Life + 5! Atk + 1!");
    }

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        MessageWindow.AppendMessage($"Damage! Life({_life}) {-damage}");
    }

    protected override void MoveToExistObject(Map.Mass mass, Vector2Int movedPos)
    {
        var otherObject = mass.existObject.GetComponent<MapObjectBase>();
        if(otherObject is Treasure)
        {
            var treasure = (otherObject as Treasure);
            OpenTreasure(treasure, mass, movedPos);
            StartCoroutine(NotMoveCoroutine(movedPos));
            return;
        }

        base.MoveToExistObject(mass, movedPos);
    }

    protected void OpenTreasure(Treasure treasure, Map.Mass mass, Vector2Int movedPos)
    {
        MessageWindow.AppendMessage($"Open Treasure!!");
        switch (treasure._currentType)
        {
            case Treasure.Type.LifeUp:
                _life += treasure._value;
                MessageWindow.AppendMessage($"Life Up! +{treasure._value}");
                break;
            case Treasure.Type.FoodUp:
                _food += treasure._value;
                MessageWindow.AppendMessage($"Food Up! +{treasure._value}");
                break;
            case Treasure.Type.Weapon:
                // 装備中の武器の攻撃力に足し合わせる
                MessageWindow.AppendMessage($"Charge Weapon! +{treasure.CurrentWeapon}");
                var newWeapon = treasure.CurrentWeapon.Merge(CurrentWeapon);
                CurrentWeapon = newWeapon;
                break;
            default: throw new NotImplementedException();
        }

        // 宝箱を開けたらマップから削除する
        mass.existObject = null;
        mass.type = MassType.Road;
        Destroy(treasure.gameObject);
    }
}
