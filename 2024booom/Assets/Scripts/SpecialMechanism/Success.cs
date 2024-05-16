using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Success : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Invoke("Succ", 4.0f);
        }
        
    }



    private void Succ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SuccessScene");
    }
}
