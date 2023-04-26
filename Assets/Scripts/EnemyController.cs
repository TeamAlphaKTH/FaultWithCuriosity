using UnityEngine;
using UnityEngine.AI;

public class EnemyController:MonoBehaviour {
	private NavMeshAgent enemyAIAgent;
	private GameObject player;

	// Start is called before the first frame update
	void Start() {
		player = GameObject.Find("Player Aaron");
		enemyAIAgent = gameObject.GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update() {
		//Get the current paranoia 10s for distance
		int paranoiaDistance = 102 - (int)Flashlight.currentParanoia * 10 / 10;

		//If the player is further than the paranoiaDistance, move the AI closer.
		if(Vector3.Distance(player.transform.position, transform.position) > paranoiaDistance) {
			enemyAIAgent.destination = player.transform.position;
			enemyAIAgent.stoppingDistance = paranoiaDistance;
		}
	}
}
