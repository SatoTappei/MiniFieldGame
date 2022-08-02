using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_PlayerMovement : MonoBehaviour
{
    public Animator _animator;
    public Pos2D _grid;
    public EDir _direction = EDir.Up; // ★ プレイヤーの向きを現す、重要そう
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
    /// 入力されたキーに対応する向きを返す
    /// 何も入力されていなければPause(Nullに該当)を返す
    /// </summary>
    EDir KeyToDir()
    {
        // 毎フレーム下4つの判定をするのは無駄なので、キーが入力されていない時はPauseを返す
        if (!Input.anyKey) return EDir.Pause;

        if (Input.GetKey(KeyCode.LeftArrow)) return EDir.Left;
        if (Input.GetKey(KeyCode.UpArrow)) return EDir.Up;
        if (Input.GetKey(KeyCode.RightArrow)) return EDir.Right;
        if (Input.GetKey(KeyCode.DownArrow)) return EDir.Down;
        
        // 方向キー以外が押されているときもPauseを返す
        return EDir.Pause;
    }

    /// <summary>引数で与えられた向きに対応する回転のベクトルを返す</summary>
    Quaternion DirToRotation(EDir d)
    {
        // 上を基準として時計回り
        // Euler関数…角度を指定することで回転ベクトルに変換してくれる
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

    /// <summary>グリッド座標をワールド座標に変換 ワールド座標 = グリッド座標 * 2</summary>
    float ToWorldX(int xgrid) => xgrid * 2;
    float ToWorldZ(int zgrid) => zgrid * 2;

    /// <summary>ワールド座標をグリッド座標に変換 グリッド座標 = ワールド座標 / 2　して小数点以下を切り捨て</summary>
    int ToGridX(float xworld) => Mathf.FloorToInt(xworld / 2);
    int ToGridZ(float zworld) => Mathf.FloorToInt(zworld / 2);

    /// <summary>補完で計算して進む</summary>
    Pos2D Move(Pos2D currentPos, Pos2D newPos, ref int frame)
    {
        // 現在のグリッド座標をワールド座標に変換
        float px1 = ToWorldX(currentPos.x);
        float pz1 = ToWorldZ(currentPos.z);
        // 目的地のグリッド座標をワールド座標に変換
        float px2 = ToWorldX(newPos.x);
        float pz2 = ToWorldZ(newPos.z);
        // この関数がmaxFrame回呼び出されると目的地に到達する
        frame++; // <= 参照渡しなので元の値が変わる
        float t = (float)frame / _maxFrame;
        // このフレームでの位置
        float newX = px1 + (px2 - px1) * t;
        float newZ = pz1 + (pz2 - pz1) * t;

        transform.position = new Vector3(newX, 0, newZ);
        _animator.SetFloat(_hashSpeedPara, _speed, _speedDampTime, Time.deltaTime);
        // 移動が終わったら次のマスに来たことを返す
        if(_maxFrame == frame)
        {
            frame = 0;
            return newPos;
        }
        // 移動が終わっていなければ元のマスにいる状態ということにして返す
        return currentPos;
    }

    /// <summary>現在の座標と移動したい方向を渡すと移動先の座標を取得</summary>
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
