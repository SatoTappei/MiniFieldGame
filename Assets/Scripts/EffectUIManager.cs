using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// �Q�[���J�n�A�X�e�[�W�N���A�A�Q�[���I�[�o�[�̉��o�p��UI�𐧌䂷��
/// </summary>
public class EffectUIManager : MonoBehaviour
{
    /// <summary>�Q�[���X�^�[�g�̉��o�Ɏg���I�u�W�F�N�g�B�̐e</summary>
    [SerializeField] Transform _gameStartEffect;
    /// <summary>�X�e�[�W�N���A�̉��o�Ɏg���I�u�W�F�N�g�B�̐e</summary>
    [SerializeField] Transform _stageClearEffect;
    /// <summary>�Q�[���I�[�o�[���̉��o�Ɏg���I�u�W�F�N�g�̒B�̐e</summary>
    [SerializeField] GameObject _gameOverEffect;
    /// <summary>"���̃X�e�[�W��"�̃{�^��</summary>
    [SerializeField] GameObject _nextStageButton;
    /// <summary>"���g���C����"�̃{�^��</summary>
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

    /// <summary>�Q�[���J�n���̉��o</summary>
    public IEnumerator GameStartEffect(int stageNum, StageDataSO so)
    {
        // �e�e�L�X�g�ɃX�e�[�W�̏���\��
        _gameStartEffect.GetChild(0).GetChild(1).GetComponent<Text>().text = stageNum.ToString();
        _gameStartEffect.GetChild(1).GetChild(1).GetComponent<Text>().text = so.MaxCoin.ToString();
        _gameStartEffect.GetChild(2).GetChild(1).GetComponent<Text>().text = so.MaxEnemy.ToString();
        _gameStartEffect.GetChild(3).GetChild(1).GetComponent<Text>().text = so.TurnLimit.ToString();
        // �p�l�����A�j���[�V���������ĕ\��������
        Sequence sequence1 = DOTween.Sequence();
        sequence1.Join(_gameStartEffect.DOScale(new Vector3(1, 0.01f, 1), 0.5f));
        sequence1.Append(_gameStartEffect.DOScale(Vector3.one, 0.15f));
        sequence1.Append(_gameStartEffect.DOShakeScale(0.15f, 0.25f));
        //// �A�j���[�V�������n�܂���1�b���"�X�y�[�X�L�[�������Ă�"�̃{�^����\��
        yield return new WaitForSeconds(1.0f);
        _gameStartEffect.GetChild(4).gameObject.SetActive(true);
        //�N���b�N���ꂽ��p�l�����A�j���[�V���������ĕ���
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        SoundManager._instance.Play("SE_����");
        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(_gameStartEffect.DOScale(new Vector3(1.25f, 1.25f, 1), 0.15f));
        sequence2.Append(_gameStartEffect.DOScale(Vector3.zero, 0.25f));
    }

    /// <summary>�X�e�[�W�N���A���̉��o</summary>
    public IEnumerator StageClearEffect(int stageNum, StageDataSO so, int coin, int enemy, int turn, int score)
    {
        _stageClearEffect.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_nextStageButton);
        // �Ō�̃X�e�[�W���ۂ��ŕ\��������e��؂�ւ���
        string label = GameManager._instance.CheckAllClear() ? "���U���g��" : "���̃X�e�[�W��";
        _stageClearEffect.GetChild(5).GetComponentInChildren<Text>().text = label;
        // ���݂̃X�e�[�W�ԍ���\��
        _stageClearEffect.GetChild(0).GetChild(1).GetComponent<Text>().text = stageNum.ToString();
        // �e���U���g���|�b�v������A�j���[�V�����t���ŕ\��������
        yield return StartCoroutine(Pop(1, (so.MaxCoin - coin) + " / " + so.MaxCoin));
        yield return StartCoroutine(Pop(2, (so.MaxEnemy - enemy) + " / " + so.MaxEnemy));
        yield return StartCoroutine(Pop(3, (so.TurnLimit - turn) + " / " + so.TurnLimit));
        // �X�R�A��0���猻�݂̒l�܂ŉ��Z�����A�j���[�V�����ŕ\������
        Sequence sequence = DOTween.Sequence();
        Transform scoreTrans = _stageClearEffect.GetChild(4).GetChild(1);
        sequence.Join(scoreTrans.GetComponent<Text>().DOCounter(0, score, 1.5f));
        sequence.Append(scoreTrans.DOScale(1.2f, 0.15f));
        sequence.Append(scoreTrans.DOScale(1f, 0.15f));
        sequence.AppendCallback(() => SoundManager._instance.Play("SE_���U���g2"));
        sequence.SetLink(gameObject);

        // �X�R�A�ȊO�̃��U���g���|�b�v������A�j���[�V����
        IEnumerator Pop(int childIndex, string str)
        {
            Transform item = _stageClearEffect.GetChild(childIndex).GetChild(1);
            item.GetComponent<Text>().text = str;
            Sequence sequence = DOTween.Sequence();
            sequence.Join(item.DOScale(1.2f, 0.15f));
            sequence.Append(item.DOScale(1.0f, 0.15f));
            sequence.SetLink(gameObject);
            SoundManager._instance.Play("SE_���U���g1");
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>�Q�[���I�[�o�[���̉��o</summary>
    public IEnumerator GameOverEffect()
    {
        // TODO:�����ƍ��
        SoundManager._instance.Play("SE_�`�[��");
        _gameOverEffect.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_retryButton);
        yield return new WaitForSeconds(1.5f);
        SoundManager._instance.Play("SE_���߂��ׂ當�����o");
        _gameOverEffect.transform.GetChild(0).transform.DORotate(new Vector3(0, 0, -8.5f), 0.05f);
        yield return null;
    }

    /// <summary>���Ԑ؂�ŃQ�[���I�[�o�[�ɂȂ����ۂ̉��o</summary>
    public IEnumerator TimeOverEffect()
    {
        FindObjectOfType<PlayerManager>().Death(ActorDir.Neutral);
        yield return StartCoroutine(GameOverEffect());
    }
}
