using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���U���g��ʑS�̂��Ǘ��E���䂷��
/// </summary>
public class TitleManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�Q�[���v���C�̃V�[���֐��ڂ���</summary>
    public void MoveGamePlay()
    {
        // �Q�[���J�n���ɍ��v�X�R�A��0�ɂ���
        GameManager._instance.UpdateTotalScore(0);

        GameManager._instance.FadeOut("GamePlay");
    }
}
