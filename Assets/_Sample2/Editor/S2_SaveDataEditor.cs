using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // <= そういえばこんなのあったけど使っていなかった

public class S2_SaveDataEditor : EditorWindow
{
    string text = "データが見つかりませんでした";
    Vector2 scroll;

    [MenuItem("Window/SaveDataEditor")]
    static void Open()
    {
        GetWindow<S2_SaveDataEditor>("セーブデータ編集");
    }

    string key;

    void OnGUI()
    {
        key = EditorGUILayout.TextField(key);
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
            if (GUILayout.Button("参照")) text = ReadSaveData(key);
            if (GUILayout.Button("保存")) WriteSaveData(key, text);
        }
        EditorGUILayout.EndHorizontal();

        //key = EditorGUILayout.TextField(key);
        scroll = EditorGUILayout.BeginScrollView(scroll);
        text = EditorGUILayout.TextArea(text);
        EditorGUILayout.EndScrollView();
    }

    /// <summary>セーブデータを文字列で取得</summary>
    string ReadSaveData(string k)
    {
        string data = PlayerPrefs.GetString(k).ToString();
        if (data == "") return "データが見つかりませんでした";
        data = JsonPrettyPrint(data);
        return data;
    }

    string JsonPrettyPrint(string i_json)
    {
        if (string.IsNullOrEmpty(i_json))
        {
            return string.Empty;
        }

        int ws = i_json.LastIndexOf("width") + 7;
        int wf = i_json.IndexOf(",", ws) - 1;
        int width = int.Parse(i_json.Substring(ws, wf - ws + 1));
        int ds = i_json.IndexOf("data", wf);
        int i = 0;
        int j = 0;
        i_json = i_json.Replace(System.Environment.NewLine, "").Replace("\t", "");
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        bool quote = false;
        bool ignore = false;
        int offset = 0;
        int indentLength = 3;

        foreach (char ch in i_json)
        {
            switch (ch)
            {
                case '"':
                    if (!ignore)
                    {
                        quote = !quote;
                    }
                    break;
                case '\'':
                    if (quote)
                    {
                        ignore = !ignore;
                    }
                    break;
            }

            if (quote)
            {
                sb.Append(ch);
            }
            else
            {
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        sb.Append(System.Environment.NewLine);
                        sb.Append(new string(' ', ++offset * indentLength));
                        break;
                    case '}':
                    case ']':
                        sb.Append(System.Environment.NewLine);
                        sb.Append(new string(' ', --offset * indentLength));
                        sb.Append(ch);
                        break;
                    case ',':
                        sb.Append(ch);
                        if (i > ds)
                        {
                            if (j < width - 1)
                            {
                                j++;
                            }
                            else
                            {
                                j = 0;
                                sb.Append(System.Environment.NewLine);
                                sb.Append(new string(' ', offset * indentLength));
                            }
                        }
                        else
                        {
                            sb.Append(System.Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        sb.Append(' ');
                        break;
                    default:
                        if (ch != ' ')
                        {
                            sb.Append(ch);
                        }
                        break;
                }
            }
            i++;
        }
        return sb.ToString().Trim();
    }

    /// <summary>セーブデータを書き換える</summary>
    void WriteSaveData(string k, string data)
    {
        Debug.Log(data);
        data.Replace("\n", "");
        Debug.Log(data);
        PlayerPrefs.SetString(k, data);
    }
}
