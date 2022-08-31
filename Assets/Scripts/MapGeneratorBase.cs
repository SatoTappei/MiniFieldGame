using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�𐶐�����X�N���v�g�̒��ۃN���X
/// </summary>
public abstract class MapGeneratorBase : MonoBehaviour
{
    /// <summary>�f�o�b�O�p��R�L�[�ŃV�[���̍ēǂݍ��݂������邩�ǂ���</summary>
    [SerializeField] bool isDebug;

    void Start()
    {
        
    }

    void Update()
    {
        // �}�b�v�������[�h����e�X�g
        if (Input.GetKeyDown(KeyCode.R) && isDebug)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
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
        return str;
    }
}
