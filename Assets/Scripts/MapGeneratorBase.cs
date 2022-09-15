using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�𐶐�����X�N���v�g�̒��ۃN���X
/// </summary>
public abstract class MapGeneratorBase : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    /// <summary>
    /// �}�b�v�������������ĕ�����ɂ��ĕԂ�
    /// </summary>
    /// <param name="width">�t���A�̕�</param>
    /// <param name="height">�t���A�̍���</param>
    /// <returns>�t���A�̕�����</returns>
    public abstract string GenerateRandomMap(int width, int height);

    /// <summary>�񎟌��z��𕶎���ɂ��ĕԂ�</summary>
    protected string ArrayToString(string[,] array)
    {
        string str = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
                str += array[i, j];
            if (i < array.GetLength(0) - 1)
                str += '\n';
        }
        //Debug.Log(str); // �f�o�b�O�p�Ɏc���Ă���
        return str;
    }
}
