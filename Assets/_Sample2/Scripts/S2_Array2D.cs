using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_Array2D
{
    public int width;
    public int height;
    int[,] data;

    public S2_Array2D(int w, int h)
    {
        width = w;
        height = h;
        data = new int[width, height];
    }

    /// <summary>X/Z座標にある値を取得する</summary>
    public int Get(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return data[x, z];
        }
        return -1;
    }

    /// <summary>X/Z座標に値(v)を設定する</summary>
    public int Set(int x, int z, int v)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            data[x, z] = v;
            return v;
        }
        return -1;
    }
}