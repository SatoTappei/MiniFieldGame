using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_MessageAnimation : MonoBehaviour
{
    public float waitTime = 8.0f;
    public float maxPerFrameD = 1.0f;

    bool isMoving = false;
    bool isDeleting = false;
    Vector3 prevPos;
    int frame = 0;

    void Start()
    {
        prevPos = transform.position;
    }

    void Update()
    {
        Invoke("DeleteMessage", waitTime);
    }

    /// <summary>補完で計算してアニメーションさせる</summary>
    public bool MoveMessage(Vector3 p2, float maxPerFrame)
    {
        isMoving = !isDeleting;
        frame++;
        float c = maxPerFrame / Time.deltaTime;
        float t = frame / c;
        transform.position = prevPos + (p2 - prevPos) * t;
        if (c <= frame)
        {
            frame = 0;
            transform.position = p2;
            prevPos = p2;
            isMoving = false;
            return true;
        }
        return false;
    }

    /// <summary>削除アニメーション中ではないか</summary>
    public bool IsDeleting() => isDeleting;

    /// <summary>削除アニメーション</summary>
    void DeleteMessage()
    {
        if (isMoving) return;
        isDeleting = true;
        MoveMessage(prevPos + new Vector3(0, -1000, 0), maxPerFrameD);
        if (transform.position.y < -100.0f)
            Destroy(gameObject);
    }
}
