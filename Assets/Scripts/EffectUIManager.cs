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

    void Start()
    {
        _gameStartEffect.SetActive(false);
        _stageClearEffect.SetActive(false);
        _gameOverEffect.SetActive(false);
    }

    void Update()
    {
        
    }

    /// <summary>ゲーム開始時の演出</summary>
    public IEnumerator GameStartEffect(/*呼び出す側がステージの情報を渡す*/)
    {
        _gameStartEffect.SetActive(true);
        // TODO:ちゃんと演出を作る
        _gameStartEffect.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>().text = 65535.ToString();
        yield return null;
        _gameStartEffect.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Text>().text = 256.ToString();
        yield return null;
        _gameStartEffect.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Text>().text = "いっぱい";
        yield return null;
        _gameStartEffect.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Text>().text = "たくさん";
        yield return new WaitForSeconds(0.5f);
        _gameStartEffect.SetActive(false);
    }

    /// <summary>ステージクリア時の演出</summary>
    public IEnumerator StageClearEffect()
    {
        _stageClearEffect.SetActive(true);
        // TODO:ちゃんと演出を作る
        _stageClearEffect.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>().text = 65535.ToString() + " / " + 65535.ToString();
        yield return null;
        _stageClearEffect.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Text>().text = 256.ToString() + " / " + 256.ToString();
        yield return null;
        _stageClearEffect.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Text>().text = "いっぱい" + " / " + "いっぱい";
        yield return null;
        _stageClearEffect.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Text>().text = "たくさん" + " / " + "たくさん";
        yield return new WaitForSeconds(0.5f);
        _stageClearEffect.SetActive(false);
        yield return null;
    }

    /// <summary>ゲームオーバー時の演出</summary>
    public IEnumerator GameOverEffect()
    {
        _gameOverEffect.SetActive(true);
        yield return null;
    }
}
