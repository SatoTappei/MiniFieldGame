using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 同じメニューにボタンが2つ以上ある場合に選択された＆選択が外れたの処理をする
/// </summary>
public class PlaySceneUIButton : MonoBehaviour
{
    Image _img;
    /// <summary>ボタンが選ばれているときの色</summary>
    [SerializeField] Color32 _selectColor;
    /// <summary>ボタンのデフォルトの色</summary>
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

    /// <summary>ボタンを選択した際の演出</summary>
    public void SelectThis()
    {
        transform.DOScale(Vector3.one * 1.1f, 0.1f);
        _img.DOColor(_selectColor,0.1f);
        SoundManager._instance.Play("SE_ボタン選択");
    }

    /// <summary>ボタンから選択が外れた際の演出</summary>
    public void DeselectThis()
    {
        transform.DOScale(Vector3.one, 0.1f);
        _img.DOColor(_defaultColor, 0.1f);
    }
}
