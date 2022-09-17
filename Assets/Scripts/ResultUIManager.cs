using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// ���U���g�V�[����UI���Ǘ�����
/// </summary>
public class ResultUIManager : MonoBehaviour
{
    /// <summary>�N���A���̃X�R�A��\������e�L�X�g</summary>
    [SerializeField] Text _clearScoreText;
    /// <summary>�X�R�A�l�[����\������e�L�X�g</summary>
    [SerializeField] Text _scoreNameText;
    /// <summary>�X�R�A�l�[�����̓{�^���̐e</summary>
    [SerializeField] GameObject _inputButtonParent;
    /// <summary>�{�^����I�����Ă��Ȃ���Ԃɂ��邽�߂̃_�~�[�̃{�^��</summary>
    [SerializeField] GameObject _dummyButton;
    /// <summary>�X�R�A�����L���O�̃p�l��</summary>
    [SerializeField] GameObject _rankingPanel;
    /// <summary>�f�t�H���g�̃X�R�A�l�[������͂���{�^��</summary>
    [SerializeField] GameObject _defaultInputButton;
    /// <summary>�^�C�g���ɖ߂�̃{�^��</summary>
    [SerializeField] GameObject _backTitleButton;
    /// <summary>�����L���O�ɕ\�������e���U���g�̃I�u�W�F�N�g�̐e</summary>
    [SerializeField] Transform _rankingItemParent;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�N���A���̃X�R�A��\������</summary>
    public IEnumerator DispClearScore(int score)
    {
        bool finish = false;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_clearScoreText.DOCounter(0, score, 2.0f));
        sequence.Append(_clearScoreText.transform.DOScale(1.2f, 0.15f));
        sequence.Append(_clearScoreText.transform.DOScale(1.0f, 0.15f));
        sequence.AppendCallback(() =>
        {
            SoundManager._instance.Play("SE_���U���g2");
            finish = true;
        });
        // TODO:�n�C�X�R�A�X�V���ɂ͐�p�̉��o������
        sequence.SetLink(gameObject);

        yield return new WaitUntil(() => finish);
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>�X�R�A�l�[����\������</summary>
    public void SetScoreName(string name) => _scoreNameText.text = name;

    /// <summary>�X�R�A�l�[���̓��̓p�l�������</summary>
    public void ChangeToRankingPanel()
    {
        // �X�R�A�l�[�����t���b�V��������
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_scoreNameText.DOFade(0, 0.33f).SetEase(Ease.Flash, 5));
        sequence.Append(_scoreNameText.DOFade(255, 0.01f));
        // �X�R�A�l�[�����͉�ʂ����ɃX���C�h������
        Transform clearScorePanel = _inputButtonParent.transform.parent;
        sequence.Append(clearScorePanel.DOMoveY(-600, 0.25f).SetDelay(1.0f).SetRelative());
        // �����L���O�p�l�������ɃX���C�h������
        sequence.Join(_rankingPanel.transform.DOMoveY(-600, 0.25f).SetRelative());
    }

    /// <summary>�����L���O�̊e���U���g�̃I�u�W�F�N�g���Z�b�g����</summary>
    public void SetRankingItem(int rank, string name, int score)
    {
        if (rank < 1 || rank > _rankingItemParent.childCount)
        {
            Debug.LogWarning("�n���ꂽ���ʂ�\������I�u�W�F�N�g������܂���B:" + rank);
            return;
        }

        GameObject go = _rankingItemParent.GetChild(rank - 1).gameObject;
        go.GetComponent<ResultRankingItem>().SetResult(name, score);
    }

    /// <summary>�_�~�[�̃{�^����I����Ԃɂ��ăL�[�̓��͂�h��</summary>
    public void SetSelectedDummyButton() => EventSystem.current.SetSelectedGameObject(_dummyButton);

    /// <summary>�f�t�H���g�̃{�^����I����Ԃɂ��ăL�[�̓��͂��󂯕t����</summary>
    public void SetSelectedDefaultButton() => EventSystem.current.SetSelectedGameObject(_defaultInputButton);

    /// <summary>�^�C�g���ɖ߂�̃{�^����I����Ԃɂ��ăL�[�̓��͂��󂯕t����</summary>
    public void SetSelectedBackTitleButton() => EventSystem.current.SetSelectedGameObject(_backTitleButton);
}
