using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworking : NetworkBehaviour {
	
	[SerializeField] private Camera playCam;
	[SerializeField] private Camera miniCam;
	[SerializeField] private AudioListener playerAudio;

	void Start() {
		if (isLocalPlayer) {
			// Deactivate stuff
			GameObject.Find ("Scene Camera").SetActive(false);

			// Enable scripts
			transform.GetComponentInChildren<CameraController>().enabled = true;
			transform.GetComponentInChildren<MinimapController>().enabled = true;
			GetComponent<UserInput>().enabled = true;
			GetComponent<Mouse>().enabled = true;
			GetComponent<HUD>().enabled = true;
			
			// Activate player elemts
			playCam.enabled = true;
			miniCam.enabled = true;
			playerAudio.enabled = true;
		}
	}

}
