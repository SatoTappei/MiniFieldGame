using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_Message
{
    static Queue<string> texts = new Queue<string>();

    /// <summary>��������L���[�ɉ�����</summary>
    public static void add(string m) => texts.Enqueue(m);

    /// <summary>�L���[���當��������o��</summary>
    public static string get() => texts.Count > 0 ? texts.Dequeue() : null;

    /// <summary>�L���[�Ɋi�[����Ă��镶����̐���Ԃ�</summary>
    public static int getCount() => texts.Count;
}
