using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �^�C�g����ʑS�̂��Ǘ��E���䂷��
/// </summary>
public class TitleManager : MonoBehaviour
{
    // �Q�[�����n�߂�̃{�^��
    [SerializeField] Button _gameStartButton;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�Q�[���v���C�̃V�[���֐��ڂ���</summary>
    public void MoveGamePlay()
    {
        // �t�F�[�h���Ȃ�{�^�����������Ȃ�
        if (GameManager._instance.IsFading) return;
        _gameStartButton.interactable = false;

        // �Q�[���J�n���ɍ��v�X�R�A��0�ɂ���
        GameManager._instance.UpdateTotalScore(0);
        // �Q�[���J�n���Ɍ��݂̃X�e�[�W�ԍ��������l�ɖ߂�
        GameManager._instance.ResetStageNum();
        
        GameManager._instance.FadeOut("GamePlay");
    }
}
