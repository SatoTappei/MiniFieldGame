using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �������j���[�Ƀ{�^����2�ȏ゠��ꍇ�ɑI�����ꂽ���I�����O�ꂽ�̏���������
/// </summary>
public class PlaySceneUIButton : MonoBehaviour
{
    Image _img;
    /// <summary>�{�^�����I�΂�Ă���Ƃ��̐F</summary>
    [SerializeField] Color32 _selectColor;
    /// <summary>�{�^���̃f�t�H���g�̐F</summary>
    Color32 _defaultColor;

    void Awake()
    {
        _img = GetComponent<Image>();
        _defaultColor = _img.color;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�{�^����I�������ۂ̉��o</summary>
    public void SelectThis()
    {
        transform.DOScale(Vector3.one * 1.1f, 0.1f);
        _img.DOColor(_selectColor,0.1f);
        SoundManager._instance.Play("SE_�{�^���I��");
    }

    /// <summary>�{�^������I�����O�ꂽ�ۂ̉��o</summary>
    public void DeselectThis()
    {
        transform.DOScale(Vector3.one, 0.1f);
        _img.DOColor(_defaultColor, 0.1f);
    }
}
