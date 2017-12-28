using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIManager.GetInstance().ShowUIForms("LoginUIForm");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
