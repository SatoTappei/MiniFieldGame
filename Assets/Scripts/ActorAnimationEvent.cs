using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[��"���f����_Root"�I�u�W�F�N�g�ɃA�^�b�`����
/// �A�j���[�V�����C�x���g�ɔC�ӂ̃��\�b�h��o�^���邽��
/// </summary>
public class ActorAnimationEvent : MonoBehaviour
{
    /// <summary>�U���̍ۂɐ�������G�t�F�N�g</summary>
    [SerializeField] GameObject _attackEffect;
    /// <summary>�U���̍ۂɐ�������G�t�F�N�g�̐e</summary>
    [SerializeField] Transform _effectParent;

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>�U������ۂɃG�t�F�N�g���o��</summary>
    public void GenerateAttackEffect()
    {
        if (_attackEffect != null && _effectParent != null)
        {
            var go = Instantiate(_attackEffect, _effectParent.position, Quaternion.identity);
            go.transform.SetParent(_effectParent);
        }
    }

    /// <summary>
    /// �A�j���[�V�������I��������Ƃ�PlaySceneManager�ɒʒm����A
    /// ���݂͍U���̃A�j���[�V�����ɂ̂ݓo�^����Ă���
    /// </summary>
    public void SendAnimationFinish() => FindObjectOfType<PlaySceneManager>().SendEndAction();
}
