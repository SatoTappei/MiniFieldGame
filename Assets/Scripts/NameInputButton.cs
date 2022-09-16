using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面で名前入力に使うボタン
/// </summary>
public class NameInputButton : MonoBehaviour
{
    Text _text;
    /// <summary>このボタンに対応する文字</summary>
    [SerializeField] char _char;

    public char Char { get => _char; }

    void Awake()
    {
        _text = GetComponentInChildren<Text>();
        _text.text = _char.ToString();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
