using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

	public int durationInFrames = 20;
	public float amplitudeInDeg = 20f;

	private bool shaking = false;

	void Start () {
		EventManager.StartListening ("Shake", this.OnShake);
	}

	void OnDestroy()
	{
		EventManager.StopListening ("Shake", this.OnShake);		
	}

	public void OnShake () {
		if (!shaking) StartCoroutine (DoShake ());
	}

	IEnumerator DoShake () {
		shaking = true;
		Quaternion originalRotation = this.transform.rotation;
		for (int i = 0; i < durationInFrames; i++) {
			float angle = CustumEase ((float) i / durationInFrames);
			this.transform.rotation = Quaternion.Euler (0, 0, (angle - 0.5f) * amplitudeInDeg);
			yield return null;
		}
		this.transform.rotation = originalRotation;
		shaking = false;
	}

	// https://github.com/shohei909/tweencore-sharp + http://tweenx.spheresofa.net/core/custom/#%7B%22time%22%3A0.75%2C%22easing%22%3A%5B%22Op%22%2C%5B%22Op%22%2C%5B%22Simple%22%2C%5B%22Standard%22%2C%22Quad%22%2C%22OutIn%22%5D%5D%2C%5B%22Lerp%22%2C1.1%2C0.64%5D%5D%2C%5B%22Op%22%2C%5B%22Op%22%2C%5B%22Op%22%2C%5B%22Simple%22%2C%5B%22Standard%22%2C%22Quad%22%2C%22Out%22%5D%5D%2C%5B%22RoundTrip%22%2C%22Yoyo%22%5D%5D%2C%5B%22Repeat%22%2C2%5D%5D%2C%22Multiply%22%5D%5D%7D
	public static float CustumEase (float rate) {
		return TweenCore.FloatTools.Lerp (TweenCore.Easing.QuadOutIn (rate), 1.1f, 0.64f) * TweenCore.FloatTools.Yoyo (TweenCore.FloatTools.Repeat (TweenCore.FloatTools.Lerp (rate, 0, 2f), 0, 1), TweenCore.Easing.QuadOut);
	}
}