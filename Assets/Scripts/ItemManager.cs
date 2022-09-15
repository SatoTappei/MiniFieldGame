using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �t���A�ɗ����Ă���A�C�e���𐧌䂷��
/// </summary>
public class ItemManager : ActorBase
{
    public enum ItemType
    {
        Coin,
        PowerUp,
    }

    /// <summary>���̃A�C�e���̎��</summary>
    [SerializeField] ItemType _itemType;
    /// <summary>�C��:���̃A�C�e�����l�������Ƃ��ɏo�鉉�o</summary>
    [SerializeField] GameObject _getParticle;

    /// <summary>���̃A�C�e���̎�ނ�Ԃ�</summary>
    //public ItemType GetItemType() => _itemType;

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
    public void GetThisItem()
    {
        // TODO:�A�C�e���̎�ނ������Ă�����if�����ጩ�h�����ǂ��Ȃ��̂Œ���
        FindObjectOfType<ActionLogManager>().DispLog(_defeatedMessage);
        if (_itemType == ItemType.Coin)
        {
            FindObjectOfType<PlaySceneManager>().AddScore(100);
            SoundManager._instance.Play("SE_�R�C��");
        }
        if (_itemType == ItemType.PowerUp)
        {
            FindObjectOfType<PlayerManager>().SetPowerUp();
            SoundManager._instance.Play("SE_�p���[�A�b�v");
        }
        FindObjectOfType<MapManager>().CurrentMap.SetMapTileItem(_currentPosXZ.x, _currentPosXZ.z, null);
        SoundManager._instance.Play("SE_�R�C��");
        Instantiate(_getParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
