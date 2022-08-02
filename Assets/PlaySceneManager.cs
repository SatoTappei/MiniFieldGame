using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �Q�[���̐i�s���Ǘ��E���䂷��
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    /// <summary>�v���C���[�̃^�[���ɂȂ����疈�^�[������ɌĂ΂��</summary>
    public event UnityAction _playerAction;
    /// <summary>�G�̃^�[���ɂȂ����疈�^�[������ɌĂ΂��</summary>
    public event UnityAction _enemyAction;
    /// <summary>�v���C���[�̍s�����I�������true�ɂȂ�/summary>
    bool _endPlayerTurn;
    /// <summary>�G�S���̍s�����I�������true�ɂȂ�</summary>
    bool _endEnemyTurn;

    public bool EndPlayerTurn { set => _endPlayerTurn = value; }
    public bool EndEnemyTurn { set => _endEnemyTurn = value; }

    void Start()
    {
        StartCoroutine(TurnSystem());
    }

    void Update()
    {
        
    }

    /// <summary>���[�O���C�N�̂悤�ȃ^�[���V�X�e�����s��</summary>
    IEnumerator TurnSystem()
    {
        while (true)
        {
            _playerAction.Invoke();
            yield return new WaitUntil(() => _endPlayerTurn);
            _enemyAction.Invoke();
            yield return new WaitUntil(() => _endEnemyTurn);
        }
    }
}
