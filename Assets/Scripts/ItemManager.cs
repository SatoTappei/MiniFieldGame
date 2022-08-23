using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t���A�ɗ����Ă���A�C�e���𐧌䂷��
/// </summary>
public class ItemManager : ActorBase
{
    public enum ItemType
    {
        Coin,
    }

    /// <summary>���̃A�C�e���̎��</summary>
    [SerializeField] ItemType _itemType;
    
    /// <summary>���̃A�C�e���̎�ނ�Ԃ�</summary>
    public ItemType GetItemType() => _itemType;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// MapGenerator�Ń}�b�v�������A�z�u�ꏊ�����܂�����
    /// ���[���h���W���^�C����̍��W�ɕϊ����ăZ�b�g����
    /// </summary>
    public override void InitPosXZ()
    {
        _currentPosXZ.x = (int)transform.position.x;
        _currentPosXZ.z = (int)transform.position.z;

        // �������ꂽ���W�Ɏ��g���Z�b�g���čU����ړ��̔���Ɏg����悤�ɂ���
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileItem(_currentPosXZ.x, _currentPosXZ.z, this);
    }

    /// <summary>���̃A�C�e�����擾�����ۂ̏���</summary>
    public void GetThisItemProc()
    {
        FindObjectOfType<ActionLogManager>().DispLog(_defeatedMessage);
        // TODO:���݂̓A�C�e�����R�C�������Ȃ̂ŃX�R�A��ǉ����鏈���������Ă���
        FindObjectOfType<PlaySceneManager>().AddScore(100);
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileItem(_currentPosXZ.x, _currentPosXZ.z, null);
        SoundManager._instance.Play("SE_�R�C��");
        Destroy(gameObject);
        Debug.Log("�R�C�����l��");
    }
}
