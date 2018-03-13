using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Track : MonoBehaviour {

	public Transform target;
	public Vector2 offset;
	public float trackingDistance = 1f;
	public float stiffness = 0.05f;

	void Update () {
		if (target == null) return;

		// Position vers laquelle la caméra doit tendre
		// attention à bien conserver la position Z actuelle
		// on ne poursuit que les positions x et y
		Vector3 targetPos = new Vector3 (
			(target.position.x + offset.x),
			(target.position.y + offset.y),
			this.transform.position.z
		);

		if (stiffness == 1) {
			this.transform.position = targetPos;
			return;
		}

		// l'offset est un déplacement relatif 
		// On va appliquer à la position de la caméra
		// Il est initialisé à zéro (pas de mouvement)
		Vector3 moveOffset = Vector3.zero;

		// Si la cible s'éoigne de la distance de Tracking
		// Le offset va prendre une valeur pour déplacer la Caméra vers la cible
		// Attention, ici on utiliser Vector2.Distance plutôt que Vector3.Distance, 
		//		      car seule la distance des coordonnées x et y nous intéresse.
		//            Avec Vector3.Distance la distance en Z serait prise en compte aussi
		//            ce qui n'est pas souhaité.
		//            Les Vector3 passés en paramètre sont transformé automatique en Vector2.
		if (Vector2.Distance (target.position, this.transform.position) > trackingDistance) {
			// C'est la formule magique du easing joli et rapide à mettre en place
			// Chaque frame, on va déplacer la Caméra de <stiffness*100>% de la distance qui la sépare de la cible
			// La formule est offset = (cible - valeur) * pourcentageDeProgression
			// C'est équivalent à Mathf.Lerp(valeur, cible, pourcentageDeProgression);
			// Pour une Vector3, C'est Vector3.Lerp(position, destination, pourcentageDeProgression)
			moveOffset = (targetPos - new Vector3(this.transform.position.x, this.transform.position.y, 0)) * stiffness;
		}

		// Applique le déplacement à la position en additionnant les vecteurs
		// équivalent de <code>this.transform.position = this.transform.position + moveOffset;</code>
		this.transform.position += new Vector3(moveOffset.x, moveOffset.y, 0);
	}
}