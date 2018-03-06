using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace networked {

	public class MoveLionPhysicsNetworked : MonoBehaviour {

		public float acceleration = 8f; // unit per second, per second
		public float maxSpeed = 4f; // unit per second
		public Color orangeColor;

		private AnimationCourse ac;
		private Rigidbody2D rb;

		// Use this for initialization
		void Start () {
			// Récupère une référence au script AnimationCourse attaché au même GameObject
			ac = GetComponent<networked.AnimationCourse> ();
			rb = GetComponent<Rigidbody2D> ();
			if (this.transform.position.x > 0) {
				SpriteRenderer sr = GetComponent<SpriteRenderer> ();
				sr.color = orangeColor;
				sr.flipX = true;
			}
		}

		// Update is called once per frame
		void FixedUpdate () {
			if (rb == null) return;

			// Calcule une acceleration en fonction de l'entrée utilisateur et de l'accelération configurée pour l'objet
			// Chaque valeur du Vector3 est exprimée en unité par seconde par seconde
			// celà veut dire que chaque seconde, la vitesse augmente de cette valeur configurée.
			// Ex: Chaque seconde, la vitesse augmente de 8 unités par seconde.
			Vector3 currentAcceleration = new Vector3 (
				Input.GetAxis ("Horizontal") * acceleration,
				Input.GetAxis ("Vertical") * acceleration,
				0
			);

			rb.AddForce (currentAcceleration, ForceMode2D.Force);

			// Vector3.ClampMagnitude permet de limiter la vitesse globale en borant l'amplitude du vecteur de vitesse
			rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxSpeed);
			// Le freinage est simulé par le linear drag du Rigidbody

			// Utilise l'entrée utilisateur pour décider quelle animation afficher.
			// celà permet d'avoir un feedback (retour visuel) immédiat qui lui indique que son
			// action (bouger, ne plus bouger, changer de direction) est prise en compte.
			ac.SetAnimationFromSpeed (Input.GetAxis ("Horizontal") + 0.001f * currentAcceleration.magnitude);
		}
	}
}