using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �͈͓��ɓ������_������
/// </summary>
public class CloudDeleter : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cloud")
        {
            Destroy(other.gameObject);
        }
    }
}
