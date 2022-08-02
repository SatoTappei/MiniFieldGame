using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̍s���𐧌䂷��
/// </summary>
public class PlayerController : MonoBehaviour
{

    void OnEnable()
    {
        FindObjectOfType<PlaySceneManager>()._playerAction += TurnStart; 
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    /// <summary>�v���C���[�̃^�[�����n�܂�����Ă΂�鏈��</summary>
    void TurnStart()
    {
        Debug.Log("�v���C���[�^�[���J�n");
        StartCoroutine(Action());
    }

    /// <summary>�v���C���[���s��������</summary>
    IEnumerator Action()
    {
        // TODO:�s���̏����������A"�ړ�"��������"�U��"��������v���C���[�̍s���͏I��
        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Space)); // �e�X�g Space�L�[�������܂ő҂�
        PlaySceneManager psm = FindObjectOfType<PlaySceneManager>();
        psm.EndPlayerTurn = true;
    }

    /// <summary>���͂ɑΉ������L�����N�^�[�̌�����Ԃ�</summary>
    ActorDir GetKeyToDir()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");

        if (vert > 0) return ActorDir.Up;
        else if (vert < 0) return ActorDir.Down;
        else if (hori > 0) return ActorDir.Right;
        else if (hori < 0) return ActorDir.Left;
        
        return ActorDir.Neutral;
    }
}
