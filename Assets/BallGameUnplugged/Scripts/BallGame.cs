using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallGame : MonoBehaviour {

	int score1 = 0;
	int score2 = 0;

	public string blueScoreEventName;
	public string orangeScoreEventName;

	public Text score1Txt;
	public Text score2Txt;

	public GameObject ballSetPrefab;
	public Vector2 initialBallPos;

	void incr1 (Transform theBall) {
		score1++;
		Destroy (theBall.parent.gameObject);
		GameObject newBalls = Instantiate(ballSetPrefab);
		newBalls.transform.position = initialBallPos;
		if (score1 >= 10) reset ();
	}
	void incr2 (Transform theBall) {
		score2++;
		Destroy (theBall.parent.gameObject);
		GameObject newBalls = Instantiate(ballSetPrefab);
		newBalls.transform.position = initialBallPos;
		if (score2 >= 10) reset ();
	}
	void reset () {
		score1 = 0;
		score2 = 0;
	}

	// Use this for initialization
	void Start () {
		EventManager<Transform>.StartListening (blueScoreEventName, incr1);
		EventManager<Transform>.StartListening (orangeScoreEventName, incr2);
	}

	void Destroy () {
		EventManager<Transform>.StopListening (blueScoreEventName, incr1);
		EventManager<Transform>.StopListening (orangeScoreEventName, incr2);
	}

	// Update is called once per frame
	void Update () {
		if (score1Txt.text != score1.ToString ()) score1Txt.text = score1.ToString ();
		if (score2Txt.text != score2.ToString ()) score2Txt.text = score2.ToString ();
	}
}