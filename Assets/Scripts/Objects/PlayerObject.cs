using UnityEngine;
using System.Collections;

public class PlayerObject : WorldObject {
		
	// Governing variables
	public Player player;
	
	public Texture2D buildImage;
	public int cost, hitPoints, maxHitPoints;
	
	// For UserInput.cs
	public Vector2 screenPos;
	public bool onScreen;
	public bool isVisible = false;
	protected bool selected = false;
	
	// For minimap
	public int mapPixelSize = 4;
	
	// GUI
	public Texture minimapUnitTexture;
	
	protected override void Awake() {
		base.Awake();
	}
	
	protected override void Start () {
		base.Start();
		player = transform.root.GetComponent< Player >();
	}
	
	protected override void Update () {
		base.Update();
		
		// if unit not selected, get screen space
		if(!selected) {
			// track the screen position
			screenPos = player.playCam.WorldToScreenPoint(this.transform.position);
			
			// if within the screen space
			if(player.mouse.UnitsWithinScreenSpace(screenPos)) {
				// and not already added to unitsOnScreen, add it!
				if(!onScreen) {
					player.mouse.unitsOnScreen.Add (this.gameObject);
					onScreen = true;
				}
			} else {
				// Unit is not in screen space
				
				// Remove if previously on the screen
				if(onScreen) {
					player.mouse.RemoveFromOnScreenUnits(this.gameObject);
				}
				
			}
		}
	}
	
	protected override void OnGUI() {
		base.OnGUI();

//		if (player.isLocalPlayer || isVisible) {
			// Calculate the screen position of the unit
			Vector2 xyPos = Common.CalculateMinimapPosFromWorldCoordinate (transform.position);
		
			// Place a small gui box over the minimap
			Rect minimapPosition = new Rect (
			xyPos.x,
			xyPos.y,
			mapPixelSize,
			mapPixelSize
			);
		
			if (!minimapUnitTexture) {
				Color useColor = player.color;
				if (player.isLocalPlayer) {
					useColor = Color.green;
				} else {
					useColor = Color.red;
				}

				Common.GUIDrawRect (minimapPosition, useColor);
			} else {
				GUI.DrawTexture (minimapPosition, minimapUnitTexture);
			}
//		}
	}
	
	public override void ObjectGotRightClicked(Player byPlayer) {
		base.ObjectGotRightClicked (byPlayer);
		if (player.isLocalPlayer) {
			// A player right-clicked his own unit
			Debug.Log("A player right-clicked his own unit");
		} else {
			// Another player right-clicked this unit
			Debug.Log("Another player right-clicked this unit");
		}
	}
	
	public virtual void DoRightClickAction(GameObject hitObject) {

	}
	
	public override void PerformAction(string actionToPerform) {
		base.PerformAction (actionToPerform);
	}

	// Helper function to set the selected status of a unit
	public void SetSelected(bool isSelected) {
		// Only allow the local player to set the selected status
		if (player.isLocalPlayer) {
			this.selected = isSelected;
			transform.FindChild ("Selected").gameObject.SetActive(isSelected);
		} else {
			this.selected = false;
			transform.FindChild ("Selected").gameObject.SetActive(false);
		}
	}
	

}
