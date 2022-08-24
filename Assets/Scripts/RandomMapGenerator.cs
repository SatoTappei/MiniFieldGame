using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��敪���@��p���ă}�b�v�𐶐�����
/// </summary>
public class RandomMapGenerator : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>�����_���Ń}�b�v�𐶐����ĕ�����ɂ��ĕԂ�</summary>
    public string GenerateRandomMap(int width, int height)
    {
        // �w�肳�ꂽ���ƍ����Ń}�b�v�̓񎟌��z����쐬����
        string[,] map = new string[width, height];
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                map[i, j] = "W";

        // TODO:�}�b�v����敪�����鏈��
        map = SplitFloor(map);

        // �񎟌��z���string�^�ɂ��ĕԂ�
        string str = "";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
                str += map[i, j];
            if (i != height - 1)
                str += '\n';
        }
        return str;
    }

    /// <summary>���</summary>
    struct Rect
    {
        public int x; // �����X���W
        public int y; // �����Y���W
        public int width; // ��
        public int height; // ����
    }

    // �}�b�v�����ɕ������ĕ�����ŕԂ�
    public string[,] SplitFloor(string[,] map)
    {
        // �c�A�������͉���1~3��������
        // ���������邩
        //int splitNum = Random.Range(1, 4);
        // �c������
        //bool isVert = Random.Range(0, 2) == 0 ? true : false;
        // �[���ǁA�����̒�`��2*2�ȏ�

        // �������������i�[���郊�X�g
        List<Rect> rects = new List<Rect>();
        // �}�b�v�S�̂�1�̋��Ƃ���
        Rect rect;
        rect.x = 0;
        rect.y = 0;
        rect.width = map.GetLength(1);
        rect.height = map.GetLength(0);

        // ���������ɕ������邩�ǂ���
        bool isVert = true;

        while (rect.width * rect.height > 16)
        {
            Debug.Log($"����̍��W{rect.x}:{rect.y} ��{rect.width} ����{rect.height}");
            // ���߂�ꂽ���C����2��������
            int splitLine = Random.Range(4, (isVert ? rect.width : rect.height) - 4);
            // 2����������`���i�[���Ă���
            Rect rect1;
            Rect rect2;

            // �c�ɕ�������ꍇ
            if (isVert)
            {
                // ��������2�����ꂼ��Rect�Ɋi�[����
                rect1.x = rect.x;
                rect1.y = rect.y;
                rect1.width = Mathf.Abs(rect.x - splitLine);
                rect1.height = rect.height;
                rect2.x = splitLine + 1;
                rect2.y = rect.y;
                rect2.width = Mathf.Abs(splitLine - rect.width - 1);
                rect2.height = rect.height;
                Debug.Log("�c����");
            }
            // ���ɕ�������ꍇ
            else
            {
                rect1.x = rect.x;
                rect1.y = rect.y;
                rect1.width = rect.width;
                rect1.height = Mathf.Abs(rect.y - splitLine);
                rect2.x = rect.x;
                rect2.y = splitLine + 1;
                rect2.width = rect.width;
                rect2.height = Mathf.Abs(splitLine - rect.height - 1);
                Debug.Log("������");
            }

            // �������ق������X�g�Ɋi�[
            int area1 = rect1.width * rect1.height;
            int area2 = rect2.width * rect2.height;
            rects.Add(area1 < area2 ? rect1 : rect2);
            // �傫���ق������͕�������
            rect = area1 < area2 ? rect2 : rect1;

            // �c�Ɖ��̕�����؂�ւ���
            isVert = !isVert;

            // ������������\������e�X�g�A����Ȃ��Ȃ��������
            if (area1 < area2)
            {
                for (int i = rect1.y; i < rect1.height; i++)
                    for (int j = rect1.x; j < rect1.width; j++)
                        map[i, j] = "O";
            }
            else
            {
                for (int i = rect2.y; i < rect2.height; i++)
                    for (int j = rect2.x; j < rect2.x + (rect2.width - 1); j++)
                        map[i, j] = "O";
            }
            // �e�X�g�����܂�
        }
        return map;
    }
}
