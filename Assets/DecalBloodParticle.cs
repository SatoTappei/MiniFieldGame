using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̃p�[�e�B�N�����Ԃ�������I�u�W�F�N�g�̕\�ʂɌ��̃f�J�[���𐶐�����
/// </summary>
public class DecalBloodParticle : MonoBehaviour
{
    /// <summary>���̃f�J�[��</summary>
    [SerializeField] GameObject _bloodDecal;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        Instantiate(_bloodDecal, new Vector3(transform.position.x, 0.15f, transform.position.z), Quaternion.Euler(90, 0, 0));
        // 2��ȏ㐶�����Ȃ��悤�ɃR���|�[�l���g��j�����Ă����B�������Ȃ����牺�̏���������
        Destroy(this);
    }
}
