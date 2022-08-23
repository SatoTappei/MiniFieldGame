using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    /// <summary>最大ステージ番号</summary>
    [SerializeField] int _maxStageNum;
    /// <summary>現在のステージ番号、最初は1なので注意</summary>
    int _currentStageNum = 1;
    /// <summary>合計スコア</summary>
    int _totalScore;
    /// <summary>フェードに使う画像</summary>
    [SerializeField] Image _fadeImage;
    /// <summary>フェード中かどうか、ゲームが始まる際には必ずフェードから始めるので初期値がtrue</summary>
    bool _isFading = true;

    public int CurrentStageNum { get => _currentStageNum; }
    public bool IsFading { get => _isFading; }
    public int TotalScore { get => _totalScore; }

    /// <summary>ステージをクリアしたのでステージ番号を1つ進める</summary>
    public void AdvanceStageNum() => _currentStageNum = Mathf.Min(++_currentStageNum, _maxStageNum);
    /// <summary>全ステージクリアしたかどうか</summary>
    public bool CheckAllClear() => _currentStageNum == _maxStageNum;
    /// <summary>
    /// 合計スコアを更新する
    /// 現在はステージクリア後に呼ばれて次のステージにスコアを引き継がせる
    /// </summary>
    public void UpdateTotalScore(int score) => _totalScore = score;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        // シーンが読み込まれるたびにフェードインのメソッドを呼ぶ
        SceneManager.activeSceneChanged += FadeIn;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>フェードインする</summary>
    public void FadeIn(Scene _, Scene __)
    {
        Debug.Log("フェードイン");
        Sequence sequence = DOTween.Sequence();
        // TODO:フェードの演出を変える
        sequence.Join(DOTween.ToAlpha(() => _fadeImage.color, color => _fadeImage.color = color, 0f, 1f));
        // TODO:フェードの演出ここまで
        sequence.AppendCallback(() => { _fadeImage.gameObject.SetActive(false);_isFading = false; });
    }

    /// <summary>フェードアウト後、シーンを推移する</summary>
    public void FadeOut(string sceneName)
    {
        Debug.Log("フェードアウト");
        _isFading = true;
        _fadeImage.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        // TODO:フェードの演出を変える
        sequence.Join(DOTween.ToAlpha(() => _fadeImage.color, color => _fadeImage.color = color, 1f, 1f));
        // TODO:フェードの演出ここまで
        sequence.AppendCallback(() => SceneManager.LoadScene(sceneName));
    }
}
