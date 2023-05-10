using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnCollision : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

	public void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.CompareTag("Player")) {
			Debug.Log("Hello");
			audioSource.Play();
		}
	}
}
