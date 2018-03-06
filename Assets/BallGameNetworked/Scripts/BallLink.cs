using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace networked {

	public class BallLink : MonoBehaviour {

		public Transform ball1;
		public Transform ball2;
		public Vector3 offsetY;

		private void Start () {
			EventManager<Joint2D>.StartListening ("OnJointBreak2D", OnBreak);
		}

		private void OnDestroy () {
			EventManager<Joint2D>.StopListening ("OnJointBreak2D", OnBreak);
		}

		private void OnBreak (Joint2D arg0) {
			if (arg0 == ball1.GetComponent<Joint2D> () || arg0 == ball2.GetComponent<Joint2D> ()) {
				this.gameObject.SetActive (false);
			}
		}

		// Update is called once per frame
		void LateUpdate () {
			if (ball1 == null || ball2 == null) return;
			this.transform.position = (ball1.position + ball2.position) / 2 + offsetY;
			//this.transform.eulerAngles = new Vector3 (0, 0, Vector2.Angle (ball2.position, ball1.position));
			this.transform.right = ball1.position - (transform.position - offsetY);
		}
	}
}