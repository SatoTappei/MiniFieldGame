using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �v���C���[�̍s���̃��O����ʂɕ\������
/// </summary>
public class ActionLogManager : MonoBehaviour
{
    [SerializeField] GameObject _log;
    [SerializeField] Transform _parent;

    void Start()
    {
        
    }

    void Update()
    {

    }

    /// <summary>�R�C�����l���������O��\��</summary>
    public void DispLog(string message)
    {
        var go = Instantiate(_log, _parent.transform.position, Quaternion.identity, _parent);
        go.GetComponentInChildren<Text>().text = message;

        // �V�����ǉ������e�L�X�g�ȊO��y���W�����炷
        for (int i = 0; i < _parent.childCount - 1; i++)
        {
            Transform trans = _parent.GetChild(i);
            trans.position = new Vector3(trans.position.x, trans.position.y - Screen.height / 18, trans.position.z);
        }

        // ������ʂɕ\������Ă���e�L�X�g��4�ȏ゠��Έ�ԌÂ����̂�����
        if (_parent.childCount >= 4)
        {
            GameObject child = _parent.GetChild(0).gameObject;
            child.GetComponentInChildren<Animator>().Play("FadeOut");
            // �e��0�Ԗڂ̎q��������������Ώۂ̂��̂͐e��ʂ̂��̂ɂ��Ă���
            child.transform.SetParent(transform);
            Destroy(child, 2.0f);
        }
    }
}
