using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(Explodable))]
public abstract class ExplodableAddon : MonoBehaviour {
    //protected Explodable explodable;
    //protected DelayedDestruction delayedDestruction;
	// Use this for initialization
	void Start () {
        //explodable = GetComponent<Explodable>();
        //delayedDestruction = GetComponent<DelayedDestruction>();
	}

    public abstract void OnFragmentsGenerated(List<GameObject> fragments);
}
