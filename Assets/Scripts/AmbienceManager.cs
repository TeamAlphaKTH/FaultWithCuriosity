using UnityEngine;

public class AmbienceManager:MonoBehaviour {
	[SerializeField] private AudioSource ambienceSource;
	[SerializeField] private AudioClip[] AmbienceClips;
	private int pickSound;
	private int randomInt;

	// Update is called once per frame
	void FixedUpdate() {
		if(!ambienceSource.isPlaying) {
			randomInt = UnityEngine.Random.Range(0, 1000);
			if(randomInt == 313) {
				pickSound = UnityEngine.Random.Range(0, AmbienceClips.Length);
				ambienceSource.clip = AmbienceClips[pickSound];
				ambienceSource.Play();
			}
		}
	}
}
