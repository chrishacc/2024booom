using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestruction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyFragments", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyFragments()
    {
        Destroy(gameObject);
    }
}
