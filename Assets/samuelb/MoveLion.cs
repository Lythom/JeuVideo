using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLion : MonoBehaviour {

	public float speed = 1f; // unit per second

	private AnimationCourse ac;

	// Use this for initialization
	void Start () {
		ac = GetComponent<AnimationCourse>();
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 pos = this.transform.position;
		this.transform.position = new Vector3(
			pos.x + horizontal * speed * Time.deltaTime,
			pos.y + vertical * speed * Time.deltaTime,
			pos.z
		);

		ac.SetSpeed(horizontal * speed * Time.deltaTime);
	}
}
