using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S2_MessageWindow : MonoBehaviour
{
    public Text _text;
    public float maxPerFrameH = 0.5f;
    public float maxPerFrameV = 1.0f;
    bool isAdding = false;
    bool isFalling = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (isAdding)
        {
            S2_MessageAnimation anim;
            if (!isFalling)
            {
                anim = transform.GetChild(transform.childCount - 1).GetComponent<S2_MessageAnimation>();
                isAdding = !anim.MoveMessage(transform.position + Vector3.zero, maxPerFrameH);
                return;
            }

            for (int i = 0; i < transform.childCount - 1; i++)
            {
                anim = transform.GetChild(i).GetComponent<S2_MessageAnimation>();
                if (anim.IsDeleting()) continue;
                isFalling = !anim.MoveMessage(transform.position + new Vector3(0, -100 * (transform.childCount - i - 1), 0), maxPerFrameV);
            }
        }
        else ShowMessage();
    }

    /// <summary>メッセージを追加(表示)する</summary>
    void ShowMessage()
    {
        if (S2_Message.getCount() > 0)
        {
            isAdding = true;
            isFalling = transform.childCount > 0;
            string m = S2_Message.get();
            Text msg = Instantiate(_text, transform);
            msg.transform.position = transform.position + new Vector3(-2000, 0, 0);
            msg.text = m;
        }
    }
}
