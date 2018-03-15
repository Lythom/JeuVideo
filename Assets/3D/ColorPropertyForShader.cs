using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPropertyForShader : MonoBehaviour {

	public Color color;
	public float animationSpeed;

	MeshRenderer mrenderer;
	MaterialPropertyBlock props;

	// Use this for initialization
	void Start () {
		mrenderer = GetComponent<MeshRenderer> ();
		props =  new MaterialPropertyBlock ();
	}

	// Update is called once per frame
	void Update () {
		props.SetColor ("_Color", color);		
		props.SetFloat ("_AnimationSpeed", animationSpeed);		
		mrenderer.SetPropertyBlock (props);
	}
}