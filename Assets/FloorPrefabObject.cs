using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W�̏��ƂȂ�I�u�W�F�N�g
/// </summary>
public class FloorPrefabObject : MonoBehaviour
{
    /// <summary>�����_���ŕʂ̐F�ɂ��邽�߂̏��̃}�e���A��</summary>
    [SerializeField] Material _mat;

    void Start()
    {
        int r = Random.Range(1, 3);
        if (r == 1) GetComponent<MeshRenderer>().material = _mat;
    }

    void Update()
    {
        
    }
}
