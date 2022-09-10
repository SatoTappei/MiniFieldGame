using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �X�e�[�W���\������u���b�N
/// </summary>
public class BlockTilePrefab : MonoBehaviour
{
    /// <summary>�����_���ŕʂ̐F�ɂ��邽�߂̏��̃}�e���A��</summary>
    [SerializeField] Color32[] colors;
    /// <summary>������Ɉʒu�����炵����㏈��������</summary>
    [SerializeField] UnityEvent _setPrefabMethod;

    void Start()
    {
        // �F�������_���ɕύX������
        int r = Random.Range(0, colors.Length);
        GetComponent<MeshRenderer>().material.color = colors[r];

        _setPrefabMethod.Invoke();
    }

    void Update()
    {
        
    }

    /// <summary>�ʒu���c�����Ƀ����_���ɂ��炷</summary>
    public void SetRandomY()
    {
        float r = Random.Range(0.0f, 0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y + r, transform.position.z);
    }
}
