using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class S2_JsonEditorWindow : ScriptableWizard
{
    [SerializeField]
    S2_JsonData m_jsonData = null;

    [MenuItem("Tool/JsonEditor")]
    public static void Open()
    {
        DisplayWizard<S2_JsonEditorWindow>("JsonEditor", "Save");
    }

    void OnWizardCreate()
    {
        string json = JsonUtility.ToJson(m_jsonData);
        json = JsonPrettyPrint(json);
        string path = EditorUtility.SaveFilePanel("名前を付けてJsonを保存", "", "Setting", "json");

        System.IO.File.WriteAllText(path, json);

        // プロジェクトフォルダ内に保存された際の対応
        AssetDatabase.Refresh();
    }

    string JsonPrettyPrint(string i_json)
    {
        if (string.IsNullOrEmpty(i_json))
        {
            return string.Empty;
        }

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
                        sb.Append(System.Environment.NewLine);
                        sb.Append(new string(' ', offset * indentLength));
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
        }
        return sb.ToString().Trim();
    }
}
