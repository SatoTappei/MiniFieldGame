using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �v���C���[�֌W��UI�ւ̕\�����s��
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    /// <summary>�o�߃^�[����\������e�L�X�g</summary>
    [SerializeField] Text _dispTurnText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�o�߂����^�[����\������</summary>
    public void SetProgressTurn(int turn)
    {
        _dispTurnText.text = turn.ToString("000");
        // TODO:DotWeen���g�p�����A�j���[�V�������g������
    }
}
