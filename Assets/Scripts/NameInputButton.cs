using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���U���g��ʂŖ��O���͂Ɏg���{�^��
/// </summary>
public class NameInputButton : MonoBehaviour
{
    Text _text;
    /// <summary>���̃{�^���ɑΉ����镶��</summary>
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
