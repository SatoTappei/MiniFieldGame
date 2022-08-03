using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class S2_LoadFieldMap : MonoBehaviour
{
    public string mapName;
    public S2_Field field;
    public S2_ActorMovement player;
    public Transform enemies;

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
            // Step4-5 �Ȃ񂩃o�O���Ă�̂œK���ɑ���̏���
            //player.SetPosition(1, 1);
            foreach (var objgp in map.Elements("objectgroup"))
            {
                switch (objgp.Attribute("id").Value)
                {
                    case "2":
                        foreach (var obj in objgp.Elements("object"))
                        {
                            int x = int.Parse(obj.Attribute("x").Value);
                            int z = int.Parse(obj.Attribute("y").Value);
                            int pw = int.Parse(obj.Attribute("width").Value);
                            int ph = int.Parse(obj.Attribute("height").Value);
                            string name = obj.Attribute("name").Value;
                            if (name == "Player")
                            {
                                player.SetPosition(ToMirrorX(x / pw, w), z / ph);
                                continue;
                            }
                            if (name.Contains("Enemy"))
                            {
                                GameObject enemyObj = (GameObject)Resources.Load("Prefabs/" + name);
                                GameObject enemy = Instantiate(enemyObj, enemies.transform); // <= �����Ȃ񂩃o�O���Ă���Aenemies�̎Q�Ƃ��Ȃ�
                                enemy.GetComponent<S2_ActorMovement>().SetPosition(ToMirrorX(x / pw, w), z / ph);
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
