using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �Q�[���J�n�A�X�e�[�W�N���A�A�Q�[���I�[�o�[�̉��o�p��UI�𐧌䂷��
/// </summary>
public class EffectUIManager : MonoBehaviour
{
    /// <summary>�Q�[���X�^�[�g�̉��o�Ɏg���I�u�W�F�N�g�B�̐e</summary>
    [SerializeField] GameObject _gameStartEffect;
    /// <summary>�X�e�[�W�N���A�̉��o�Ɏg���I�u�W�F�N�g�B�̐e</summary>
    [SerializeField] GameObject _stageClearEffect;
    /// <summary>�Q�[���I�[�o�[���̉��o�Ɏg���I�u�W�F�N�g�̒B�̐e</summary>
    [SerializeField] GameObject _gameOverEffect;

    void Awake()
    {
        _gameStartEffect.transform.localScale = Vector3.zero;
        _stageClearEffect.SetActive(false);
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
        _gameStartEffect.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>().text = stageNum.ToString();
        _gameStartEffect.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Text>().text = so.MaxCoin.ToString();
        _gameStartEffect.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Text>().text = so.MaxEnemy.ToString();
        _gameStartEffect.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Text>().text = so.TurnLimit.ToString();
        // �p�l�����A�j���[�V���������ĕ\��������
        Sequence sequence1 = DOTween.Sequence();
        sequence1.Join(_gameStartEffect.transform.DOScale(new Vector3(1, 0.01f, 1), 0.5f));
        sequence1.Append(_gameStartEffect.transform.DOScale(Vector3.one, 0.15f));
        sequence1.Append(_gameStartEffect.transform.DOShakeScale(0.15f, 0.25f));
        // �A�j���[�V�������n�܂���1�b���"���N���b�N�����Ă�"�̃{�^����\��
        yield return new WaitForSeconds(1.0f);
        _gameStartEffect.transform.GetChild(4).gameObject.SetActive(true);
        //�N���b�N���ꂽ��p�l�����A�j���[�V���������ĕ���
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(_gameStartEffect.transform.DOScale(new Vector3(1.25f, 1.25f, 1), 0.15f));
        sequence2.Append(_gameStartEffect.transform.DOScale(Vector3.zero, 0.25f));
    }

    /// <summary>�X�e�[�W�N���A���̉��o</summary>
    public IEnumerator StageClearEffect(int stageNum, StageDataSO so, int coin, int enemy, int turn, int score)
    {
        _stageClearEffect.SetActive(true);
        // ���݂̃X�e�[�W�ԍ���\��
        _stageClearEffect.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>().text = stageNum.ToString();
        // �e���U���g���|�b�v������A�j���[�V�����t���ŕ\��������
        Pop(_stageClearEffect.transform.GetChild(1).gameObject.transform.GetChild(1), (so.MaxCoin - coin).ToString() + " / " + so.MaxCoin.ToString());
        yield return new WaitForSeconds(0.5f);
        Pop(_stageClearEffect.transform.GetChild(2).gameObject.transform.GetChild(1), (so.MaxEnemy - enemy).ToString() + " / " + so.MaxEnemy.ToString());
        yield return new WaitForSeconds(0.5f);
        Pop(_stageClearEffect.transform.GetChild(3).gameObject.transform.GetChild(1), (so.TurnLimit - turn).ToString() + " / " + so.TurnLimit.ToString());
        yield return new WaitForSeconds(0.5f);
        // �X�R�A��0���猻�݂̒l�܂ŉ��Z�����A�j���[�V�����ŕ\������
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_stageClearEffect.transform.GetChild(4).gameObject.transform.GetChild(1).GetComponent<Text>().DOCounter(0, score, 1.5f));
        sequence.Append(_stageClearEffect.transform.GetChild(4).gameObject.transform.GetChild(1).transform.DOScale(1.2f, 0.15f));
        sequence.Append(_stageClearEffect.transform.GetChild(4).gameObject.transform.GetChild(1).transform.DOScale(1f, 0.15f));
        sequence.AppendCallback(() => SoundManager._instance.Play("SE_���U���g2"));
        sequence.SetLink(gameObject);

        // �|�b�v������A�j���[�V����
        void Pop(Transform trans, string str)
        {
            trans.GetComponent<Text>().text = str;
            Sequence sequence = DOTween.Sequence();
            sequence.Join(trans.DOScale(1.2f, 0.15f));
            sequence.Append(trans.DOScale(1.0f, 0.15f));
            sequence.SetLink(gameObject);
            SoundManager._instance.Play("SE_���U���g1");
        }
    }

    /// <summary>�Q�[���I�[�o�[���̉��o</summary>
    public IEnumerator GameOverEffect()
    {
        _gameOverEffect.SetActive(true);
        yield return null;
    }
}
