using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        _gameStartEffect.SetActive(false);
        _stageClearEffect.SetActive(false);
        _gameOverEffect.SetActive(false);
    }

    void Update()
    {
        
    }

    /// <summary>�Q�[���J�n���̉��o</summary>
    public IEnumerator GameStartEffect(/*�Ăяo�������X�e�[�W�̏���n��*/)
    {
        _gameStartEffect.SetActive(true);
        // TODO:�����Ɖ��o�����
        _gameStartEffect.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>().text = 65535.ToString();
        yield return null;
        _gameStartEffect.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Text>().text = 256.ToString();
        yield return null;
        _gameStartEffect.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Text>().text = "�����ς�";
        yield return null;
        _gameStartEffect.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Text>().text = "��������";
        yield return new WaitForSeconds(0.5f);
        _gameStartEffect.SetActive(false);
    }

    /// <summary>�X�e�[�W�N���A���̉��o</summary>
    public IEnumerator StageClearEffect()
    {
        _stageClearEffect.SetActive(true);
        // TODO:�����Ɖ��o�����
        _stageClearEffect.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>().text = 65535.ToString() + " / " + 65535.ToString();
        yield return null;
        _stageClearEffect.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Text>().text = 256.ToString() + " / " + 256.ToString();
        yield return null;
        _stageClearEffect.transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<Text>().text = "�����ς�" + " / " + "�����ς�";
        yield return null;
        _stageClearEffect.transform.GetChild(3).gameObject.transform.GetChild(1).GetComponent<Text>().text = "��������" + " / " + "��������";
        yield return new WaitForSeconds(0.5f);
        _stageClearEffect.SetActive(false);
        yield return null;
    }

    /// <summary>�Q�[���I�[�o�[���̉��o</summary>
    public IEnumerator GameOverEffect()
    {
        _gameOverEffect.SetActive(true);
        yield return null;
    }
}
