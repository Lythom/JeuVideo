using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableTexture : MonoBehaviour {

	public Color color = Color.blue;
	public Texture2D brush;

	private Texture2D texture;
	// Use this for initialization
	void Start () {
		texture = new Texture2D (160, 160);
		texture.filterMode = FilterMode.Point;
		GetComponent<Renderer> ().material.mainTexture = texture;

		for (int y = 0; y < texture.height; y++) {
			for (int x = 0; x < texture.width; x++) {
				Color color = ((x & y) != 0 ? new Color (1, 1, 1, 0) : Color.black);
				texture.SetPixel (x, y, color);
			}
		}
		texture.Apply ();
		this.GetComponent<Renderer>().sortingLayerName = "AboveBackground";
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDrag () {
		RaycastHit hit;
         if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
             return;
		PaintOn(hit.textureCoord, brush);
		texture.Apply ();
	}

	public void PaintOn (Vector2 textureCoord, Texture2D splashTexture) {
		int x = (int) (textureCoord.x * texture.width) - (splashTexture.width / 2);
		int y = (int) (textureCoord.y * texture.height) - (splashTexture.height / 2);
		for (int i = 0; i < splashTexture.width; ++i)
			for (int j = 0; j < splashTexture.height; ++j) {
				int newX = x + i;
				int newY = y + j;
				Color existingColor = texture.GetPixel (newX, newY);
				Color targetColor = splashTexture.GetPixel (i, j) * color;
				float alpha = targetColor.a;
				if (alpha > 0) {
					Color result = Color.Lerp (existingColor, targetColor, alpha); // resulting color is an addition of splash texture to the texture based on alpha
					result.a = existingColor.a + alpha; // but resulting alpha is a sum of alphas (adding transparent color should not make base color more transparent)
					texture.SetPixel (newX, newY, result);
				}
			}

		texture.Apply ();
	}
}