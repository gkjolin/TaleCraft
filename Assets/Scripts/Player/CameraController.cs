using UnityEngine;
using System.Collections;
using RTS;

public class CameraController : MonoBehaviour {
	
	[SerializeField] private Camera playerCam;
	[SerializeField] private Terrain worldTerrain;
	private float worldTerrainPadding = 25f;
	
	// box limit struct
	public struct BoxLimit
	{
		public float leftLimit;
		public float rightLimit;
		public float topLimit;
		public float bottomLimit;
	}
	
	// Define camera and mouse limits
	public static BoxLimit cameraLimits = new BoxLimit();
	public static BoxLimit mouseScrollLimits = new BoxLimit();
	
	void Start() {

		worldTerrain = (Terrain)FindObjectOfType (typeof(Terrain));
		
		// Declare camera limits
		cameraLimits.leftLimit = worldTerrain.transform.position.x + worldTerrainPadding;
		cameraLimits.rightLimit = worldTerrain.terrainData.size.x + worldTerrain.transform.position.x - worldTerrainPadding;
		cameraLimits.topLimit = worldTerrain.terrainData.size.z + worldTerrain.transform.position.z - worldTerrainPadding;
		cameraLimits.bottomLimit = worldTerrain.transform.position.z + worldTerrainPadding;
		
		// Declare mouse limits
		mouseScrollLimits.leftLimit = ResourceManager.CameraMoveTriggerPadding;
		mouseScrollLimits.rightLimit = ResourceManager.CameraMoveTriggerPadding;
		mouseScrollLimits.topLimit = ResourceManager.CameraMoveTriggerPadding;
		mouseScrollLimits.bottomLimit = ResourceManager.CameraMoveTriggerPadding;
		
	}
	
	public void Update() {
		
		if (CheckIfUserCameraInput ()) {
			Vector3 cameraDesiredMove = GetDesiredTranslation ();

			if (!IsDesiredPositionOverBoundaries (cameraDesiredMove)) {
				this.transform.Translate (cameraDesiredMove, Space.World);
				
				Vector3 limitedHeightPosition = this.transform.position;
				
				// Limit height
				if(this.transform.position.y > ResourceManager.MaxCameraHeight) {
					limitedHeightPosition.y = ResourceManager.MaxCameraHeight;
				} else if(this.transform.position.y < ResourceManager.MinCameraHeight) {
					limitedHeightPosition.y = ResourceManager.MinCameraHeight;
				}
				this.transform.position = limitedHeightPosition;
				//				
				//				float smooth = 2f;
				//				float tiltAngle = 15f;
				//				float tiltAroundX = (Input.GetAxis("Mouse ScrollWheel") * tiltAngle) * limitedHeightPosition.y;
				//
				//				Quaternion target = Quaternion.Euler(tiltAroundX, 0, 0);
				//				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, target, Time.deltaTime * smooth);
				
			} else{
				
				// Get the current position
				Vector3 currentPosition = this.transform.position;
				
				// Move the camera to the boundary
				if (
					(Input.GetKey (KeyCode.UpArrow) || Input.mousePosition.y > (Screen.height - mouseScrollLimits.topLimit)) &&
					currentPosition.z > cameraLimits.topLimit
					)
				{
					currentPosition.z = cameraLimits.topLimit;
				}
				if (
					(Input.GetKey (KeyCode.DownArrow) || Input.mousePosition.y < mouseScrollLimits.bottomLimit) &&
					currentPosition.z < cameraLimits.bottomLimit
					)
				{
					currentPosition.z = cameraLimits.bottomLimit;
				}
				if (
					(Input.GetKey (KeyCode.LeftArrow) || Input.mousePosition.x < mouseScrollLimits.leftLimit) &&
					currentPosition.x < cameraLimits.leftLimit
					)
				{
					currentPosition.x = cameraLimits.leftLimit;
				}
				if (
					(Input.GetKey (KeyCode.RightArrow) || Input.mousePosition.x > (Screen.width - mouseScrollLimits.rightLimit)) &&
					currentPosition.x > cameraLimits.rightLimit
					){
					currentPosition.x = cameraLimits.rightLimit;
				}
				
				this.transform.position = currentPosition;
				
			}
		}
		
	}
	
	public void MoveCameraToLocation(Vector3 targetPoint) {
		
		// Keep the height
		targetPoint.y = playerCam.transform.position.y;
		targetPoint.z -= 10f;
		
		// Move the camera
		playerCam.transform.position = targetPoint;
		
	}
	
	public Vector3 GetDesiredTranslation() {
		
		float moveSpeed = 0f;
		Vector3 desiredTranslation = new Vector3 ();
		
		if (Common.ShiftKeysDown ())
			moveSpeed = (ResourceManager.CameraMoveSpeed + ResourceManager.CameraShiftBonusSpeed) * Time.deltaTime;
		else
			moveSpeed = ResourceManager.CameraMoveSpeed * Time.deltaTime;
		
		// Move via keyboard or mouse
		if (Input.GetKey (KeyCode.UpArrow) || Input.mousePosition.y > (Screen.height - mouseScrollLimits.topLimit)) {
			desiredTranslation += Vector3.forward * moveSpeed;
		}
		if (Input.GetKey (KeyCode.DownArrow) || Input.mousePosition.y < mouseScrollLimits.bottomLimit) {
			desiredTranslation += Vector3.back * moveSpeed;
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.mousePosition.x < mouseScrollLimits.leftLimit) {
			desiredTranslation += Vector3.left * moveSpeed;
		}
		if (Input.GetKey (KeyCode.RightArrow) || Input.mousePosition.x > (Screen.width - mouseScrollLimits.rightLimit)) {
			desiredTranslation += Vector3.right * moveSpeed;
		}
		
		// Zoom scroll
		if (IsMouseScrollerScrolled ()){
			desiredTranslation.y -= (Input.GetAxis ("Mouse ScrollWheel") * ResourceManager.CameraScrollSpeed);
		}
		
		return desiredTranslation;
		
	}
	
	public bool CheckIfUserCameraInput() {
		bool keyboardMove = false;
		bool mouseMove = false;
		bool zoomMove = false;
		bool canMove = false;
		
		if (CameraController.AreCameraKeyboardButtonsPressed ()) {
			keyboardMove = true;
		}
		
		if (CameraController.IsMousePositionWithinBounaries ()) {
			mouseMove = true;
		}
		
		if (CameraController.IsMouseScrollerScrolled ()) {
			zoomMove = true;
		}
		
		if (keyboardMove || mouseMove || zoomMove) {
			canMove = true;
		}
		
		return canMove;
	}
	
	public static bool AreCameraKeyboardButtonsPressed() {
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.RightArrow))
			return true;
		else
			return false;
	}
	
	public static bool IsMousePositionWithinBounaries() {
		if (
			(Input.mousePosition.x < mouseScrollLimits.leftLimit && Input.mousePosition.x > -5) ||
			(Input.mousePosition.x > (Screen.width - mouseScrollLimits.rightLimit) && Input.mousePosition.x < (Screen.width + 5)) ||
			(Input.mousePosition.y < mouseScrollLimits.bottomLimit && Input.mousePosition.y > -5) ||
			(Input.mousePosition.y > (Screen.height - mouseScrollLimits.topLimit) && Input.mousePosition.y < (Screen.height + 5))
			)
			return true;
		else
			return false;
	}
	
	public static bool IsMouseScrollerScrolled() {
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			return true;
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			return true;
		else
			return false;
	}
	
	public bool IsDesiredPositionOverBoundaries(Vector3 desiredTranslation) {
		
		Vector3 desiredWorldPosition = this.transform.TransformPoint (desiredTranslation);
		
		bool overBoundaries = false;
		
		// Check boundaries
		if (desiredWorldPosition.x < cameraLimits.leftLimit)
			overBoundaries = true;
		if (desiredWorldPosition.x > cameraLimits.rightLimit)
			overBoundaries = true;
		if (desiredWorldPosition.z > cameraLimits.topLimit)
			overBoundaries = true;
		if (desiredWorldPosition.z < cameraLimits.bottomLimit)
			overBoundaries = true;
		
		return overBoundaries;
		
	}
	
}
