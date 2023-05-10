using UnityEngine;

public class SoundTriggerRoom:MonoBehaviour {
	[SerializeField] AudioSource myAudioSource;

	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")) {
			if(!myAudioSource.isPlaying) {
				Debug.Log("Hello");
				myAudioSource.Play();
			}
		}

	}
}
