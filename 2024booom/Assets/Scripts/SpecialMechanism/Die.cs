using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Invoke("Died", 1.0f);
        }
        
    }

    void Died()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene01");
    }
}
