using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEntered : MonoBehaviour {

	public GameObject ball;

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject == ball) Debug.Log("La balle de même couleur est passée !");
	}
}
