using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour {

	public float gridCellWidth = 2;
	public float gridCellHeight = 2;

	void Awake () {
		this.enabled = false;
	}
	
	void Update () {
		#if UNITY_EDITOR
			Vector3 pos = this.transform.position;
			float x = Mathf.Round(pos.x / gridCellWidth);
			float y = Mathf.Round(pos.y / gridCellHeight);
			this.transform.position = new Vector3(x * gridCellWidth, y * gridCellHeight, pos.z);
		#endif
	}
}
