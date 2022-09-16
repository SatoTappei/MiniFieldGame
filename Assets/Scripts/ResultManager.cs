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

    string _scoreName;

    IEnumerator Start()
    {
        // �X�R�A�̕\�����I���܂ł͓��͂ł��Ȃ��悤�ɂ���
        _resultUIManager.SetSelectedDummyButton();
        // �X�R�A��\��������
        yield return StartCoroutine(_resultUIManager.DispClearScore(9999/*GameManager._instance.TotalScore*/));
        // �X�R�A�l�[������͂���
        _resultUIManager.SetSelectedDefaultButton();
        yield return null;
    }

    void Update()
    {
        
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
        // �_�~�[�̃{�^����I����Ԃɂ��ē��͂�h��
        _resultUIManager.SetSelectedDummyButton();
        // �X�R�A�l�[�����͉�ʂ���ă����L���O��ʂֈڍs����
        _resultUIManager.ChangeToRankingPanel();
        SoundManager._instance.Play("SE_�^�C�g���{�^��");
        // �����L���O��ʂֈڍs���ď����҂��Ă���{�^����������悤�ɂ���
        DOVirtual.DelayedCall(1.5f, () =>
        {
            _resultUIManager.SetSelectedBackTitleButton();
        });
    }

    /// <summary>�^�C�g���ɖ߂�</summary>
    public void MoveTitle()
    {
        GameManager._instance.FadeOut("Title");
    }
}
