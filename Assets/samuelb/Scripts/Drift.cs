using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.eulerAngles += new Vector3(0,0, Input.GetAxis ("Horizontal"));
		if(Input.GetAxis ("Horizontal") == 0) {
			this.transform.eulerAngles = new Vector3(0,0,Mathf.LerpAngle(this.transform.eulerAngles.z, 0, 0.02f));
		}
	}
}
