using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] float lifeTime = 3f;
    [SerializeField] bool isAutoDeactivate;

    WaitForSeconds waitLifeTime;

    private void Awake()
    {
        waitLifeTime = new WaitForSeconds(lifeTime);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());

    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifeTime;
        if(isAutoDeactivate)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
