using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItem : MonoBehaviour {
	public GameObject gameManager;
	/**
		Element of the object : Can be 'Fire' or 'Forst'
	 */
	public string element;
	/**
		Type of the object : Can be 'Defense' or 'Attack'
	 */
	public string type;

	private void OnTriggerEnter2D () {
		Debug.Log("Item picked !");
		if (element == "Fire"){
			if (type == "defense"){
				gameManager.GetComponent<GameManagerMain>().fireDefense += 10;
			} else {
				gameManager.GetComponent<GameManagerMain>().frostDefense += 10;
			}
		} else {
			if (type == "defense"){
				gameManager.GetComponent<GameManagerMain>().fireAttack += 10;
			} else {
				gameManager.GetComponent<GameManagerMain>().frostAttack += 10;
			}
		}
		Destroy(gameObject);
	}
}