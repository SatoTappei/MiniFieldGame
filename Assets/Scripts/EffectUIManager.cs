using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム開始、ステージクリア、ゲームオーバーの演出用のUIを制御する
/// </summary>
public class EffectUIManager : MonoBehaviour
{
    /// <summary>ゲームスタートの演出に使うオブジェクト達の親</summary>
    [SerializeField] GameObject _gameStartEffect;
    /// <summary>ステージクリアの演出に使うオブジェクト達の親</summary>
    [SerializeField] GameObject _stageClearEffect;
    /// <summary>ゲームオーバー時の演出に使うオブジェクトの達の親</summary>
    [SerializeField] GameObject _gameOverEffect;

    void Awake()
    {
        _gameStartEffect.SetActive(false);
        _stageClearEffect.SetActive(false);
        _gameOverEffect.SetActive(false);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>ゲーム開始時の演出</summary>
    public IEnumerator GameStartEffect(int stageNum, StageDataSO so)
    {
        _gameStartEffect.SetActive(true);
        // TODO:ちゃんと演出を作る
        _gameStartEffect.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>().text = stageNum.ToString();
        yield return null;
        _gameStartEffect.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Text>().text = so.MaxCoin.ToString();
        yield return null;
        _gameStartEffect.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Text>().text = so.MaxEnemy.ToString();
        yield return null;
        _gameStartEffect.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Text>().text = so.TurnLimit.ToString();
        yield return new WaitForSeconds(0.5f);
        _gameStartEffect.SetActive(false);
    }

    /// <summary>ステージクリア時の演出</summary>
    public IEnumerator StageClearEffect(int stageNum, StageDataSO so, int coin, int enemy, int turn, int score)
    {
        _stageClearEffect.SetActive(true);
        // TODO:ちゃんと演出を作る
        _stageClearEffect.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>().text = stageNum.ToString();
        yield return null;
        _stageClearEffect.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Text>().text = coin.ToString() + " / " + so.MaxCoin.ToString();
        yield return null;
        _stageClearEffect.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Text>().text = enemy.ToString() + " / " + so.MaxEnemy.ToString();
        yield return null;
        _stageClearEffect.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Text>().text = turn.ToString() + " / " + so.TurnLimit.ToString();
        yield return null;
        _stageClearEffect.transform.GetChild(4).gameObject.transform.GetChild(1).GetComponent<Text>().text = score.ToString();
    }

    /// <summary>ゲームオーバー時の演出</summary>
    public IEnumerator GameOverEffect()
    {
        _gameOverEffect.SetActive(true);
        yield return null;
    }
}
