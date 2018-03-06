using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace networked {

	public class BallGameNetworked : MonoBehaviour {

		int score1 = 0;
		int score2 = 0;

		public string blueScoreEventName;
		public string orangeScoreEventName;

		public Text score1Txt;
		public Text score2Txt;

		public GameObject ballBluePrefab;
		public GameObject ballOrangePrefab;
		public Vector2 initialBallPos;

		void incr1 (Transform theBall) {
			score1++;
			Transform parent = theBall.parent;
			foreach (Transform child in parent) {
				Destroy (child.gameObject);
			}
			respawnBalls (parent);
			if (score1 >= 10) reset ();
		}
		void incr2 (Transform theBall) {
			score2++;
			Transform parent = theBall.parent;
			foreach (Transform child in parent) {
				Destroy (child.gameObject);
			}
			respawnBalls (parent);
			if (score2 >= 10) reset ();
		}
		void respawnBalls (Transform parent) {
			GameObject newBallBlue = Instantiate (ballBluePrefab, parent);
			GameObject newBallOrange = Instantiate (ballOrangePrefab, parent);
			newBallBlue.transform.position = initialBallPos + Vector2.up;
			newBallOrange.transform.position = initialBallPos + Vector2.down;
			newBallBlue.GetComponent<DistanceJoint2D> ().connectedBody = newBallOrange.GetComponent<Rigidbody2D> ();
			foreach (networked.BallLink ballLink in newBallBlue.GetComponentsInChildren<networked.BallLink> ()) {
				ballLink.ball2 = newBallOrange.transform;
			}
		}
		void reset () {
			score1 = 0;
			score2 = 0;
		}

		// Use this for initialization
		void Start () {
			Debug.Log ("start listening");
			EventManager<Transform>.StartListening (blueScoreEventName, incr1);
			EventManager<Transform>.StartListening (orangeScoreEventName, incr2);
		}

		void Destroy () {
			Debug.Log ("stop listening");
			EventManager<Transform>.StopListening (blueScoreEventName, incr1);
			EventManager<Transform>.StopListening (orangeScoreEventName, incr2);
		}

		// Update is called once per frame
		void Update () {
			if (score1Txt.text != score1.ToString ()) score1Txt.text = score1.ToString ();
			if (score2Txt.text != score2.ToString ()) score2Txt.text = score2.ToString ();
		}
	}
}