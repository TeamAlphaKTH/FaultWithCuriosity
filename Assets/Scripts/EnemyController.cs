using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController:MonoBehaviour {
	[SerializeField] private float damageMultiplier = 0.1f;
	[SerializeField] private float damageDistance = 3f;
	[SerializeField] private float teleportDistanceMultiplier = 3f;
	[SerializeField] private GameObject scarePointsObject;

	public static float scareDistance;
	private static Transform[] scarePoints;
	private static NavMeshAgent enemyAIAgent;
	public static Transform player;
	private static float distanceToPlayer;
	private float currentParanoia;

	[SerializeField] private AudioSource enemyAudioSource;
	[SerializeField] private AudioClip[] enemyClips;
	private int pickSound;
	private int soundEvent;

	// Start is called before the first frame update
	void Start() {
		enemyAIAgent = gameObject.GetComponent<NavMeshAgent>();
		scareDistance = damageDistance * teleportDistanceMultiplier;
		scarePoints = scarePointsObject.GetComponentsInChildren<Transform>();
	}

	// Update is called once per frame
	void Update() {

		currentParanoia = player.gameObject.GetComponent<Flashlight>().currentParanoia;

		//Get the current paranoia 10s for distance
		int paranoiaDistance = 101 - (int)Math.Floor(currentParanoia);

		distanceToPlayer = Vector3.Distance(player.position, transform.position);

		//If the player is further than the paranoiaDistance, move the AI closer.
		if(distanceToPlayer * 3 > paranoiaDistance) {
			enemyAIAgent.SetDestination(player.position);
			//enemyAIAgent.destination = player.position;
			enemyAIAgent.stoppingDistance = paranoiaDistance;
		} else {
			DealDamage();
		}
	}

	private void FixedUpdate() {
		soundEvent = UnityEngine.Random.Range(0, 1000);
		if(soundEvent == 15) {
			if(!enemyAudioSource.isPlaying) {
				pickSound = UnityEngine.Random.Range(0, enemyClips.Length);
				enemyAudioSource.clip = enemyClips[pickSound];
				enemyAudioSource.Play();
			}
		}
	}

	/// <summary>
	/// Increase the paranoia level of the <see cref="player"/>.
	/// </summary>
	private void DealDamage() {
		if(distanceToPlayer < damageDistance && currentParanoia < 100) {
			player.gameObject.GetComponent<Flashlight>().currentParanoia += (damageDistance - distanceToPlayer) * damageMultiplier;
		}
	}

	/// <summary>
	/// Teleport the <see cref="enemyAIAgent"/> away to the <see cref="scarePoints"/> that is
	/// the furthest away from the <paramref name="currentPosition"/>.
	/// </summary>
	/// <param name="currentPosition">A <see cref="Vector3"/> of the current position of the player.</param>
	public static void ScareTeleport(Vector3 currentPosition) {
		if(distanceToPlayer < scareDistance) {
			float furthestSpawn = Vector3.Distance(scarePoints[1].position, currentPosition);
			Vector3 movePosition = scarePoints[1].position;

			for(int i = 2; i < scarePoints.Length; i++) {
				float distance = Vector3.Distance(scarePoints[i].position, currentPosition);
				if(distance > furthestSpawn) {
					furthestSpawn = distance;
					movePosition = scarePoints[i].position;
				}
			}

			enemyAIAgent.Warp(movePosition);
			enemyAIAgent.SetDestination(movePosition);
		}
	}
}
