﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Player : NetworkBehaviour {

	// Public variables
	public Camera playCam;
	public Camera miniCam;
	public Mouse mouse;
	public CameraController cameraControl;
	public MinimapController minimapControl;

	// Player materials
	public int startMaterials;
	public int startEnergy;
	private Dictionary< ResourceType, int > resources;

	public Color color;
	public string username;
	public bool isHuman;

	void Awake() {
		// Initialize resources
		resources = InitResourceList();
	}

	void Start() {
		AddStartResources();
//		hud.SetResourceValues(resources);

		// Hide the minimap
//		if (!isHuman || !isLocalPlayer) {
//			minimapControl.HideMinimap();
//		}
	}
	
	// Update is called once per frame
//	void Update () {
//		if (isHuman && isLocalPlayer) {
//			minimapControl.ShowMinimap ();
//			hud.SetResourceValues (resources);
//		}
//	}
//
//	void OnGUI() {
//		if (this.isHuman && isLocalPlayer) {
//			minimapControl.RenderViewportBox();
//		}
//	}

	private Dictionary< ResourceType, int > InitResourceList() {
		Dictionary< ResourceType, int > list = new Dictionary< ResourceType, int >();
		list.Add(ResourceType.Minerals, 0);
		list.Add(ResourceType.Energy, 0);
		return list;
	}

	private void AddStartResources() {
		AddResource(ResourceType.Minerals, startMaterials);
		AddResource(ResourceType.Energy, startEnergy);
	}

	public void AddResource(ResourceType type, int amount) {
		resources[type] += amount;
	}

//	public void AddUnit(string unitName, Vector3 spawnPoint, Vector3 rallyPoint, Quaternion rotation) {
//		Units units = GetComponentInChildren< Units >();
//		GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
//		newUnit.transform.parent = units.transform;
//
//		// Have the unit move to the rally point
//		if (spawnPoint != rallyPoint) {
//			Unit unitScript = newUnit.GetComponent< Unit >();
//			unitScript.MoveToLocation (rallyPoint);
//		}
//	}

}
