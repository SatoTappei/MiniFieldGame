using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Q�[���}�l�[�W���[
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    /// <summary>�ő�X�e�[�W�ԍ�</summary>
    [SerializeField] int _maxStageNum;
    /// <summary>���݂̃X�e�[�W�ԍ��A�ŏ���1�Ȃ̂Œ���</summary>
    int _currentStageNum = 1;
    /// <summary>���v�X�R�A</summary>
    int _totalScore;
    /// <summary>�t�F�[�h�Ɏg���摜�̐e�I�u�W�F�N�g</summary>
    [SerializeField] Transform _fadeBlockParent;
    /// <summary>�t�F�[�h�Ɏg���摜�̃��X�g</summary>
    List<GameObject> _fadeBlocks = new List<GameObject>();
    /// <summary>�t�F�[�h�����ǂ����A�Q�[�����n�܂�ۂɂ͕K���t�F�[�h����n�߂�̂ŏ����l��true</summary>
    bool _isFading = true;

    public int CurrentStageNum { get => _currentStageNum; }
    public bool IsFading { get => _isFading; }
    public int TotalScore { get => _totalScore; }

    /// <summary>�X�e�[�W�ԍ������Z�b�g����</summary>
    public void ResetStageNum() => _currentStageNum = 1;
    /// <summary>�X�e�[�W���N���A�����̂ŃX�e�[�W�ԍ���1�i�߂�</summary>
    public void AdvanceStageNum() => _currentStageNum = Mathf.Min(++_currentStageNum, _maxStageNum);
    /// <summary>�S�X�e�[�W�N���A�������ǂ���</summary>
    public bool CheckAllClear() => _currentStageNum == _maxStageNum;
    /// <summary>
    /// ���v�X�R�A���X�V����
    /// ���݂̓X�e�[�W�N���A��ɌĂ΂�Ď��̃X�e�[�W�ɃX�R�A�������p������
    /// </summary>
    public void UpdateTotalScore(int score) => _totalScore = score;

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // �V�[�����ǂݍ��܂�邽�тɃt�F�[�h�C���̃��\�b�h���Ă�
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

    ///// <summary>�t�F�[�h�C������</summary>
    public async void FadeIn(Scene _, Scene __)
    {
        Debug.Log("�t�F�[�h�C����");

        for (int i = 0; i < _fadeBlocks.Count; i += 3)
        {
            _fadeBlocks[i].SetActive(false);
            _fadeBlocks[i + 1].SetActive(false);
            _fadeBlocks[i + 2].SetActive(false);
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.01f));
        }
        _isFading = false;
    }

    /// <summary>�t�F�[�h�A�E�g��A�V�[���𐄈ڂ���</summary>
    public async void FadeOut(string sceneName)
    {
        Debug.Log("�t�F�[�h�A�E�g��");
        _isFading = true;

        for (int i = 0; i < _fadeBlocks.Count; i += 3)
        {
            _fadeBlocks[i].SetActive(true);
            _fadeBlocks[i + 1].SetActive(true);
            _fadeBlocks[i + 2].SetActive(true);
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.01f));
        }
        Debug.Log("�t�F�[�h�A�E�g�I��");
        SceneManager.LoadScene(sceneName);
    }
}
