using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�̐����𐧌䂷��
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary��������}�b�v�̕�����</summary>
    [TextArea(7, 7), SerializeField] string _mapStr;


    void Start()
    {
        MapGenerator mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateMap(_mapStr);
    }

    void Update()
    {
        
    }
}
