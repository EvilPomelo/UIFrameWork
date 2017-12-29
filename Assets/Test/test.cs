using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public GameObject ga;
	// Use this for initialization
	void Start () {
        foreach (Transform trans in ga.transform)
        {
            print(trans.name);
        }
        print("Next");
        Transform[] tra = ga.GetComponentsInChildren<Transform>();
        foreach (Transform trans in tra)
        {
            print(trans.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
