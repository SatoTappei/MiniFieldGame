using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�̐����𐧌䂷��
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary��������}�b�v�̕�����</summary>
    [TextArea(7, 7), SerializeField] string _mapStr;

    void Start()
    {
        MapGenerator mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateMap(_mapStr);
        mapGenerator.SetPlayer(canMove: TileType.Floor);

        // �G�^�[���J�n��
        //      �G�𐶐����邩�ǂ����`�F�b�N
        //      �G�𐶐�����
        // �G�^�[���I����
        //      �G�S�����s���������ǂ����`�F�b�N
        //      �G���S���s��������^�[���I��

        // �v���C���[�͕ǂ�ړ��s�̃}�X�ɂ͔z�u�ł��Ȃ� <= �L�����N�^�[���ɐN���s�\�̒n�`��enum�Őݒ肵�Ă��
        // �����ꏊ�ɂ͓G��z�u�ł��Ȃ��̂ŁA�}�X���ɏ�ɓG�������̓v���C���[�����邩�ǂ�������K�v������

    }

    void Update()
    {
        
    }
}
