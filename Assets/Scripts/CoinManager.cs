using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t���A�ɗ����Ă���R�C���𐧌䂷��
/// ���݂͊l��������X�R�A�𑝂₷�����Ȃ̂�ActorBase���p�����Ă��Ȃ�
/// </summary>
public class CoinManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // �R���C�_�[�Ō��m����̂ł��̂��߂�����Player�Ƀ��W�b�g�{�f�B�ƃR���C�_�[��t���Ă���
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<PlaySceneManager>().AddScore(100);
        }
    }
}
