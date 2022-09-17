using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary>
/// ���U���g�̃Z�[�u�⃍�[�h���s��
/// </summary>
public static class SaveDataManager
{
    /// <summary>�����L���O�p�̃X�R�A��z��ŕۑ����邽�߂̃��b�p�[</summary>
    [System.Serializable]
    public class ResultData
    {
        public List<Result> _results = new List<Result>();
    }

    /// <summary>�v���C���[�̃X�R�A�̃N���X</summary>
    [System.Serializable]
    public class Result
    {
        public string _name;
        public int _score;

        public Result(string name, int score)
        {
            _name = name;
            _score = score;
        }
    }

    /// <summary>�����L���O�̕\���Ɏg���ǂݍ��񂾃��U���g�̃f�[�^</summary>
    static ResultData _resultData;

    /// <summary>���݂������U���g���ۑ�����Ă��邩��Ԃ�</summary>
    public static int GetDataCount() => _resultData._results.Count;
    /// <summary>�w�肵�����ʂ̃��U���g��Ԃ�(1�� = 1)</summary>
    public static Result GetRankResult(int rank) => _resultData._results[rank - 1];

    /// <summary>����̃��U���g�̃Z�[�u���s��</summary>
    public static void Save(string name, int score)
    {
        // ����̃f�[�^�����X�g�Ɋi�[����
        _resultData._results.Add(new Result(name, score));
        // ���5�̃f�[�^�̂ݔ����o���ĕۑ�����
        _resultData._results = _resultData._results.OrderByDescending(r => r._score).Take(5).ToList();

        string json = JsonUtility.ToJson(_resultData);
        using (StreamWriter sw = new StreamWriter(Application.dataPath + "/saveData.json"))
        {
            sw.Write(json);
            sw.Flush();
            sw.Close();
        }
    }

    /// <summary>�Z�[�u�f�[�^���烉���L���O�p�̃��U���g�����[�h����</summary>
    public static void Load()
    {
        // ���[�h���Z�[�u�̏��ōs���̂ł��̃^�C�~���O��new����
        _resultData = new ResultData();
        
        try
        {
            using (StreamReader sr = new StreamReader(Application.dataPath + "/saveData.json"))
            {
                string str = sr.ReadToEnd();
                _resultData = JsonUtility.FromJson<ResultData>(str);
            }
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("�Z�[�u�f�[�^�̃t�@�C����������܂���ł���");
        }
    }
}