using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ���U���g��ʑS�̂��Ǘ��E���䂷��
/// </summary>
public class ResultManager : MonoBehaviour
{
    [SerializeField] ResultUIManager _resultUIManager;

    /// <summary>���͂��ꂽ�v���C���[�̖��O��ێ�����</summary>
    string _scoreName;
    /// <summary>���O�̓��͂��I�������true�ɂȂ�</summary>
    bool _finishedNameEntry;

    IEnumerator Start()
    {
        // �X�R�A�̕\�����I���܂ł͓��͂ł��Ȃ��悤�ɂ���
        _resultUIManager.SetSelectedDummyButton();
        // �t�F�[�h�C�����I���̂�҂�
        yield return new WaitWhile(() => GameManager._instance.IsFading);
        // �X�R�A��\��������
        SoundManager._instance.Play("SE_�J�^�J�^");
        yield return StartCoroutine(_resultUIManager.DispClearScore(GameManager._instance.TotalScore));
        // �X�R�A�l�[������͂���
        _resultUIManager.SetSelectedDefaultButton();
        // �X�R�A�l�[������͂��I����܂ő҂�
        yield return new WaitUntil(() => _finishedNameEntry);
        // �O��܂ł̃��U���g�̃f�[�^�����[�h����
        SaveDataManager.Load();
        // ����̃��U���g���Z�[�u����
        SaveDataManager.Save(_scoreName, GameManager._instance.TotalScore);
        // �����L���O�ɔ��f������
        SetRanking();
        // �l�[���G���g���[���I���ă����L���O��ʂֈڍs����
        MoveRanking();
        // �����L���O�ɔ��f
        SetRanking();
    }

    void Update()
    {
        //// �Z�[�u�ƃ��[�h�̃e�X�g�A�I����������
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    string[] ns = { "TPI", "TKS", "HOG", "PIY" };
        //    int r = Random.Range(0, 4);
        //    int score = Random.Range(0, 10000);
        //    SaveDataManager.Save(ns[r], score);
        //    Debug.Log("�Z�[�u�f�[�^���Z�[�u���܂���");
        //}
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    SaveDataManager.Load();
        //    Debug.Log("�Z�[�u�f�[�^�����[�h���܂���");
        //}
    }

    /// <summary>�X�R�A�l�[���ɕ�����ǉ�����</summary>
    public void AddScoreName(NameInputButton nib)
    {
        _scoreName += nib.Char;
        _resultUIManager.SetScoreName(_scoreName);

        // ���͂��ꂽ���O��3�����ɂȂ�����X�R�A�l�[���̓��͂��I����
        if (_scoreName.Length == 3)
            EndInputScoreName();
        else
            SoundManager._instance.Play("SE_���U���g1");
    }

    /// <summary>�X�R�A�l�[���̓��͂��I����</summary>
    public void EndInputScoreName()
    {
        // ���O�̓��͂��Ȃ��ꍇ�͊���̖��O�ɂ���
        if (_scoreName == null)
        {
            _scoreName = "^^;";
            _resultUIManager.SetScoreName(_scoreName);
        }
        SoundManager._instance.Play("SE_�^�C�g���{�^��");
        // �_�~�[�̃{�^����I����Ԃɂ��ē��͂�h��
        _resultUIManager.SetSelectedDummyButton();
        // �l�[���G���g���[���I������t���O�𗧂Ă�
        _finishedNameEntry = true;
    }

    /// <summary>�����L���O��ʂɈڍs����</summary>
    void MoveRanking()
    {
        // �X�R�A�l�[�����͉�ʂ���ă����L���O��ʂֈڍs����
        _resultUIManager.ChangeToRankingPanel();
        // �����L���O��ʂֈڍs���ď����҂��Ă���{�^����������悤�ɂ���
        DOVirtual.DelayedCall(1.5f, () =>
        {
            _resultUIManager.SetSelectedBackTitleButton();
        });
    }

    /// <summary>�����L���O�ɔ��f������</summary>
    void SetRanking()
    {
        int max = SaveDataManager.GetDataCount();
        for (int i = 1; i <= max; i++)
        {
            SaveDataManager.Result result = SaveDataManager.GetRankResult(i);
            _resultUIManager.SetRankingItem(i, result._name, result._score);
        }
    }

    /// <summary>�^�C�g���ɖ߂�</summary>
    public void MoveTitle()
    {
        GameManager._instance.FadeOut("Title");
    }
}
