using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDeCourse : MonoBehaviour {

	public Sprite[] sprites;

	private SpriteRenderer spriteR;

	private int currentSpriteIdx = 0;

	float timeSinceLastSprite;

	float timeBetweenSprite = 0.1f;

	public float speed = 1.5f;

	// Use this for initialization
	void Start () {
		spriteR = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {


/* 		if(Input.GetKey (KeyCode.RightArrow)){

		} */

		 if (Input.GetKey(KeyCode.LeftArrow))
         {
             transform.position += Vector3.left * speed * Time.deltaTime;
			 moveBack();
         }
         if (Input.GetKey(KeyCode.RightArrow))
         {
             transform.position += Vector3.right * speed * Time.deltaTime;
			 move();
         }
         if (Input.GetKey(KeyCode.UpArrow))
         {
             transform.position += Vector3.up * speed * Time.deltaTime;
			 move();
         }
         if (Input.GetKey(KeyCode.DownArrow))
         {
             transform.position += Vector3.down * speed * Time.deltaTime;
			 move();
         }
	}

	void move(){
		Vector3 newDir = new Vector3 (0, 0, 90);
		transform.rotation = Quaternion.LookRotation (newDir);
		timeSinceLastSprite += Time.deltaTime;
		if (timeSinceLastSprite >= timeBetweenSprite) {
			timeSinceLastSprite -= timeBetweenSprite;
			currentSpriteIdx = (currentSpriteIdx + 1) % sprites.Length;
			spriteR.sprite = sprites [currentSpriteIdx];
		}	
	}

	void moveBack(){
		Vector3 newDir = new Vector3(0,0, -90);
		transform.rotation = Quaternion.LookRotation (newDir);
		timeSinceLastSprite += Time.deltaTime;
		if (timeSinceLastSprite >= timeBetweenSprite) {
			timeSinceLastSprite -= timeBetweenSprite;
			currentSpriteIdx = (currentSpriteIdx + 1) % sprites.Length;
			spriteR.sprite = sprites [currentSpriteIdx];
		}	
	}

	
}
