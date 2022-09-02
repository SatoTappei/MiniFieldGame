using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>常にカメラを向くオブジェクト</summary>
public class BillBoard : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.y = transform.position.y;
        transform.LookAt(cameraPos);
    }
}
