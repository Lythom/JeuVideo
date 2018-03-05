using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace networked {

	public class OnCollideShake : MonoBehaviour {

		private void OnCollisionEnter2D (Collision2D other) {
			EventManager<float>.TriggerEvent ("Shake", other.relativeVelocity.magnitude);
		}
	}

}