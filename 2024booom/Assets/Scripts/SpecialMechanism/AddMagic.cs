using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMagic : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            MagicLimit.Instance.AddMagic();
            Destroy(gameObject);
        }
        
    }
}
