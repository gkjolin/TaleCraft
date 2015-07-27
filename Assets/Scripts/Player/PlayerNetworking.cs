using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworking : NetworkBehaviour {

	[SerializeField] private Camera playerCam;
	[SerializeField] private AudioListener playerAudio;

	void Start() {
		if (isLocalPlayer) {
			// Deactivate stuff
			GameObject.Find ("Scene Camera").SetActive(false);

			// Activate player stuff
			GetComponent<CameraController>().enabled = true;
			GetComponent<UserInput>().enabled = true;
			playerCam.enabled = true;
			playerAudio.enabled = true;
		}
	}

}
