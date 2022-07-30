using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// マップ上のオブジェクトの基底クラス
/// </summary>
public class MapObjectBase : MonoBehaviour
{
    public enum Group
    {
        Player,
        Enemy,
        Other,
    }

    [Range(0, 100)] public float _moveSecond = 0.1f;
    public bool _isNowMoving { get; private set; } = false;
    public Vector2Int _pos;
    public Vector2Int _prevPos { get; protected set; }
    public Direction _forward;
    public int _life = 5;
    public int _attack = 2;
    public int _exp = 0;
    public Group _currentGroup = Group.Other;
    [SerializeField] Weapon _weapon;
    Map _map;
    bool _visible = true;
    public bool Visible
    {
        get => _visible;
        set
        {
            _visible = value;
            foreach(var renderer in GetComponents<Renderer>().Concat(GetComponentsInChildren<Renderer>()))
            {
                renderer.enabled = value;
            }
        }
    }
    public Map Map { get => _map != null ? _map : (_map = FindObjectOfType<Map>()); }
    /// <summary>装備する際に設定処理が必要なのでプロパティ経由で設定する</summary>
    public Weapon CurrentWeapon
    {
        get => _weapon;
        set
        {
            if(_weapon != null)
            {
                _weapon.Detach(this);
            }
            _weapon = value;
            if(_weapon != null)
            {
                _weapon.Attach(this);
            }
        }
    }
    
    void Awake()
    {
        if(CurrentWeapon != null)
        {
            CurrentWeapon.Attach(this);
        }
    }

    /// <summary>位置と前方向を設定するメソッド</summary>
    public void SetPosAndForward(Vector2Int pos, Direction forward)
    {
        _prevPos = Vector2Int.one * -1;
        _pos = pos;
        _forward = forward;

        transform.position = Map.CalcMapPos(_pos);
    }

    /// <summary>移動処理</summary>
    public virtual void Move(Direction dir)
    {
        _isNowMoving = false;
        var (movedMass, movedPos) = Map.GetMovePos(_pos, dir);
        if (movedMass == null) return;

        var massData = Map[movedMass.type];
        if (movedMass.existObject)
        {
            MoveToExistObject(movedMass, movedPos);
        }
        else if (massData.isRoad)
        {
            MoveToRoad(movedMass, movedPos);
        }
        else
        {
            MoveToNotMoving(movedMass, movedPos);
        }
    }

    protected virtual void MoveToExistObject(Map.Mass mass, Vector2Int movedPos)
    {
        var otherObject = mass.existObject.GetComponent<MapObjectBase>();
        if (IsAttackableObject(this, otherObject))
        {
            if (AttackTo(otherObject))
            {
                // 攻撃の結果相手を倒したらそのマスに移動する
                StartCoroutine(MoveCoroutine(movedPos));
                return;
            }
        }

        StartCoroutine(NotMoveCoroutine(movedPos));
    }

    protected virtual void MoveToRoad(Map.Mass mass,Vector2Int movedPos)
    {
        StartCoroutine(MoveCoroutine(movedPos));
    }

    protected virtual void MoveToNotMoving(Map.Mass mass, Vector2Int movedPos)
    {
        StartCoroutine(NotMoveCoroutine(movedPos));
    }

    /// <summary>対象が攻撃可能か</summary>
    public static bool IsAttackableObject(MapObjectBase self,MapObjectBase other)
    {
        return self._currentGroup != other._currentGroup
            && (self._currentGroup != Group.Other && other._currentGroup != Group.Other);
    }

    public virtual bool AttackTo(MapObjectBase other)
    {
        other._life -= _attack;
        other.Damaged(_attack);
        if(other._life <= 0)
        {
            other.Dead();
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Damaged(int damage)
    {

    }

    public virtual void Dead()
    {
        Destroy(gameObject);
    }

    IEnumerator MoveCoroutine(Vector2Int target)
    {
        // マップのマス情報を更新する
        var startMass = Map[_pos.x, _pos.y];
        startMass.existObject = null;
        var movedPos = Map.CalcMapPos(target);
        _prevPos = _pos;
        _pos = target;
        var movedMass = Map[_pos.x, _pos.y];
        movedMass.existObject = gameObject;
        Visible = movedMass.Visible;

        // モデルの移動処理
        _isNowMoving = true;
        var start = transform.position;
        var timer = 0f;
        while(timer < _moveSecond)
        {
            yield return null;
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(start, movedPos, timer / _moveSecond);
        }
        transform.position = movedPos;
        _isNowMoving = false;
    }

    protected IEnumerator NotMoveCoroutine(Vector2Int target)
    {
        var movedPos = Map.CalcMapPos(target);

        _isNowMoving = true;
        var start = transform.position;
        var timer = 0f;
        movedPos = Vector3.Lerp(start, movedPos, 0.5f);
        while(timer < _moveSecond)
        {
            yield return null;
            timer += Time.deltaTime;
            var t = 1.0f - Mathf.Abs(timer / _moveSecond * 2 - 1.0f);
            transform.position = Vector3.Lerp(start, movedPos, t);
        }
        transform.position = start;
        _isNowMoving = false;
    }
}
