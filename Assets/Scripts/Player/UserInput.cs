using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UserInput : NetworkBehaviour {
	
	[SerializeField] private Transform ball;
	private float ballMoveSpeed = 30;
	
	// Update is called once per frame
	void Update () {
		if (UserUpdatedCameraPosition ()) {
			if(Input.GetKey(KeyCode.A)) {
				// Move camera left
				ball.transform.Translate(Vector3.left * ballMoveSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.D)) {
				ball.transform.Translate(Vector3.right * ballMoveSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.W)) {
				// Move camera left
				ball.transform.Translate(Vector3.forward * ballMoveSpeed * Time.deltaTime);
			}
			if(Input.GetKey(KeyCode.S)) {
				ball.transform.Translate(Vector3.back * ballMoveSpeed * Time.deltaTime);
			}
		}
	}
	
	private bool UserUpdatedCameraPosition() {
		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) {
			return true;
		}
		return false;
	}
}
