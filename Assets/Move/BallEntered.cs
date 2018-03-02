using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEntered : MonoBehaviour {

	public string ballTag;
	public string scoreEventName;

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag(ballTag)) EventManager<Transform>.TriggerEvent (scoreEventName, other.transform);
	}
}