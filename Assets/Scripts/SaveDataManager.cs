using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// リザルトのセーブやロードを行う
/// </summary>
public static class SaveDataManager
{
    /// <summary>プレイヤーのスコアのクラス</summary>
    [System.Serializable]
    class ScoreData
    {
        public string _name;
        public string _score;
    }
}
