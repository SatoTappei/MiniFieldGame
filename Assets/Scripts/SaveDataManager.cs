using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary>
/// リザルトのセーブやロードを行う
/// </summary>
public static class SaveDataManager
{
    /// <summary>ランキング用のスコアを配列で保存するためのラッパー</summary>
    [System.Serializable]
    public class ResultData
    {
        public List<Result> _results = new List<Result>();
    }

    /// <summary>プレイヤーのスコアのクラス</summary>
    [System.Serializable]
    public class Result
    {
        public string _name;
        public int _score;

        public Result(string name, int score)
        {
            _name = name;
            _score = score;
        }
    }

    /// <summary>ランキングの表示に使う読み込んだリザルトのデータ</summary>
    static ResultData _resultData;

    /// <summary>現在いくつリザルトが保存されているかを返す</summary>
    public static int GetDataCount() => _resultData._results.Count;
    /// <summary>指定した順位のリザルトを返す(1位 = 1)</summary>
    public static Result GetRankResult(int rank) => _resultData._results[rank - 1];

    /// <summary>今回のリザルトのセーブを行う</summary>
    public static void Save(string name, int score)
    {
        // 今回のデータをリストに格納する
        _resultData._results.Add(new Result(name, score));
        // 上位5つのデータのみ抜き出して保存する
        _resultData._results = _resultData._results.OrderByDescending(r => r._score).Take(5).ToList();

        string json = JsonUtility.ToJson(_resultData);
        using (StreamWriter sw = new StreamWriter(Application.dataPath + "/saveData.json"))
        {
            sw.Write(json);
            sw.Flush();
            sw.Close();
        }
    }

    /// <summary>セーブデータからランキング用のリザルトをロードする</summary>
    public static void Load()
    {
        // ロード→セーブの順で行うのでこのタイミングでnewする
        _resultData = new ResultData();
        
        try
        {
            using (StreamReader sr = new StreamReader(Application.dataPath + "/saveData.json"))
            {
                string str = sr.ReadToEnd();
                _resultData = JsonUtility.FromJson<ResultData>(str);
            }
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("セーブデータのファイルが見つかりませんでした");
        }
    }
}