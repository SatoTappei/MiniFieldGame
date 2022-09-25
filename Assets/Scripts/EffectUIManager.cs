using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// ゲーム開始、ステージクリア、ゲームオーバーの演出用のUIを制御する
/// </summary>
public class EffectUIManager : MonoBehaviour
{
    /// <summary>ゲームスタートの演出に使うオブジェクト達の親</summary>
    [SerializeField] Transform _gameStartEffect;
    /// <summary>ステージクリアの演出に使うオブジェクト達の親</summary>
    [SerializeField] Transform _stageClearEffect;
    /// <summary>ゲームオーバー時の演出に使うオブジェクトの達の親</summary>
    [SerializeField] GameObject _gameOverEffect;
    /// <summary>"次のステージへ"のボタン</summary>
    [SerializeField] GameObject _nextStageButton;
    /// <summary>"リトライする"のボタン</summary>
    [SerializeField] GameObject _retryButton;

    void Awake()
    {
        _gameStartEffect.localScale = Vector3.zero;
        _stageClearEffect.gameObject.SetActive(false);
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
        // 各テキストにステージの情報を表示
        _gameStartEffect.GetChild(0).GetChild(1).GetComponent<Text>().text = stageNum.ToString();
        _gameStartEffect.GetChild(1).GetChild(1).GetComponent<Text>().text = so.MaxCoin.ToString();
        _gameStartEffect.GetChild(2).GetChild(1).GetComponent<Text>().text = so.MaxEnemy.ToString();
        _gameStartEffect.GetChild(3).GetChild(1).GetComponent<Text>().text = so.TurnLimit.ToString();
        // パネルをアニメーションさせて表示させる
        Sequence sequence1 = DOTween.Sequence();
        sequence1.Join(_gameStartEffect.DOScale(new Vector3(1, 0.01f, 1), 0.5f));
        sequence1.Append(_gameStartEffect.DOScale(Vector3.one, 0.15f));
        sequence1.Append(_gameStartEffect.DOShakeScale(0.15f, 0.25f));
        //// アニメーションが始まって1秒後に"スペースキーを押してね"のボタンを表示
        yield return new WaitForSeconds(1.0f);
        _gameStartEffect.GetChild(4).gameObject.SetActive(true);
        //クリックされたらパネルをアニメーションさせて閉じる
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        SoundManager._instance.Play("SE_決定");
        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(_gameStartEffect.DOScale(new Vector3(1.25f, 1.25f, 1), 0.15f));
        sequence2.Append(_gameStartEffect.DOScale(Vector3.zero, 0.25f));
    }

    /// <summary>ステージクリア時の演出</summary>
    public IEnumerator StageClearEffect(int stageNum, StageDataSO so, int coin, int enemy, int turn, int score)
    {
        _stageClearEffect.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_nextStageButton);
        // 最後のステージか否かで表示する内容を切り替える
        string label = GameManager._instance.CheckAllClear() ? "リザルトへ" : "次のステージへ";
        _stageClearEffect.GetChild(5).GetComponentInChildren<Text>().text = label;
        // 現在のステージ番号を表示
        _stageClearEffect.GetChild(0).GetChild(1).GetComponent<Text>().text = stageNum.ToString();
        // 各リザルトをポップさせるアニメーション付きで表示させる
        yield return StartCoroutine(Pop(1, (so.MaxCoin - coin) + " / " + so.MaxCoin));
        yield return StartCoroutine(Pop(2, (so.MaxEnemy - enemy) + " / " + so.MaxEnemy));
        yield return StartCoroutine(Pop(3, (so.TurnLimit - turn) + " / " + so.TurnLimit));
        // スコアは0から現在の値まで加算されるアニメーションで表示する
        Sequence sequence = DOTween.Sequence();
        Transform scoreTrans = _stageClearEffect.GetChild(4).GetChild(1);
        sequence.Join(scoreTrans.GetComponent<Text>().DOCounter(0, score, 1.5f));
        sequence.Append(scoreTrans.DOScale(1.2f, 0.15f));
        sequence.Append(scoreTrans.DOScale(1f, 0.15f));
        sequence.AppendCallback(() => SoundManager._instance.Play("SE_リザルト2"));
        sequence.SetLink(gameObject);

        // スコア以外のリザルトをポップさせるアニメーション
        IEnumerator Pop(int childIndex, string str)
        {
            Transform item = _stageClearEffect.GetChild(childIndex).GetChild(1);
            item.GetComponent<Text>().text = str;
            Sequence sequence = DOTween.Sequence();
            sequence.Join(item.DOScale(1.2f, 0.15f));
            sequence.Append(item.DOScale(1.0f, 0.15f));
            sequence.SetLink(gameObject);
            SoundManager._instance.Play("SE_リザルト1");
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>ゲームオーバー時の演出</summary>
    public IEnumerator GameOverEffect()
    {
        // TODO:ちゃんと作る
        SoundManager._instance.Play("SE_チーン");
        _gameOverEffect.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_retryButton);
        yield return new WaitForSeconds(1.5f);
        SoundManager._instance.Play("SE_がめおべら文字演出");
        _gameOverEffect.transform.GetChild(0).transform.DORotate(new Vector3(0, 0, -8.5f), 0.05f);
        yield return null;
    }

    /// <summary>時間切れでゲームオーバーになった際の演出</summary>
    public IEnumerator TimeOverEffect()
    {
        FindObjectOfType<PlayerManager>().Death(ActorDir.Neutral);
        yield return StartCoroutine(GameOverEffect());
    }
}
