using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour {

	public int decayFrames;
	public float rumbleAmount;
	private int frames;
	private Vector3 initialPosition;
	private System.Random rnd = new System.Random();

	// Use this for initialization
	void Start () {
		initialPosition = this.transform.position;
	}

	// Update is called once per frame
	void Update () {
		frames++;

		// Tremble
		tremble();

		// Decay
		spriteDecay();

		if (frames >= decayFrames) {
			Destroy(gameObject);
		}

	}

	void tremble() {
		float xpos = rumbleAmount * (float)rnd.NextDouble() * 2 - rumbleAmount;
		float ypos = rumbleAmount * (float)rnd.NextDouble() * 2 - rumbleAmount;
		this.transform.position = initialPosition + new Vector3(xpos, ypos, 0);
	}

	void spriteDecay() {
		GameObject effectSprite = this.gameObject.transform.GetChild(0).gameObject;
		Color tmp = effectSprite.GetComponent<SpriteRenderer>().color;
		tmp.a = (float)EaseFunctions.EaseIn(1.0, 0.0, 3, frames, decayFrames, false, false);
		effectSprite.GetComponent<SpriteRenderer>().color = tmp;
	}

}
