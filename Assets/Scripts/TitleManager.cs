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
        GameManager._instance.FadeOut("GamePlay");
    }
}
