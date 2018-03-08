using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour {

	private bool fightStarted;

	public Transform player;

	public Canvas cadreText;

	public Text endText;

	private List<string> text;

	private int inc;

	private bool dead;

	private GameObject myCam;

	private GameObject myPlayer;

	private Tracking trackingScript;

	private MovePlayer move;

	private GameObject gameManager;
	
	private GameManagerMain stats;

	// Use this for initialization
	void Start () {
		fightStarted = false;
		dead = false;
		text = new List<string>();

		myCam = GameObject.FindWithTag("MainCamera");
		trackingScript = (Tracking) myCam.GetComponent(typeof(Tracking));

		
		myPlayer = GameObject.FindWithTag("Player");
		move = (MovePlayer) myPlayer.GetComponent(typeof(MovePlayer));

		gameManager = GameObject.FindWithTag("GameManager");
		stats = (GameManagerMain) gameManager.GetComponent(typeof(GameManagerMain));
	}
	
	// Update is called once per frame
	void Update () {
		if(fightStarted==false && isFinalMap()) {
			fightStarted = true;
			inc = 0;
			fight();
		}
		if(isFinalMap() && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
			inc += 1;
			if(text[inc] == "Restarting") { 
				SceneManager.LoadScene("MapLPJ");
			}
            endText.text = text[inc];
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Debug.Log("Quit");
			Application.Quit();
		}
	}

	bool isFinalMap() {
		int x = trackingScript.matriceEnCoursX;
		int y = trackingScript.matriceEnCoursY;
		if(x==1 && y==1) {
			return true;
		}else {
			return false;
		}
	}

	void fight() {
		// Debug.Log("start fight");
		// Debug.Log(player.position);

		//Stop character
		// Debug.Log(move.canMove);
		move.canMove = false;

		//Teleport character to the center of the map
		GameObject map = trackingScript.mapEnCour();
		float x = map.transform.position.x;
		float y = map.transform.position.y;
		player.position = new Vector3(x, y, 0);

		//Display text
		text.Add("A dragon is attacking you! You can not run away, you'll have to fight!");


		text.Add("The dragon spits fire on you.");
		if(stats.fireDefense > 0) {
			text.Add("You protect yourself with your shield of fire.");
		} else if(stats.frostDefense > 0) {
			text.Add("You did not succeed in defending yourself. Your shield of ice melts following the breath of the dragon. You burn and die in horrible sulfur.");
			dead = true;
		} else {
			text.Add("You have no shield to protect you from the dragon's breath! You burn and die in horrible sulfur.");
			dead = true;
		}

		if(!dead) {
			text.Add("It's your turn to hit!");
		}

		if(!dead && stats.frostAttack > 0) {
			text.Add("Your icy sword-blow defeats the dragon!");
		} else if(!dead && stats.fireAttack > 0) {
			text.Add("Your flaming sword is ineffective against the dragon! He devours you ...");
			dead = true;
		} else if(!dead) {
			text.Add("You don't have a sword to fight the dragon. He devours you ...");
			dead = true;
		}

		if(!dead) {
			text.Add("You win this fight, well play!");
		} else {
			text.Add("You loose for this time... Try again!");
		}

		text.Add("Press Enter to restart or Escape to quit");
		text.Add("Restarting");

		endText.text = text[inc];
		cadreText.sortingOrder = 3;

	}
}
