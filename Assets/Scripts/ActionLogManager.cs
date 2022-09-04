using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの行動のログを画面に表示する
/// </summary>
public class ActionLogManager : MonoBehaviour
{
    [SerializeField] GameObject _log;
    [SerializeField] Transform _parent;

    void Start()
    {
        
    }

    void Update()
    {

    }

    /// <summary>コインを獲得したログを表示</summary>
    public void DispLog(string message)
    {
        var go = Instantiate(_log, _parent.transform.position, Quaternion.identity, _parent);
        go.GetComponentInChildren<Text>().text = message;

        // 新しく追加したテキスト以外のy座標をずらす
        for (int i = 0; i < _parent.childCount - 1; i++)
        {
            Transform trans = _parent.GetChild(i);
            trans.position = new Vector3(trans.position.x, trans.position.y - Screen.height / 18, trans.position.z);
        }

        // もし画面に表示されているテキストが4つ以上あれば一番古いものを消す
        if (_parent.childCount >= 4)
        {
            GameObject child = _parent.GetChild(0).gameObject;
            child.GetComponentInChildren<Animator>().Play("FadeOut");
            // 親の0番目の子を消すから消す対象のものは親を別のものにしておく
            child.transform.SetParent(transform);
            Destroy(child, 2.0f);
        }
    }
}
