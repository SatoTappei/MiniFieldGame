using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̃��f����Root�I�u�W�F�N�g�ɃA�^�b�`����
/// �A�j���[�V�����C�x���g�ɔC�ӂ̃��\�b�h��o�^���邽��
/// </summary>
public class ActorPrefabRoot : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// �A�j���[�V�������I��������Ƃ�PlaySceneManager�ɒʒm����A
    /// ���݂͍U���̃A�j���[�V�����ɂ̂ݓo�^����Ă���
    /// </summary>
    public void SendAnimationFinish() => FindObjectOfType<PlaySceneManager>().SendEndAction();

    /// <summary>�U������Ď��ʎ��̉��o�ADead�A�j���[�V�����̍Ō�ɌĂ΂��</summary>
    public void EffectActorIsDead()
    {
        SendAnimationFinish();
        // ���̃X�N���v�g�����Ă���I�u�W�F�N�g�̓L�����̃��f���Ȃ̂Őe���Ə���
        Destroy(transform.parent.gameObject);
    }
}
