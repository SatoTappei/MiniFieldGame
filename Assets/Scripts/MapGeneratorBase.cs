using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップを生成するスクリプトの抽象クラス
/// </summary>
public abstract class MapGeneratorBase : MonoBehaviour
{
    /// <summary>デバッグ用にRキーでシーンの再読み込みをさせるかどうか</summary>
    [SerializeField] bool isDebug;

    void Start()
    {
        
    }

    void Update()
    {
        // マップをリロードするテスト
        if (Input.GetKeyDown(KeyCode.R) && isDebug)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    public abstract string GenerateRandomMap(int width, int height);

    /// <summary>二次元配列を文字列にして返す</summary>
    protected string ArrayToString(string[,] array)
    {
        string str = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
                str += array[i, j];
            if (i < array.GetLength(0) - 1)
                str += '\n';
        }
        return str;
    }
}
