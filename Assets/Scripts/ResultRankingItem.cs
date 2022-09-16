using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���U���g��ʂ̃����L���O�Ŋe�v���C���[�̃��U���g��\������I�u�W�F�N�g
/// </summary>
public class ResultRankingItem : MonoBehaviour
{
    /// <summary>�X�R�A�l�[����\������e�L�X�g</summary>
    [SerializeField] Text _nameText;
    /// <summary>�X�R�A��\������e�L�X�g</summary>
    [SerializeField] Text _scoreText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>���U���g���Z�b�g����</summary>
    public void SetResult(string name, int score)
    {
        _nameText.text = name;
        _scoreText.text = score.ToString();
    }
}
