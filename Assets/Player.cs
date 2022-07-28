using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �v���C���[���䂷��
/// </summary>
public class Player : MapObjectBase
{
    [Range(0, 100)] public float _cameraDistance;
    public Vector3 _cameraDirection = new Vector3(0, 10, -3);

    void Start()
    {
        StartCoroutine(CameraMove());
    }

    IEnumerator CameraMove()
    {
        var camera = Camera.main;
        while (true)
        {
            // �J�����̈ʒu���v���C���[����̑��Έʒu�ɐݒ肷��
            camera.transform.position = transform.position + _cameraDirection.normalized * _cameraDistance;
            camera.transform.LookAt(transform.position);
            yield return null;
        }
    }
}
