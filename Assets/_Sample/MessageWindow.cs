using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 行動を表示するメッセージウインドウを制御する
/// </summary>
public class MessageWindow : MonoBehaviour
{
    /// <summary>メッセージ本体</summary>
    public Text _messagePrefab;
    [Range(1, 15)]
    public int _messageLimit = 5;

    Transform _root;

    void Awake()
    {
        _root = transform.Find("Root");
        foreach(Transform child in _root)
        {
            Destroy(child.gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public static MessageWindow Find()
    {
        return FindObjectOfType<MessageWindow>();
    }

    public void AppendMessage(string message)
    {
        var obj = Instantiate(_messagePrefab, _root);
        obj.text = message;

        if (_root.childCount > _messageLimit)
        {
            var removeCount = _root.childCount - _messageLimit;
            for (int i = removeCount - 1; 0 <= i; i--)
            {
                Destroy(_root.GetChild(i).gameObject);
            }
        }
    }
}
