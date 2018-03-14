using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace lepoupon {
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

		private GameObject ambienceSound;

		// private AudioSourceLoop myAudio; // SBO: AudioSourceLoop could not be found :(

		// Use this for initialization
		void Start () {
			fightStarted = false;
			dead = false;
			text = new List<string>();

			// SBO: Préférer une variable public qui sera contribuée via l'éditeur
			// 		Préférer la forme GetComponent<Type>();
			// ex:  public GameObjet myCam;
			//      trackingScript = myCam.GetComponent<Tracking>();
			// celà évite des erreurs au runtime en vérifiant plus de choses à la compilation.
			// L'accès par tag est surtout utile quand on peut avoir une instance changeante pour une même fonction à un instant du jeu. ex: la balle de BallGame qui est détruite puis reconstruite au moment du but.
			myCam = GameObject.FindWithTag("MainCamera");
			trackingScript = (Tracking) myCam.GetComponent(typeof(Tracking));
	
			myPlayer = GameObject.FindWithTag("Player");
			move = (MovePlayer) myPlayer.GetComponent(typeof(MovePlayer));

			gameManager = GameObject.FindWithTag("GameManager");
			stats = (GameManagerMain) gameManager.GetComponent(typeof(GameManagerMain));

			ambienceSound = GameObject.FindWithTag("Sound");
			// myAudio = (AudioSourceLoop) ambienceSound.GetComponent(typeof(AudioSourceLoop));// SBO: AudioSourceLoop could not be found :(
		}
		
		// Update is called once per frame
		void Update () {
			if(fightStarted==false && isFinalMap()) {
				fightStarted = true;
				inc = 0;
				fight();
			}
			// SBO préférer un Input mappé que l'utilisateur pour reconfigurer :
			// Ex: Input.GetButtonDown("Select") qui sera paramétré dans le InputManager.
			if(isFinalMap() && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
				inc += 1;
				if(text[inc] == "Restarting") { 
					SceneManager.LoadScene("MapLPJ");
				}
				endText.text = text[inc];
			}
			if(Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
			}
		}

		bool isFinalMap() {
			// SBO: calcul en fonction du référentiel des cartes
			// Position / taille = transformation du référentiel scène dans le référentiel scènes
			// arrondi = la tuile dont le centre est le plus proche du héro
			int x = Mathf.RoundToInt(myPlayer.transform.position.x / 28);
			int y = Mathf.RoundToInt(myPlayer.transform.position.y / 16);
			if(x==1 && y==1) {
				return true;
			}else {
				return false;
			}
		}

		void fight() {

			//Stop character
			move.canMove = false;

			//myAudio.setIsBossFight(true);

			//Teleport character to the center of the map
			// SBO: quite à ce que ça soit en dur, autant ne pas le cacher
			// SBO: idéalement, prévoir un gameObejct "BossPosition" qui sera placé à la main et qui servira d'ancre de référence pour chaque carte
			player.position = new Vector3(28, 16, 0);

			// SBO: OUI ! Les feedbacks sont très clairs et permettent déjà au joueur de savoir comment réussir sur une prochaine partie
			// Attention pour jeudi après midi : pas sûr que tous les testeurs soient à l'aise avec l'anglais !

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
				text.Add("You win this fight, well played!");
			} else {
				text.Add("You loose for this time... Try again!");
			}

			text.Add("Press Enter to restart or Escape to quit");
			text.Add("Restarting");

			endText.text = text[inc];
			cadreText.sortingOrder = 3;

		}
	}
}