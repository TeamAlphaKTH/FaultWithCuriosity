using TMPro;
using UnityEngine;

public class StaminaUI:MonoBehaviour {
	[SerializeField] private TextMeshProUGUI stamina = default;

	private void OnEnable() {
		FirstPersonController.OnStaminaChange = updateStamina;
	}

	private void Start() {
		updateStamina(100);
	}

	private void updateStamina(float currentStamina) {
		stamina.text = currentStamina.ToString("00");
	}

}
