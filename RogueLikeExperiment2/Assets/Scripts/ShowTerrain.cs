using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClipperLib;
using VecPath = System.Collections.Generic.List<UnityEngine.Vector2>;
using VecPaths = System.Collections.Generic.List<System.Collections.Generic.List<UnityEngine.Vector2>>;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

public class ShowTerrain : MonoBehaviour{

	SpriteRenderer sr;
	PolygonCollider2D pc;
	Texture2D texture;
	Color clear = new Color (0,0,0,0);
	Color terrain = new Color (.12f, .59f, .7f, 1);
	Color marker = new Color (1, 0, 0, 1);
	// Use this for initialization
	void Start () {
		pc = gameObject.GetComponent<PolygonCollider2D> ();
		sr = gameObject.GetComponent<SpriteRenderer> ();
	}

	public void UpdateTerrainImage (int width, int height, Paths paths, int pixelSize){
		texture = new Texture2D (width, height);
		Color[] colors = new Color[width * height];

		for (int i = 0; i < height; i++) {

			for (int j = 0; j < width; j++) {
				if (pc.OverlapPoint (new Vector2 (j, i))) {
					colors [i * width + j] = terrain;
				}
				else {
					colors [i * width + i] = clear;
				}
			}
		}

		texture.SetPixels (colors);
		texture.alphaIsTransparency = true;

		texture.Apply ();

		sr.sprite = Sprite.Create (texture, new Rect (new Vector2 (0, 0), new Vector2 (width, height)), new Vector2 (.5f, .5f), 1f);

	}

	/*
	 * 
	 * 
	 * For Images based on the center of the object, not the lower left corner.
	 * 
	public void UpdateTerrainImage (int width, int height, Paths paths, int pixelSize) { 
		texture = new Texture2D(width, height);
		Color[] colors = new Color[width * height];
		for (int i = 0; i < height; i++) {
			
			for (int j = 0; j < width; j++){
				if (pc.OverlapPoint (new Vector2 (j - width / 2, i - height / 2))) {
					colors [i * width + j] = terrain;
				}
				else {
					colors [i * width + j] = clear;
				}
			}
		}

		texture.SetPixels (colors);
		texture.alphaIsTransparency = true;

		texture.Apply();

		sr.sprite = Sprite.Create (texture, new Rect (new Vector2 (0, 0), new Vector2 (width, height)), new Vector2 (.5f, .5f), 1f);
	}
	*/
}
