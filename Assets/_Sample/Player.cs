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
    /// <summary>周りのマスが見える範囲</summary>
    [Range(1, 10)] public int VisibleRange = 5;
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

        UpdateVisibleMass();
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

            UpdateVisibleMass();

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

        // ゴール判定
        var mass = Map[_pos.x, _pos.y];
        if(mass.type == MassType.Goal)
        {
            StartCoroutine(Goal());
        }
        else
        {
            // 終了処理
            DoWaitEvent = false;
        }
    }

    IEnumerator Goal()
    {
        // ゴール時にウェイトを入れたければ使う
        yield return new WaitForSeconds(0.0f);

        var mapSceneManager = FindObjectOfType<MapSceneManager>();
        mapSceneManager.GenerateMap();

        var player = FindObjectOfType<Player>();
        player._life = _life;
        player._attack = _attack;
        player._food = _food;
        player._exp = _exp;
        player._level = _level;
        player.CurrentWeapon = CurrentWeapon;
        
        // 処理の都合上、Attackが上がるのでそれを打ち消すためのもの
        if(CurrentWeapon != null)
        {
            player._attack -= CurrentWeapon._attack;
        }

        var saveData = new SaveData();
        saveData._level = _level;
        saveData._life = _life;
        saveData._attack = _attack;
        saveData._exp = _exp;
        if(CurrentWeapon != null)
        {
            saveData._attack -= CurrentWeapon._attack;
            saveData._weaponName = CurrentWeapon._name;
            saveData._weaponAttack = CurrentWeapon._attack;
        }
        else
        {
            saveData._weaponName = "";
            saveData._weaponAttack = 0;
        }
        saveData._mapData = Map.MapData;
        saveData.Save();
    }

    public override void Dead()
    {
        base.Dead();

        var mapManager = FindObjectOfType<MapSceneManager>();
        mapManager._gameOver.SetActive(true);

        SaveData.Destroy();
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
        else if(otherObject is Trap)
        {
            var trap = (otherObject as Trap);
            StampTrap(trap, mass, movedPos);
            StartCoroutine(NotMoveCoroutine(movedPos));
            return;
        }

        base.MoveToExistObject(mass, movedPos);
    }

    protected void StampTrap(Trap trap, Map.Mass mass, Vector2Int movedPos)
    {
        MessageWindow.AppendMessage($"!! Trap !!");
        switch (trap._currentType)
        {
            case Trap.Type.LifeDown:
                _life -= trap._value;
                MessageWindow.AppendMessage($"LifeDown.. -{trap._value}");
                break;
            case Trap.Type.FoodDown:
                _food -= trap._value;
                MessageWindow.AppendMessage($"FoodDown.. -{trap._value}");
                break;
            default: throw new NotImplementedException();
        }

        // 罠はマップから削除する
        mass.existObject = null;
        mass.type = MassType.Road;
        Destroy(trap.gameObject);
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

    void UpdateVisibleMass()
    {
        var map = Map;
        var startPos = _pos - Vector2Int.one * VisibleRange;
        var endPos = _pos + Vector2Int.one * VisibleRange;
        for(var y = startPos.y; y <= endPos.y; ++y)
        {
            if (y < 0) continue;
            if (Map.MapSize.y <= y) break;

            for (var x = startPos.x; x <= endPos.x; ++x)
            {
                if (x < 0) continue;
                if (map.MapSize.x <= x) break;
                map[x, y].Visible = true;
            }
        }
    }

    public void Recover(SaveData saveData)
    {
        CurrentWeapon = null;
        _level = saveData._level;
        _life = saveData._life;
        _attack = saveData._attack;
        _exp = saveData._exp;
        if(saveData._weaponName != "")
        {
            var weapon = ScriptableObject.CreateInstance<Weapon>();
            weapon._name = saveData._weaponName;
            weapon._attack = saveData._weaponAttack;
            CurrentWeapon = weapon;
        }
    }
}
