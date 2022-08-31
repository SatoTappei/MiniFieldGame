using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ”ÍˆÍ“à‚É“ü‚Á‚½‰_‚ğÁ‚·
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
