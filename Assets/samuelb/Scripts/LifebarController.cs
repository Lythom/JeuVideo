using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class LifebarController : MonoBehaviour {

	public Transform background;
	public Transform foreground;

	public float hp = 10;
	public float maxHP = 10;

	// Update is called once per frame
	void Update () {
		if (background != null) background.transform.localScale = new Vector3 (maxHP + 0.2f, 1, 1);
		if (foreground != null) foreground.transform.localScale = new Vector3 (Mathf.Clamp (hp, 0, maxHP), 0.8f, 1);
	}

	public void AddHp (float amount) {
		hp += amount;
		EventManager<float>.TriggerEvent("Shake", amount);
	}
}