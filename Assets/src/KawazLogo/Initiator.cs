using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class Initiator : MonoBehaviour {

	public int FrameRate;
	public GameObject Sound;
	public GameObject Particle;
	private bool particleEnabled = false;

	void Awake() {
		Application.targetFrameRate = FrameRate;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (SplashScreen.isFinished) {
			AudioSource AS = Sound.GetComponent<AudioSource>();
			if (!AS.isPlaying) {
				AS.Play();
			}
			if (Music.Just.Bar == 1 && Music.Just.Beat == 0 && !particleEnabled) {
				GameObject ps = (GameObject)Instantiate(Particle);
				ps.transform.position = new Vector3(0.0f, 0.0f, 0.5f);
				particleEnabled = true;
			}
		}
	}
}
