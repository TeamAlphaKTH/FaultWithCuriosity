using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options:MonoBehaviour {
	private AudioSource[] allSound;
	public static float volumeLevel = 1.0f;
	[SerializeField] private Slider volumeSlider;
	[SerializeField] private TextMeshProUGUI volumeText;
	private void Start() {
		//Initialize GameObjects
		allSound = FindObjectsOfType<AudioSource>();

		if(volumeLevel != 1.0f) {
			ChangeVolume(volumeLevel);
		}
		//Change the text of the current volume settings.
		if(volumeText != null)
			volumeText.text = Mathf.RoundToInt(volumeLevel * 100).ToString();
		if(volumeSlider != null) {
			volumeSlider.value = volumeLevel;
			//When slider changes run ChangeVolume function with the new value.
			volumeSlider.onValueChanged.AddListener(ChangeVolume);
		}

	}
	private void Update() {
		if(allSound.Length == 0)
			allSound = FindObjectsOfType<AudioSource>();
		//Update the slider


	}
	public void ChangeVolume(float arg) {
		volumeLevel = arg;
		foreach(AudioSource source in allSound) {
			source.volume = volumeLevel;
		}
		if(volumeText != null)
			volumeText.text = Mathf.RoundToInt(volumeLevel * 100).ToString();
	}
}
