using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplodeOnG : MonoBehaviour
{
    private Explodable _explodable;
    public PlayerInput input;

    void Start()
    {
        _explodable = GetComponent<Explodable>();
    }
    void Update()
    {
        if(input.explo)
        {
            _explodable.explode();
            ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
            ef.doExplosion(transform.position);
            Debug.Log("Explode");
        }
        
    }
}
