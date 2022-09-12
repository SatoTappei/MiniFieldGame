using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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
    /// <summary>フェードに使う画像の親オブジェクト</summary>
    [SerializeField] Transform _fadeBlockParent;
    /// <summary>フェードに使う画像のリスト</summary>
    List<GameObject> _fadeBlocks = new List<GameObject>();
    /// <summary>フェード中かどうか、ゲームが始まる際には必ずフェードから始めるので初期値がtrue</summary>
    bool _isFading = true;

    public int CurrentStageNum { get => _currentStageNum; }
    public bool IsFading { get => _isFading; }
    public int TotalScore { get => _totalScore; }

    /// <summary>ステージ番号をリセットする</summary>
    public void ResetStageNum() => _currentStageNum = 1;
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

            // シーンが読み込まれるたびにフェードインのメソッドを呼ぶ
            SceneManager.activeSceneChanged += FadeIn;

            foreach (Transform child in _fadeBlockParent)
            {
                _fadeBlocks.Add(child.gameObject);
            }
            _fadeBlocks = _fadeBlocks.OrderBy(g => System.Guid.NewGuid()).ToList();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 今回は特殊なフェードをするのでFadeImageオブジェクトは使わないので、Alphaを0にして透明にしてある。

    /// <summary>フェードインする</summary>
    public void FadeIn(Scene _, Scene __)
    {
        Debug.Log("フェードイン中");
        StartCoroutine(Fade());

        IEnumerator Fade()
        {
            for (int i = 0; i < _fadeBlocks.Count; i += 3)
            {
                _fadeBlocks[i].SetActive(false);
                _fadeBlocks[i + 1].SetActive(false);
                _fadeBlocks[i + 2].SetActive(false);
                yield return new WaitForSeconds(0.01f);
                Debug.Log("表示させていく");
            }
            _isFading = false;
        }
    }

    /// <summary>フェードアウト後、シーンを推移する</summary>
    public void FadeOut(string sceneName)
    {
        Debug.Log("フェードアウト中");
        _isFading = true;
        StartCoroutine(Fade());

        IEnumerator Fade()
        {
            for (int i = 0; i < _fadeBlocks.Count; i += 3)
            {
                _fadeBlocks[i].SetActive(true);
                _fadeBlocks[i + 1].SetActive(true);
                _fadeBlocks[i + 2].SetActive(true);
                yield return new WaitForSeconds(0.01f);
            }
            Debug.Log("フェードアウト終了");
            SceneManager.LoadScene(sceneName);
        }
    }
}
