using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップに登場するキャラクターの基底クラス
/// </summary>
public abstract class CharacterBase : ActorBase
{
    
    /// <summary>キャラクターの種類</summary>
    public enum CharacterType
    {
        Player,
        Enemy,
        // Obstacle,
    }

    /// <summary>キャラクターの方向</summary>
    public enum Direction
    {
        Neutral = 360, // 何も入力されていない状態
        Up = 0,
        Down = 180,
        Right = 90,
        Left = 270,
    };

    [SerializeField] protected Animator _anim;
    /// <summary>キャラクターが次のタイルに移動するのにかかる時間</summary>
    protected const float MoveTileTime = 15.0f;
    /// <summary>このキャラクターの種類</summary>
    [SerializeField] CharacterType _characterType;
    /// <summary>このキャラクターが侵入できるタイル</summary>
    [SerializeField] TileType[] _canMoveTile;
    /// <summary>被ダメージ時のエフェクト</summary>
    [SerializeField] protected GameObject _damageEffect;
    /// <summary>被ダメージ時のDecalエフェクト</summary>
    [SerializeField] protected GameObject _decalEffect;
    /// <summary>キャラクターが死んだときに出るラグドール</summary>
    [SerializeField] protected GameObject _ragDoll;
    /// <summary>現在のキャラクターの向き</summary>
    //Direction _currentDir = Direction.Up;
    /// <summary>入力された方向、キャラクターの移動に使用する。敵の場合は自動で決まる</summary>
    protected Direction _inputDir;
    /// <summary>移動する際の移動先の座標</summary>
    protected PosXZ _targetPosXZ;
    /// <summary>行動中かどうか</summary>
    protected bool _inAction;
    
    /// <summary>このキャラクターの種類を返す</summary>
    public CharacterType GetCharacterType() => _characterType;

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// MapGeneratorでマップ生成時、配置場所が決まったら
    /// ワールド座標をタイル上の座標に変換してセットする
    /// </summary>
    public override void InitPosXZ()
    {
        _currentPosXZ.x = (int)transform.position.x;
        _currentPosXZ.z = (int)transform.position.z;

        // 生成された座標に自身をセットして攻撃や移動の判定に使えるようにする
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileCharacter(_currentPosXZ.x, _currentPosXZ.z, this);
    }

    /// <summary>入力に対応したキャラクターの向きを返す</summary>
    protected Direction GetKeyToDir(float vert, float hori)
    {
        if (vert == 1) return Direction.Up;
        else if (vert == -1) return Direction.Down;
        else if (hori == 1) return Direction.Right;
        else if (hori == -1) return Direction.Left;

        return Direction.Neutral;
    }

    /// <summary>指定した座標に補完しつつ移動させる</summary>
    protected IEnumerator Move(PosXZ target)
    {
        //MapManager mm = FindObjectOfType<MapManager>();
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

        // 移動が完了したら現在のタイル上の位置を移動先の座標に変更する
        _currentPosXZ = target;
        // 移動が完了したら自分が最後に移動完了したキャラクターかを確認してもらう
        FindObjectOfType<PlaySceneManager>().CheckRemMoveActor();
    }

    /// <summary>現在の座標と方向から移動先の座標を取得</summary>
    protected PosXZ GetTargetTile(Direction dir)
    {
        PosXZ target = _currentPosXZ;

        if (dir == Direction.Up) target.z++;
        else if (dir == Direction.Down) target.z--;
        else if (dir == Direction.Right) target.x++;
        else if (dir == Direction.Left) target.x--;
        
        // 移動先が壁なら現在の位置を返す、その際はターンを進めないようにする
        return target;
    }

    /// <summary>方向に対応したVector3型を返す</summary>
    protected Vector3 DirectionToVec3(Direction dir)
    {
        if (dir == Direction.Up) return Vector3.forward;
        else if (dir == Direction.Down) return Vector3.back;
        else if (dir == Direction.Right) return Vector3.right;
        else if (dir == Direction.Left) return Vector3.left;

        // どれにも該当しないならx/z方向ではなくて上向きのベクトルを返す
        return Vector3.up;
    }

    /// <summary>ターンの最初に呼ばれる処理</summary>
    public abstract void TurnInit();

    /// <summary>キー入力待ち中に呼ばれる処理</summary>
    public abstract void StandBy();

    /// <summary>キャラクターが移動を開始するときに呼ばれる処理</summary>
    public abstract void MoveStart();

    /// <summary>キャラクターが移動中に呼ばれる処理</summary>
    public abstract void Move();

    /// <summary>キャラクターが移動を終えたときに呼ばれる処理</summary>
    public abstract void MoveEnd();

    /// <summary>キャラクターが行動を開始するときに呼ばれる処理</summary>
    public abstract void ActionStart();

    /// <summary>キャラクターが行動中に呼ばれる処理</summary>
    public abstract void Action();

    /// <summary>キャラクターが行動を終えるときに呼ばれる処理</summary>
    public abstract void ActionEnd();

    /// <summary>このキャラクターがダメージを受けたときに呼ばれる処理</summary>
    public abstract void Damaged(Direction attackedDir);
}
