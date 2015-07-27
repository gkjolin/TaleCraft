using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {
	
	[SerializeField] private Camera playerCam;
	private float cameraMoveSpeed = 30;

	// Update is called once per frame
	void Update () {
		if (UserUpdatedCameraPosition ()) {
			if(Input.GetKey(KeyCode.LeftArrow)) {
				// Move camera left
				playerCam.transform.Translate(Vector3.left * cameraMoveSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.RightArrow)) {
				playerCam.transform.Translate(Vector3.right * cameraMoveSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.UpArrow)) {
				// Move camera left
				playerCam.transform.Translate(Vector3.forward * cameraMoveSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.DownArrow)) {
				playerCam.transform.Translate(Vector3.back * cameraMoveSpeed * Time.deltaTime);
			}
		}
	}

	private bool UserUpdatedCameraPosition() {
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) {
			return true;
		}
		return false;
	}
}
