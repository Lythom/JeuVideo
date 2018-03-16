using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollideBounce : MonoBehaviour {

	private void OnCollisionEnter2D(Collision2D other) {
		
		ContactPoint2D contact = other.contacts[0];
		Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		rb.AddForce(contact.normal * contact.relativeVelocity.magnitude * contact.relativeVelocity.magnitude * 0.4f, ForceMode2D.Impulse);
	}
}
