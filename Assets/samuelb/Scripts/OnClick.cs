using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClick : MonoBehaviour {

	public UnityEvent actionOnClick;

	// Use this for initialization
	void Start () {
		if (actionOnClick == null)
			actionOnClick = new UnityEvent ();
	}

	public void OnMouseDown () {
		actionOnClick.Invoke ();
	}
}