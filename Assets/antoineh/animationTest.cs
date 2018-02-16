using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCourse : MonoBehaviour {

	public Sprite[] runSprites;
	public Sprite[] jumpSprites;
	private SpriteRenderer spriteRenderer;
	private int currentRunSpriteIndex = 0;
	private int currentJumpSpriteIndex = 0;
	private float time = 0;
	private float bigTime = 0;
	private bool jumping = false;
	public float frameDuration = 0.1f;
	public float bigFrameDuration = 3f;


	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = runSprites[0];
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		bigTime += Time.deltaTime;

		if (bigTime >= bigFrameDuration) {
			jumping = true;
			if (time >= frameDuration && currentJumpSpriteIndex <= jumpSprites.Length - 1) {
				if (currentJumpSpriteIndex < 4) {
					transform.Translate(0f, 0.2f, 0.1f);
				} else {
					transform.Translate(0f, -0.2f, -0.1f);
				}
				currentJumpSpriteIndex = (currentJumpSpriteIndex + 1) % jumpSprites.Length;
				spriteRenderer.sprite = jumpSprites[currentJumpSpriteIndex];
				time -= frameDuration;
			}

			if (currentJumpSpriteIndex == jumpSprites.Length - 1) {
				jumping = false;
				bigTime -= bigFrameDuration;
			}

		} else if (time >= frameDuration && !jumping) {
			currentRunSpriteIndex = (currentRunSpriteIndex + 1) % runSprites.Length;
			spriteRenderer.sprite = runSprites[currentRunSpriteIndex];
			time -= frameDuration;
		}
	}
}
