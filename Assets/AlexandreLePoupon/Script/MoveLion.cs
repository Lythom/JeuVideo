using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SBO: les pbs de collisions viennent de ce script : le déplacement est géré à la main (position affectée en dur) alors que les collisions sont gérés par la physique.
// => Faire en sorte que le déplacement soit géré par la physique en modifiant acceleration et velocity plutôt que positions.
public class MoveLion : MonoBehaviour {

	public float acceleration = 8f; // unit per second, per second
	public float maxSpeed = 4f; // unit per second

	public Vector3 currentSpeed;

	private AnimationCourse ac;

	// Use this for initialization
	void Start () {
		// Récupère une référence au script AnimationCourse attaché au même GameObject
		ac = GetComponent<AnimationCourse> ();
	}

	// Update is called once per frame
	void Update () {
		// Calcule une acceleration en fonction de l'entrée utilisateur et de l'accelération configurée pour l'objet
		// Chaque valeur du Vector3 est exprimée en unité par seconde par seconde
		// celà veut dire que chaque seconde, la vitesse augmente de cette valeur configurée.
		// Ex: Chaque seconde, la vitesse augmente de 8 unités par seconde.
		Vector3 currentAcceleration = new Vector3 (
			Input.GetAxis ("Horizontal") * acceleration,
			Input.GetAxis ("Vertical") * acceleration,
			0
		);

		// Calcule la nouvelle vitesse à partir de l'accélération
		// currentAcceleration retourne un changement de vitesse par seconde mais lors d'un update 
		// seulement "Time.deltaTime" secondes se sont écoulées. On applique donc l'accélération proportionellement 
		// au temps écoulé.
		currentSpeed += currentAcceleration * Time.deltaTime;
		// Vector3.ClampMagnitude permet de limiter la vitesse globale en borant l'amplitude du vecteur de vitesse
		currentSpeed = Vector3.ClampMagnitude (currentSpeed, maxSpeed);
		// Simule un freinage lorsque le personnage cesse de se déplacer
		if (currentAcceleration.magnitude == 0) currentSpeed *= 0.8f;

		// finalement, ajoute la vitesse en cours à la position pour créer le mouvement.
		// currentSpeed est exprimé en unités par seconde mais lors d'un update 
		// seulement "Time.deltaTime" secondes se sont écoulées. On applique donc la vitesse proportionellement 
		// au temps écoulé.
		this.transform.position += currentSpeed * Time.deltaTime;

		// Utilise l'entrée utilisateur pour décider quelle animation afficher.
		// celà permet d'avoir un feedback (retour visuel) immédiat qui lui indique que son
		// action (bouger, ne plus bouger, changer de direction) est prise en compte.
		ac.SetAnimationFromSpeed (Input.GetAxis ("Horizontal") + 0.001f * currentAcceleration.magnitude);
	}
}