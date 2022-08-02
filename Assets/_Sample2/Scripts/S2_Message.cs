using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_Message
{
    static Queue<string> texts = new Queue<string>();

    /// <summary>文字列をキューに加える</summary>
    public static void add(string m) => texts.Enqueue(m);

    /// <summary>キューから文字列を取り出す</summary>
    public static string get() => texts.Count > 0 ? texts.Dequeue() : null;

    /// <summary>キューに格納されている文字列の数を返す</summary>
    public static int getCount() => texts.Count;
}
