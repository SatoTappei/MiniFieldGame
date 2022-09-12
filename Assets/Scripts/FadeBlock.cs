using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �t�F�[�h�Ɏg���摜
/// </summary>
public class FadeBlock : MonoBehaviour
{
    /// <summary>�F</summary>
    [SerializeField] Color32[] _colors;

    void Awake()
    {
        int r = Random.Range(0, _colors.Length);
        GetComponent<Image>().color = _colors[r];
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
