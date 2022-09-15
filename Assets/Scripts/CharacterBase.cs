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
        Obstacle,
    }

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
    protected ActorDir _inputDir;
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
    protected ActorDir GetKeyToDir(float vert, float hori)
    {
        if (vert == 1) return ActorDir.Up;
        else if (vert == -1) return ActorDir.Down;
        else if (hori == 1) return ActorDir.Right;
        else if (hori == -1) return ActorDir.Left;

        return ActorDir.Neutral;
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

    /// <summary>方向に対応したVector3型を返す</summary>
    protected Vector3 DirectionToVec3(ActorDir dir)
    {
        if (dir == ActorDir.Up) return Vector3.forward;
        else if (dir == ActorDir.Down) return Vector3.back;
        else if (dir == ActorDir.Right) return Vector3.right;
        else if (dir == ActorDir.Left) return Vector3.left;

        // どれにも該当しないならx/z方向ではなくて上向きのベクトルを返す
        return Vector3.up;
    }

    /// <summary>死んだときの演出を行う</summary>
    public void Death(ActorDir attackedDir)
    {
        // 死亡のアニメーションを再生(スケールを0にして見えなくする)
        _anim.Play("Dead");
        // 被ダメージのエフェクト、吹き出た血の表示と血だまりの生成
        _damageEffect.SetActive(true);
        Instantiate(_decalEffect, new Vector3(transform.position.x, 0.15f, transform.position.z), Quaternion.Euler(90, 0, 0));
        // ラグドールを生成して攻撃された方向とは逆に吹っ飛ばす
        var Obj = Instantiate(_ragDoll, transform.position, Quaternion.identity);
        Vector3 vec = DirectionToVec3(attackedDir);
        Obj.GetComponent<RagDollController>().Dir = new Vector3(vec.x, 0.5f, vec.z).normalized;
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

    /// <summary>ターン終了時に呼ばれる処理</summary>
    public abstract void TurnEnd();

    /// <summary>このキャラクターがダメージを受けたときに呼ばれる処理</summary>
    public abstract void Damaged(ActorDir attackedDir);
}
