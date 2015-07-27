using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSync : NetworkBehaviour {

	// Automatically syncs this to all other clients from server through command
	[SyncVar] private Vector3 syncPos;

	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;

	// Update is called once per frame
	void FixedUpdate () {
		TransmitPosition ();
		LerpPosition ();
	}

	void LerpPosition() {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdProvidePositionToServer(Vector3 pos) {
		// Runs in server only
		syncPos = pos;
	}

	[ClientCallback]
	void TransmitPosition() {
		if(isLocalPlayer) {
			CmdProvidePositionToServer(myTransform.position);
		}
	}

}
