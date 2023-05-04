using Unity.AI.Navigation;
using UnityEngine;

public class BakeNavMesh:MonoBehaviour {
	// Start is called before the first frame update
	void Start() {
		gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
	}

	// Update is called once per frame
	void Update() {

	}
}
