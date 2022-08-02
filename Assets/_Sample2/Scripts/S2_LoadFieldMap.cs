using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class S2_LoadFieldMap : MonoBehaviour
{
    public string mapName;
    public S2_Field field;

    void Start()
    {
        field.MapReset();
        S2_Array2D mapdata = ReadMapFile(mapName);
        if (mapdata != null)
        {
            field.Create(mapdata);
        }
    }

    void Update()
    {
        
    }

    private S2_Array2D ReadMapFile(string path)
    {
        try
        {
            XDocument xml = XDocument.Load(path);
            XElement map = xml.Element("map");
            S2_Array2D data = null;
            int w = 0;
            int h = 0;
            foreach(XElement layer in map.Elements("layer"))
            {
                switch (layer.Attribute("id").Value)
                {
                    case "1":
                        string[] sdata = (layer.Element("data").Value).Split(',');
                        w = int.Parse(layer.Attribute("width").Value);
                        h = int.Parse(layer.Attribute("height").Value);
                        data = new S2_Array2D(w, h);
                        for(int z = 0; z < h; z++)
                        {
                            for(int x = 0; x < w; x++)
                            {
                                data.Set(x, z, int.Parse(sdata[ToMirrorX(x, w) + z * w]) - 1);
                            }
                        }
                        break;
                }
            }
            return data;
        }
        catch(System.Exception i_exception)
        {
            Debug.LogErrorFormat("{0}", i_exception);
        }
        return null;
    }

    /// <summary>Z���ɑ΂��Ĕ��΂̒l��Ԃ�</summary>
    int ToMirrorX(int xgrid,int mapWidth)
    {
        return mapWidth - xgrid - 1;
    }
}
