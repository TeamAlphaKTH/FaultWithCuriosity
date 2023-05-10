using UnityEngine;

public class AnimationController:MonoBehaviour {
	private Animator animator;
	[SerializeField] private float movement = 0f;
	[SerializeField] private bool jump = false;
	[SerializeField] private bool climb = false;
	[SerializeField] private bool afk = false;
	[SerializeField] private bool dead = false;

	// Start is called before the first frame update
	void Start() {
		animator = GetComponent<Animator>();
	}

	// Update is called set amount of time
	void FixedUpdate() {
		animator.SetFloat("Movement", movement);
		animator.SetBool("Jump", jump);
		animator.SetBool("Climb", climb);
		animator.SetBool("AFK", afk);
		animator.SetBool("Dead", dead);
	}
}
