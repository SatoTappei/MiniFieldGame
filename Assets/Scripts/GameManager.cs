using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    /// <summary>�t�F�[�h�Ɏg���摜</summary>
    [SerializeField] Image _fadeImage;
    /// <summary>�t�F�[�h�����ǂ����A�Q�[�����n�܂�ۂɂ͕K���t�F�[�h����n�߂�̂ŏ����l��true</summary>
    bool _isFading = true;

    public int CurrentStageNum { get => _currentStageNum; }
    public bool IsFading { get => _isFading; }
    public int TotalScore { get => _totalScore; }

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
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        // �V�[�����ǂݍ��܂�邽�тɃt�F�[�h�C���̃��\�b�h���Ă�
        SceneManager.activeSceneChanged += FadeIn;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�t�F�[�h�C������</summary>
    public void FadeIn(Scene _, Scene __)
    {
        Debug.Log("�t�F�[�h�C��");
        Sequence sequence = DOTween.Sequence();
        // TODO:�t�F�[�h�̉��o��ς���
        sequence.Join(DOTween.ToAlpha(() => _fadeImage.color, color => _fadeImage.color = color, 0f, 1f));
        // TODO:�t�F�[�h�̉��o�����܂�
        sequence.AppendCallback(() => { _fadeImage.gameObject.SetActive(false);_isFading = false; });
    }

    /// <summary>�t�F�[�h�A�E�g��A�V�[���𐄈ڂ���</summary>
    public void FadeOut(string sceneName)
    {
        Debug.Log("�t�F�[�h�A�E�g");
        _isFading = true;
        _fadeImage.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        // TODO:�t�F�[�h�̉��o��ς���
        sequence.Join(DOTween.ToAlpha(() => _fadeImage.color, color => _fadeImage.color = color, 1f, 1f));
        // TODO:�t�F�[�h�̉��o�����܂�
        sequence.AppendCallback(() => SceneManager.LoadScene(sceneName));
    }
}
