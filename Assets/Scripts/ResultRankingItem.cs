using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面のランキングで各プレイヤーのリザルトを表示するオブジェクト
/// </summary>
public class ResultRankingItem : MonoBehaviour
{
    /// <summary>スコアネームを表示するテキスト</summary>
    [SerializeField] Text _nameText;
    /// <summary>スコアを表示するテキスト</summary>
    [SerializeField] Text _scoreText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>リザルトをセットする</summary>
    public void SetResult(string name, int score)
    {
        _nameText.text = name;
        _scoreText.text = score.ToString();
    }
}
