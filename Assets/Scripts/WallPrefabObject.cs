using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W�̕ǂƂȂ�I�u�W�F�N�g
/// </summary>
public class WallPrefabObject : MonoBehaviour
{
    void Start()
    {
        // �������ꂽ��A�ʒu��y�������ɏ������炵�ăX�e�[�W�̌����ڂɕω���t����
        float r = Random.Range(0.0f, 0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y + r, transform.position.z);
    }

    void Update()
    {
        
    }
}
