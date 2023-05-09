using Unity.Netcode;
using UnityEngine;

public class Spawner:NetworkBehaviour {
	[SerializeField] GameObject pills;
	[SerializeField] GameObject batteries;
	[ServerRpc(RequireOwnership = false)]
	public void SpawnPillServerRpc(Vector3 pos) {
		Instantiate(pills, pos, Quaternion.identity).GetComponent<NetworkObject>().Spawn();
	}
	[ServerRpc(RequireOwnership = false)]
	public void SpawnBatteryServerRpc(Vector3 pos) {
		Instantiate(batteries, pos, Quaternion.identity).GetComponent<NetworkObject>().Spawn();
	}
}
