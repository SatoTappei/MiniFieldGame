using UnityEngine;

public static class DirUtil
{
    /// <summary>
    /// 入力されたキーに対応する向きを返す
    /// 何も入力されていなければPause(Nullに該当)を返す
    /// </summary>
    public static EDir KeyToDir()
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
    public static Quaternion DirToRotation(EDir d)
    {
        // 上を基準として時計回り
        // Euler関数…角度を指定することで回転ベクトルに変換してくれる
        Quaternion r = Quaternion.Euler(0, 0, 0);
        switch (d)
        {
            case EDir.Right:
                r = Quaternion.Euler(0, 270, 0); break;
            case EDir.Down:
                r = Quaternion.Euler(0, 0, 0); break;
            case EDir.Left:
                r = Quaternion.Euler(0, 90, 0); break;
            case EDir.Up:
                r = Quaternion.Euler(0, 180, 0); break;
        }
        return r;
    }

    /// <summary>引数で与えられた回転のベクトルに対応する向きを返す</summary>
    public static EDir RotationToDir(Quaternion r)
    {
        float y = r.eulerAngles.y;
        if (y < 45)
        {
            return EDir.Down;
        }
        else if (y < 135)
        {
            return EDir.Left;
        }
        else if (y < 225)
        {
            return EDir.Up;
        }
        else if (y < 315)
        {
            return EDir.Right;
        }

        return EDir.Down;
    }

    /// <summary>現在の座標と移動したい方向を渡すと移動先の座標を取得</summary>
    public static Pos2D GetNewGrid(Pos2D position, EDir d)
    {
        Pos2D newP = new Pos2D();
        newP.x = position.x;
        newP.z = position.z;
        switch (d)
        {
            case EDir.Left:
                newP.x += 1; break;
            case EDir.Up:
                newP.z += -1; break;
            case EDir.Right:
                newP.x += -1; break;
            case EDir.Down:
                newP.z += 1; break;
        }
        return newP;
    }

    /// <summary>
    /// マップのデータ(map)と現在の座標(position)と移動したい方向(d)を渡すと
    /// (もし移動できるならば)移動先の座標を取得
    /// </summary>
    public static Pos2D Move(S2_Field field, Pos2D position, EDir d)
    {
        // 障害物があれば現在のグリッド座標、なければ移動先のグリッド座標を取得する
        Pos2D newP = GetNewGrid(position, d);
        if (field.IsCollide(newP.x, newP.z) || field.GetExistActor(newP.x,newP.z) != null) 
            return position;
        return newP;
    }

    /// <summary>ランダムな向きを返す</summary>
    public static EDir RandomDirection()
    {
        int dirnum = Random.Range(0, 4);
        switch (dirnum)
        {
            case 0:
                return EDir.Left;
            case 1:
                return EDir.Up;
            case 2:
                return EDir.Right;
            case 3:
                return EDir.Down;
        }
        return EDir.Down;
    }
}